using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using System.Linq;
using csye6225.Common.Enums;
using csye6225.Models;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Amazon.S3.Model;

namespace csye6225.Services
{
    public interface IBillService
    {
        Task<BillResponse> Create(string ownerId, BillCreateRequest req);
        Task<IEnumerable<BillResponse>> GetUserBills(string ownerId);
        Task<IEnumerable<BillResponse>> GetUserDueBills(string ownerId, string days);
        Task<bool> DeleteUserBill(string ownerId, string billId);
        Task<BillResponse> GetBill(string ownerId, string billId);
        Task<BillResponse> Update(string ownerId, string billId, BillUpdateRequest req);
        Task<FileResponse> StoreAttachment(string billId, GetObjectResponse fileInfo);
        Task<string> DeleteAttachment(string billId);
    }

    public class BillService : IBillService
    {
        private dbContext _context;
        private readonly IMapper _mapper;

        public BillService(dbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BillResponse> Create(string ownerId, BillCreateRequest req)
        {
            var bill = new BillModel() 
            { 
                id = Guid.NewGuid(),
                created_ts = DateTime.Now,
                updated_ts = DateTime.Now,
                owner_id = new Guid(ownerId),
                vendor = req.vendor,
                bill_date = Convert.ToDateTime(req.bill_date).Date,
                due_date = Convert.ToDateTime(req.due_date).Date,
                amount_due = req.amount_due,
                categories = string.Join(',', req.categories),
                payment_status = (int)((PaymentStatusEnum) Enum.Parse(typeof(PaymentStatusEnum), req.payment_status))
            };

            _context.Bill.Add(bill); 
            await _context.SaveChangesAsync();
            return _mapper.Map<BillResponse>(bill);
        }

        public async Task<IEnumerable<BillResponse>> GetUserBills(string ownerId)
        {
            var bills = await Task.Run(() =>
                _context.Bill.Include(b => b.attachment).Where(x => x.owner_id.ToString() == ownerId)
                .OrderByDescending(o => o.updated_ts)
             );
            return _mapper.Map<IEnumerable<BillResponse>>(bills);
        }

        public async Task<IEnumerable<BillResponse>> GetUserDueBills(string ownerId, string days)
        {
            double addDays = Double.Parse(days);
            var bills = await Task.Run(() =>
                _context.Bill.Where(x => x.owner_id.ToString() == ownerId)
                .Where(x => DateTime.Today <= x.due_date && x.due_date <= DateTime.Today.AddDays(addDays))
                .OrderByDescending(o => o.updated_ts)
            );
            return _mapper.Map<IEnumerable<BillResponse>>(bills);
        }

        public async Task<bool> DeleteUserBill(string ownerId, string billId)
        {
            var bill = await Task.Run(() => _context.Bill.FirstOrDefault(x => x.owner_id.ToString() == ownerId && x.id.ToString() == billId));

            if(bill == null)
                return false;
        
            _context.Bill.Remove(bill); 
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<BillResponse> GetBill(string ownerId, string billId)
        {
            var bill = await Task.Run(() =>
                _context.Bill.Include(b => b.attachment).FirstOrDefault(
                    x => x.owner_id.ToString() == ownerId && x.id.ToString() == billId
                )
            );
            return _mapper.Map<BillResponse>(bill);
        }

        public async Task<BillResponse> Update(string ownerId, string billId, BillUpdateRequest req)
        {
            var bill = await Task.Run(() => _context.Bill.FirstOrDefault(x => x.owner_id.ToString() == ownerId && x.id.ToString() == billId));

            if (bill == null)
                return null;

            bill.updated_ts = DateTime.Now;
            bill.vendor = req.vendor;
            bill.bill_date = Convert.ToDateTime(req.bill_date).Date;
            bill.due_date = Convert.ToDateTime(req.due_date).Date;
            bill.amount_due = req.amount_due;
            bill.categories = string.Join(',', req.categories);
            bill.payment_status = (int)((PaymentStatusEnum) Enum.Parse(typeof(PaymentStatusEnum), req.payment_status));
            await _context.SaveChangesAsync();
            return _mapper.Map<BillResponse>(bill);
        }

        public async Task<FileResponse> StoreAttachment(string billId, GetObjectResponse fileInfo)
        {
            var bill = await Task.Run(() => _context.Bill.FirstOrDefault(x => x.id.ToString() == billId));
            if (bill == null)
                return null;

            var file = await Task.Run(() => _context.File.FirstOrDefault(x => x.bill_id.ToString() == bill.id.ToString()));
            if(file == null) {
                file  = new FileModel();
                file.id = Guid.NewGuid();
                file.bill_id = bill.id;
                _context.File.Add(file);
                bill.attachment = file;
            }

            // var rootDir = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.LastIndexOf("/bin"));
            // file.url = Path.GetRelativePath(rootDir, fileInfo.FullName);
            file.url = String.Format("{0}/{1}/{2}", "bills", billId, fileInfo.Key);
            file.file_name = fileInfo.Key;
            file.file_ext = Path.GetExtension(fileInfo.Key);
            file.file_size = fileInfo.ContentLength;
            file.hash_code = fileInfo.GetHashCode();
            file.upload_date = DateTime.Now;

            await _context.SaveChangesAsync();
            return _mapper.Map<FileResponse>(file);
        }

        public async Task<string> DeleteAttachment(string billId)
        {
            var fileName = string.Empty;
            var file = await Task.Run(() => _context.File.FirstOrDefault(x => x.bill_id.ToString() == billId));
            if(file == null) {
                return fileName;
            }

            fileName = file.file_name;
            _context.File.Remove(file); 
            await _context.SaveChangesAsync();
            return fileName;
        }

    }
}