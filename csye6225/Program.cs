using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace csye6225
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();

            // var host = CreateWebHostBuilder(args).Build();

            // using (var scope = host.Services.CreateScope())
            // {
            //     var db = scope.ServiceProvider.GetService<DbContext>();
            //     db.Database.Migrate();
            // }

            // host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
            .UseKestrel()
            .UseUrls("http://*:5002;http://localhost:5001;http://localhost:5000")
            .UseStartup<Startup>();
    }

}
