using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace csye6225.Models
{
    public class dbContext : DbContext
    {
        string APP_CONNECTION_STRING = "";  

        public dbContext() { }
        
        public dbContext(IOptions<Parameters> options) { 
            APP_CONNECTION_STRING = options.Value.RDSConnectionString;
        }    

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(String.IsNullOrEmpty(APP_CONNECTION_STRING)) {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile($"appsettings.json")
                    .AddJsonFile($"appsettings.{env}.json")
                    .AddEnvironmentVariables()
                    .Build();

                APP_CONNECTION_STRING = configuration.GetConnectionString("DBConnection");
            }

            optionsBuilder.UseNpgsql(APP_CONNECTION_STRING);
            base.OnConfiguring(optionsBuilder);
        } 


        public virtual DbSet<AccountModel> Account { get; set; }   
        public virtual DbSet<BillModel> Bill { get; set; }
        public virtual DbSet<FileModel> File { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BillModel>()
                .HasOne(b => b.attachment)
                .WithOne(i => i.bill)
                .HasForeignKey<FileModel>(b => b.bill_id);
        }
    }
}