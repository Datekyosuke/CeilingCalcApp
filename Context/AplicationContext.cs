using CeilingCalc.Models;
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
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(u => u.Dealer)
                .WithMany(c => c.Orders)
                .HasForeignKey(u => u.DealerId);
            modelBuilder.Entity<OrderDetail>()
                .HasOne(u => u.Order)
                .WithMany(c => c.OrderDetail)
                .HasForeignKey(u => u.OrderId);
            modelBuilder.Entity<OrderDetail>()
                .Property(p => p.Material)
                .IsRequired();
        }
    }


}
