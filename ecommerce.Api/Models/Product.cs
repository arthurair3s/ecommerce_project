using ecommerce_crud.Models.Enums;

namespace ecommerce_crud.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Model { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; } 
        public string Specifications { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public ProductType Type { get; set; }

        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
