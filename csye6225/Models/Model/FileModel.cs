using System;

namespace csye6225.Models
{
    public class FileModel
    {
        public Guid id { get; set; }
        public string file_name { get; set; }
        public string  url { get; set; }
        public DateTime upload_date { get; set; }
        
    }
}