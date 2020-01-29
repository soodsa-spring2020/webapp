using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace csye6225.Models
{
    public class dbContext : DbContext
    {
        public dbContext() { }     

        public dbContext(DbContextOptions<dbContext> options) : base(options) { }     

        public virtual DbSet<AccountModel> Account { get; set; }   

        public virtual DbSet<BillModel> Bill { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
                var connectionString = configuration.GetConnectionString("LocalDBConnection");
                optionsBuilder.UseNpgsql(connectionString);
            }
        }
    }
}