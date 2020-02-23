using System;
using System.Threading.Tasks;
using csye6225.Services;
using Moq;
using csye6225.Models;
using Xunit;
using csye6225.Controllers;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using csye6225;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Diagnostics;
using Xunit.Abstractions;
using System.Reflection;
using Microsoft.Extensions.Options;

namespace UnitTests
{
    public class UserTests
    {
        ITestOutputHelper _console;

        public UserTests(ITestOutputHelper console)
        {
            this._console = console;
        }
        
        [Fact]
        public async Task CreateUsers_UnitTest()
        { 
            Random r = new Random();
            var req = new AccountCreateRequest 
            { 
                first_name = "Test 171",
                last_name = "run",
                password = "Admin@123", 
                email_address = "john.smith" + r.Next(1, 9999)  +"@example.com"
            };

           var data = new List<AccountModel>
            {
                new AccountModel { 
                first_name = "John",
                last_name = "Smith",
                email_address = "john.smith" + r.Next(1, 9999)  +"@example.com"
            }}.AsQueryable();

            var mockSet = new Mock<DbSet<AccountModel>>();
            mockSet.As<IQueryable<AccountModel>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<AccountModel>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<AccountModel>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<AccountModel>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockDomain = new Mock<dbContext>();
            mockDomain.Setup(c => c.Account).Returns(mockSet.Object);

            var expected = new AccountResponse();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<AccountResponse>(It.IsAny<AccountModel>())).Returns(expected);    

            Parameters p = new Parameters();
            var opt = new Mock<IOptions<Parameters>>();
            opt.Setup(ap => ap.Value).Returns(p);

            var service = new UserService(mockDomain.Object, mockMapper.Object, opt.Object);
            var controller = new UserController(service);
            var actionResult = await controller.Create(req);
            
            _console.WriteLine("CreateUsers_UnitTest {0}", actionResult);
            Assert.IsType<CreatedResult>(actionResult); 
        }

        // [Fact]
        // public async Task CreateUser_IntiTest()
        // {
        //     var webBuilder = new WebHostBuilder().UseStartup<Startup>();
        //     var server = new TestServer(webBuilder);
        //     var _client = server.CreateClient();
        //     //client.DefaultRequestHeaders.Authorization = 
        //      //new AuthenticationHeaderValue("Basic", "c2FqYWwuc29vZDFAZ21haWwuY29tOkFkbWluQDEyMw==");
        //     var postRequest = new HttpRequestMessage(HttpMethod.Post, "/v1/user");
        //     Random r = new Random();
        //     var req = new Dictionary<string, string>
        //     {
        //         { "first_name", "Test" },
        //         { "last_name", "run" },
        //         { "password", "Admin@123" },
        //         { "email_address", "john.smith" + r.Next(1, 9999)  +"@example.com" }
        //     };
        //     var json = JsonConvert.SerializeObject(req);
        //     postRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");
        //     var response = await _client.SendAsync(postRequest);
        //     response.EnsureSuccessStatusCode();
            
        //     //_console.WriteLine(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        //     _console.WriteLine("CreateUser_IntiTest {0}", response.StatusCode);
        //     Assert.Equal(HttpStatusCode.Created, response.StatusCode); 
        // }

        // [Fact]
        // public void UploadFileTest()
        // {
        //     //var file_path = @"../../../tmp/bills/32dd8f95-2819-4354-a2dc-d0abf6aa0e12/Invoice-0000001.pdf";
         
        //     //var dir = Directory.GetParent(Directory.GetCurrentDirectory());
        //     //var dir1 = AppContext.BaseDirectory;
        //     var dir2 = AppContext.BaseDirectory.Substring(0,AppContext.BaseDirectory.LastIndexOf("/bin"));
        //     //var appRoot = dir2.Substring(0,dir2.LastIndexOf("/")+1);

        //     var tempFolder = Path.Combine(dir2, @"tmp");
        //     var billsFolder = Path.Combine(tempFolder, @"bills");
        //     // if(!Directory.Exists(tempFolder)) {
        //     //     Directory.CreateDirectory(tempFolder);
        //     // }

        //     var billFolder = Path.Combine(billsFolder, Guid.NewGuid().ToString());
        //     if(!Directory.Exists(billFolder)) {
        //         Directory.CreateDirectory(billFolder);
        //     }

        //     var file_path = Path.Combine(billFolder, @"Invoice-test01.pdf");
        //     File.Create(file_path);

        //     //string dir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\")) ; //Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        //     //string file_path = Path.Combine(dir, @"tmp/bills/32dd8f95-2819-4354-a2dc-d0abf6aa0e12/Invoice-0000001.pdf");
        //     FileInfo file  = new FileInfo(file_path);

        //     if(file.Exists) {
        //         string file_name = file.FullName;
        //         string ext = file.Extension;
        //         long var3 = file.Length;
        //         string var4 = file.Name;

        //         string var6 = file.Directory.Name; //Bill Name 
        //         int var7 = file.GetHashCode();
        //         string var5 = file.DirectoryName;
        //     }

        //     Assert.Equal(file.Exists, true); 
    
        //     string[] files = Directory.GetFiles(billFolder);
        //     foreach(string f in files) {
        //         File.Delete(f);
        //     }
        //     Directory.Delete(billFolder);
        //     Directory.Delete(billsFolder);
        //     Directory.Delete(tempFolder);
        // }
    }
}
