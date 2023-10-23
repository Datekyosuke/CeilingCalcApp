using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using WebApiDB.Models;

namespace WebApiDB.Context
{
    public class AplicationContext : DbContext
    {
        public AplicationContext(DbContextOptions<AplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Dealer> Dealers { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(u => u.Dealer)
                .WithMany(c => c.Orders)
                .HasForeignKey(u => u.DealerInfoId);
        }
    }


}
