using System.ComponentModel.DataAnnotations;

namespace csye6225.Models
{
    public class AuthenticateModel
    {
        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string Password { get; set; }
    }
}