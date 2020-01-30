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
                first_name = "Test 1",
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

            var service = new UserService(mockDomain.Object, mockMapper.Object);
            var controller = new UserController(service);
            var actionResult = await controller.Create(req);
            
            _console.WriteLine("CreateUsers_UnitTest {0}", actionResult);
            Assert.IsType<CreatedResult>(actionResult); 
        }

        [Fact]
        public async Task CreateUser_IntiTest()
        {
            var webBuilder = new WebHostBuilder().UseStartup<Startup>();
            var server = new TestServer(webBuilder);
            var _client = server.CreateClient();
            //client.DefaultRequestHeaders.Authorization = 
             //new AuthenticationHeaderValue("Basic", "c2FqYWwuc29vZDFAZ21haWwuY29tOkFkbWluQDEyMw==");
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/v1/user");
            Random r = new Random();
            var req = new Dictionary<string, string>
            {
                { "first_name", "Test 2" },
                { "last_name", "run" },
                { "password", "Admin@123" },
                { "email_address", "john.smith" + r.Next(1, 9999)  +"@example.com" }
            };
            var json = JsonConvert.SerializeObject(req);
            postRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();
            
            //_console.WriteLine(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
            _console.WriteLine("CreateUser_IntiTest {0}", response.StatusCode);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode); 
        }
    }
}
