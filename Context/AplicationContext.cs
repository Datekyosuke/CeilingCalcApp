using CeilingCalc.Autorization;
using CeilingCalc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiDB.Models;

namespace WebApiDB.Context
{
    public class AplicationContext : IdentityDbContext<ApplicationUser, IdentityRole<long>, long>
    {
        public AplicationContext(DbContextOptions<AplicationContext> options) : base(options)
        {
            
        }

        public DbSet<Dealer> Dealers { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Operator> Operators { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasOne(u => u.Dealer)
                .WithMany(c => c.Orders)
                .HasForeignKey(u => u.DealerId);
            modelBuilder.Entity<OrderDetail>()
                .HasOne(u => u.Order)
                .WithMany(c => c.OrderDetail)
                .HasForeignKey(u => u.OrderId);
            modelBuilder.Entity<OrderDetail>()
                .HasOne(u => u.Material)
                .WithMany(c => c.OrderDetail)
                .HasForeignKey(u => u.MaterialId);
            modelBuilder.Entity<Order>()
               .HasOne(u => u.Operator)
               .WithMany(c => c.Orders)
               .HasForeignKey(u => u.OperatorId);
        }
    }


}
