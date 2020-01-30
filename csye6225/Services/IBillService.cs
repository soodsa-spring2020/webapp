using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using System.Linq;
using csye6225.Common.Enums;
using csye6225.Models;

namespace csye6225.Services
{
    public interface IBillService
    {
        Task<BillResponse> Create(string ownerId, BillCreateRequest req);
        Task<IEnumerable<BillResponse>> GetUserBills(string ownerId);
        Task<Boolean> DeleteUserBill(string ownerId, string billId);
        Task<BillResponse> GetBill(string ownerId, string billId);
        Task<BillResponse> Update(string ownerId, string billId, BillUpdateRequest req);
        Boolean CheckIfCategoriesUnique(List<string> categories);
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

            using (var _context = new dbContext()) 
            {
                _context.Bill.Add(bill); 
                await _context.SaveChangesAsync();
            }

            return _mapper.Map<BillResponse>(bill);
        }

        public async Task<IEnumerable<BillResponse>> GetUserBills(string ownerId)
        {
            var bills = await Task.Run(() => _context.Bill.Where(x => x.owner_id.ToString() == ownerId).OrderByDescending(o => o.updated_ts));
            return _mapper.Map<IEnumerable<BillResponse>>(bills);
        }

        public async Task<bool> DeleteUserBill(string ownerId, string billId)
        {
            var bill = await Task.Run(() => _context.Bill.FirstOrDefault(x => x.owner_id.ToString() == ownerId && x.id.ToString() == billId));

            if(bill == null) {
                return false;
            }

            using (var _context = new dbContext()) 
            {
                _context.Bill.Remove(bill); 
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<BillResponse> GetBill(string ownerId, string billId)
        {
            var bill = await Task.Run(() => _context.Bill.FirstOrDefault(x => x.owner_id.ToString() == ownerId && x.id.ToString() == billId));
            return _mapper.Map<BillResponse>(bill);
        }

        public async Task<BillResponse> Update(string ownerId, string billId, BillUpdateRequest req)
        {
            using(_context = new dbContext())
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
        }

        public Boolean CheckIfCategoriesUnique(List<string> categories)
        {
            if(categories.Select(s => s.Trim()).Distinct().Count() != categories.Select(s => s.Trim()).Count()){
                return false;
            }
            return true;
        }
    }
}