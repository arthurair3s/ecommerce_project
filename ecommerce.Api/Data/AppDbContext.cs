using ecommerce_crud.Models;
using Microsoft.EntityFrameworkCore;

namespace ecommerce_crud.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
        public DbSet<Models.Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Product>()
                .Property(p => p.Type)
                .HasConversion<string>();
        }
    }
}
