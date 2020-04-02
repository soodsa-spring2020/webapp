using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using csye6225.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace csye6225.Services
{
    public interface IFileService
    {
       Task<GetObjectResponse> UploadBillAttachment(string billId, IFormFile file);
       Task DeleteBillAttachment(string billId, string fileName);
    }

    public class FileService : IFileService
    {
        private readonly IOptions<Parameters> _config;
        private IAmazonS3 _client;

        public FileService(IOptions<Parameters> config, IAmazonS3 client) 
        {
            _config = config;
            _client = client;
        }

        public async Task<GetObjectResponse> UploadBillAttachment(string billId, IFormFile file) 
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

            GetObjectResponse fileObj = null;
            var fileName = ReturnSafeFileName(file.FileName);

            if (file.Length > 0) {
                var filePath = Path.Combine(billsFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                    await file.CopyToAsync(fileStream);
                }

                await SendFileToS3(filePath, _config.Value.BILL_BUCKET_NAME, @"bills/" + billId, file.FileName);
                fileObj = await GetFileObjectFromS3(_config.Value.BILL_BUCKET_NAME, @"bills/" + billId, file.FileName);
                
                if(fileObj != null) {
                    string[] files = Directory.GetFiles(billsFolder);
                    foreach(string f in files) {
                        File.Delete(f);
                    }
                    Directory.Delete(billsFolder);
                }
            }
            return fileObj;
        }

        private async Task SendFileToS3(string localFilePath, string bucketName, string subDirectoryInBucket, string fileNameInS3)  
        {  
            TransferUtility utility = new TransferUtility(_client);
            TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

            request.BucketName = bucketName;
            if (!String.IsNullOrEmpty(subDirectoryInBucket))  
                request.BucketName = bucketName + @"/" + subDirectoryInBucket;  
           
            request.Key = fileNameInS3; 
            request.FilePath = localFilePath;  
            await utility.UploadAsync(request);
        }

        private async Task<GetObjectResponse> GetFileObjectFromS3(string bucketName, string subDirectoryInBucket, string fileName)
        {
           if (!String.IsNullOrEmpty(subDirectoryInBucket))  
                bucketName = bucketName + @"/" + subDirectoryInBucket;

            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = fileName
            };

            return await _client.GetObjectAsync(request);
        }

        public async Task DeleteBillAttachment(string billId, string fileName) 
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
            
            await DeleteFileFromS3(_config.Value.BILL_BUCKET_NAME, billId, fileName);
        }

        private async Task DeleteFileFromS3(string bucketName, string billId, string fileName)  
        {  
            string keyName = @"bills/" + billId + "/" + fileName;
            await _client.DeleteObjectAsync(bucketName, keyName);
        }

        private string ReturnSafeFileName(string fileName)
        {
            fileName = fileName.Replace(" ", "_");
            Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c, '_'));
            Path.GetInvalidPathChars().Aggregate(fileName, (current, c) => current.Replace(c, '_'));
            return fileName;
        }
    }
}