using Microsoft.EntityFrameworkCore;
using WebApiDB.Models;

namespace WebApiDB.Data
{
   
    public class DealerContext :DbContext
    {
        public DealerContext() 
        {
            Database.EnsureCreated();
        }

        public DbSet<Dealer> Dealers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Database=u2028771_testbd;Data Source=server203.hosting.reg.ru;User Id=u2028771_datekyo;Password=m2jl2aoe;",
                new MySqlServerVersion(new Version(5, 7, 27)));
        }
    }

    
}
