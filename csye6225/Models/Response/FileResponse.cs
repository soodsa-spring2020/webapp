using System;

namespace csye6225.Models
{
    public class FileResponse
    {
        public Guid id { get; set; }
        public string file_name { get; set; }
        public long file_size { get; set; }
        public string file_ext { get; set; }
        public string  url { get; set; }
        public int hash_code { get; set; }
        public DateTime upload_date { get; set; }
    }
}