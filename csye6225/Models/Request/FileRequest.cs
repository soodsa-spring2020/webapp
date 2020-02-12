using System.ComponentModel.DataAnnotations;
using csye6225.Helpers;
using Microsoft.AspNetCore.Http;

namespace csye6225.Models
{
    public class FileCreateRequest
    {
        [Required]
        [FilePathExtensions("pdf,png,jpg,jpeg", ErrorMessage = "Please select a valid file of type - 'pdf, png, jpg, jpeg'.")]
        public IFormFile file { get; set; }
    }
}