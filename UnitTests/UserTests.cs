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

namespace UnitTests
{
    public class UserTests
    {
        [Fact]
        public async Task CreateUsers_UnitTest()
        { 
            Random r = new Random();
            var req = new AccountCreateRequest 
            { 
                first_name = "John",
                last_name = "Smith",
                password = "Admin@123", 
                email_address = "john.smith" + r.Next(1, 9999)  +"@example.com"
            };

           var data = new List<AccountModel>
            {
                new AccountModel { 
                first_name = "1John",
                last_name = "1Smith",
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

            var service = new UserService(mockDomain.Object, mockMapper.Object);
            var controller = new UserController(service);
            var actionResult = await controller.Create(req);
            
            Console.WriteLine("CreateUsers_UnitTest {0}", actionResult);
            Assert.IsType<CreatedResult>(actionResult); 
        }

        [Fact]
        public async Task CreateUser_IntiTest()
        {
            var _client = new TestClientProvider().Client;
            //client.DefaultRequestHeaders.Authorization = 
             //new AuthenticationHeaderValue("Basic", "c2FqYWwuc29vZDFAZ21haWwuY29tOkFkbWluQDEyMw==");
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/v1/user");
            Random r = new Random();
            var req = new Dictionary<string, string>
            {
                { "first_name", "John" },
                { "last_name", "Smith" },
                { "password", "Admin@123" },
                { "email_address", "john.smith" + r.Next(1, 9999)  +"@example.com" }
            };
            var json = JsonConvert.SerializeObject(req);
            postRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();
            
            Console.WriteLine("CreateUser_IntiTest {0}", response.StatusCode);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode); 
        }
    }

    public class TestClientProvider
    {
        public HttpClient Client { get; private set; }

        public TestClientProvider()
        {   
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            string curDir = Directory.GetCurrentDirectory();

            var webBuilder = new WebHostBuilder()
            .UseContentRoot(curDir).UseConfiguration(configuration)
            .UseStartup<Startup>();

            var server = new TestServer(webBuilder);

            Client = server.CreateClient();
        }
    }
}
