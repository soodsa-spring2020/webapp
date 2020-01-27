using System.ComponentModel.DataAnnotations;

namespace csye6225.Models
{
    public class AccountCreateRequest
    {
        [Required]
        public string first_name { get; set; } 
        
        [Required]
        public string last_name { get; set; }

        [Required]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 9)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "The password must include at least one upper case letter, one lower case letter, and one numeric digit.")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required]
        [EmailAddress] 
        public string email_address { get; set; }
    }

    public class AccountUpdateRequest
    {
        [Required]
        public string first_name { get; set; } 
        
        [Required]
        public string last_name { get; set; }

        [Required]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 9)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "The password must include at least one upper case letter, one lower case letter, and one numeric digit.")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }

}