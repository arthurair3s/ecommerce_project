using Microsoft.EntityFrameworkCore;

namespace ecommerce_crud.DTO
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
        public DbSet<ecommerce_crud.Models.Product> Products { get; set; }
    }
}
