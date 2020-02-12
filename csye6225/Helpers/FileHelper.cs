using System;
using System.IO;
using System.Linq;
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
            var fileName = returnSafeFileName(file.FileName);

            if (file.Length > 0) {
                filePath = Path.Combine(billsFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                    await file.CopyToAsync(fileStream);
                }
            }

            return filePath;
        }

        public static void DeleteBillAttachment(string billId) 
        {
            var dir = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.LastIndexOf("/bin"));
            var billsFolder = Path.Combine(dir, @"tmp/bills", billId);

            if(Directory.Exists(billsFolder)) {
                string[] files = Directory.GetFiles(billsFolder);
                foreach(string f in files) {
                    File.Delete(f);
                }

                Directory.Delete(billsFolder);
            }
        }

        private static string returnSafeFileName(string fileName)
        {
            fileName = fileName.Replace(" ", "_");
            Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c, '_'));
            Path.GetInvalidPathChars().Aggregate(fileName, (current, c) => current.Replace(c, '_'));
            return fileName;
        }
    }
}