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
        public dbContext() { }

        public dbContext(DbContextOptions<dbContext> options) : base(options) { }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     if(!optionsBuilder.IsConfigured) {
        //         var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        //         var Configuration = new ConfigurationBuilder()
        //             .SetBasePath(AppContext.BaseDirectory)
        //             .AddJsonFile($"appsettings.json", false, true)
        //             .AddJsonFile($"appsettings.{env}.json", true, true)
        //             .AddEnvironmentVariables()
        //             .Build();

        //         optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DBConnection"));
        //         base.OnConfiguring(optionsBuilder);
        //     }
        // } 

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