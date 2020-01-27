using System;

namespace csye6225.Models
{
    public class AccountResponse
    {
        public Guid id { get; set; }
        public string first_name { get; set; } 
        public string last_name { get; set; }
        public string email_address { get; set; }
        public DateTime account_created { get; set; }
        public DateTime account_updated { get; set; }
    }
}