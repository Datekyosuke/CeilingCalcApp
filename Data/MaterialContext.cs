using Microsoft.EntityFrameworkCore;
using WebApiDB.Models;

namespace WebApiDB.Data
{
    public class MaterialContext : DbContext
    {
        public MaterialContext(DbContextOptions<MaterialContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Material> Materials { get; set; }
    }
}
