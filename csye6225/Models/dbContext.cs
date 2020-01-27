using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace csye6225.Models
{
    public class dbContext : DbContext
    {
        public dbContext() { }     

        public dbContext(DbContextOptions<dbContext> options) : base(options) { }     

        public virtual DbSet<AccountModel> Account { get; set; }   

        //public DbSet<BillModel> Bill { get; set; }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseNpgsql("my connection string");
        // }  
    }
}