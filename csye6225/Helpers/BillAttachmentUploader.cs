using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace csye6225.Helpers
{
    public static class FileHelper
    {
        public static async Task<string> UploadBillAttachment(string billId, IFormFile file) 
        {
            var dir = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.LastIndexOf("/bin"));
            var billsFolder = Path.Combine(dir, @"tmp/bills", billId);

            if(!Directory.Exists(billsFolder)) {
                Directory.CreateDirectory(billsFolder);
            }
            else{
                //Directory already exists, delete its contents
                string[] files = Directory.GetFiles(billsFolder);
                foreach(string f in files) {
                    File.Delete(f);
                }
            }

            var filePath = string.Empty;
            if (file.Length > 0) {
                filePath = Path.Combine(billsFolder, file.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                    await file.CopyToAsync(fileStream);
                }
            }

            return filePath;
        }
    }
}