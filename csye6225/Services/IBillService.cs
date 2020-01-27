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
        Task<BillResponse> Create(BillCreateRequest req);
        Task<IEnumerable<BillResponse>> GetUserBills(string ownerId);
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

        public async Task<BillResponse> Create(BillCreateRequest req)
        {
            var bill = new BillModel() 
            { 
                id = Guid.NewGuid(),
                created_ts = DateTime.Now,
                updated_ts = DateTime.Now,
                owner_id = new Guid(req.owner_id),
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
    }
}