using System;
using System.Collections.Generic;
using csye6225.Common.Enums;

namespace csye6225.Models
{
    public class BillResponse
    {
        public BillResponse() 
        {
            categories = new List<string>();
        }

        public Guid id { get; set; }
        public DateTime? created_ts { get; set; }
        public DateTime? updated_ts { get; set; }
        public Guid owner_id { get; set; }
        public string vendor { get; set; }
        public DateTime bill_date { get; set; }
        public DateTime due_date { get; set; }
        public Double amount_due { get; set; } 
        public List<string> categories { get; set; }    
        public string payment_status { get; set; }
        public FileResponse attachment { get; set; }
    }
}