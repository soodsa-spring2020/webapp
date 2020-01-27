using System;
using System.Threading.Tasks;
using AutoMapper;
using csye6225.Common.Enums;
using csye6225.Models;

namespace csye6225.Services
{
    public interface IBillService
    {
        Task<BillResponse> Create(BillCreateRequest req);
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
                bill_date = req.bill_date.Date,
                due_date = req.due_date.Date,
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

    }
}