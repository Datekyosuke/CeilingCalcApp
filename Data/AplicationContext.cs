using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using WebApiDB.Models;

namespace WebApiDB.Data
{
    public class AplicationContext : DbContext
    {
        public AplicationContext(DbContextOptions<AplicationContext> options) : base(options)
        {
            
        }
        public DbSet<Dealer> Dealers { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Order> Orders { get; set; }

   
    }


}
