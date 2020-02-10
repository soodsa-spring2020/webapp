using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace csye6225.Models
{
    public class FileCreateRequest
    {
        [Required]
        public IFormFile file { get; set; }
    }
}