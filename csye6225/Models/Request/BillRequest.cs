using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using csye6225.Common.Enums;
using csye6225.Helpers;

namespace csye6225.Models
{
    public class BillCreateRequest
    {
        [Required]
        public string vendor { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? bill_date { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [BillDueDateValidation(BillDate = "bill_date")]
        public DateTime? due_date { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public Double amount_due { get; set; } 
        
        [Required]
        [UniqueCategoryValidation]
        public List<string> categories { get; set; } 

        [Required]   
        [PaymentStatusEnumValidation]
        public string payment_status { get; set; }
    }

    public class BillUpdateRequest
    {
        [Required]
        public string vendor { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? bill_date { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [BillDueDateValidation(BillDate = "bill_date")]
        public DateTime? due_date { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public Double amount_due { get; set; } 

        [Required]
        [UniqueCategoryValidation]
        public List<string> categories { get; set; }  

        [Required]   
        [PaymentStatusEnumValidation]
        public string payment_status { get; set; }
    }
}