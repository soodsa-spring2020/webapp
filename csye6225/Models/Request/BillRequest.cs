using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using csye6225.Common.Enums;
using csye6225.Helpers;

namespace csye6225.Models
{
    public class BillCreateRequest
    {
        public string owner_id { get; set; }

        [Required]
        public string vendor { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? bill_date { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? due_date { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public Double amount_due { get; set; } 
        
        [Required]
        public List<string> categories { get; set; } 

        [Required]   
        [PaymentStatusEnumValidation]
        public string payment_status { get; set; }
    }

    public class BillUpdateRequest
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
        public PaymentStatusEnum payment_status { get; set; }
    }
}