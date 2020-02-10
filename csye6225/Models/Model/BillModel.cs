using System;

namespace csye6225.Models
{
    public class BillModel
    {
        public Guid id { get; set; }
        public DateTime? created_ts { get; set; }
        public DateTime? updated_ts { get; set; }
        public Guid owner_id { get; set; }
        public string vendor { get; set; }
        public DateTime bill_date { get; set; }
        public DateTime due_date { get; set; }
        public Double amount_due { get; set; } 
        public string categories { get; set; }    
        public int payment_status { get; set; }
        public Guid? attachment { get; set; }
    }
}