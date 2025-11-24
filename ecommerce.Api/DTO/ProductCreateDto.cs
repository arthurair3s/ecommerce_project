using ecommerce_crud.Models.Enums;

namespace ecommerce_crud.DTO
{
    public class ProductCreateDto
    {
        public string Model { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Specifications { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public ProductType Type { get; set; }
    }
}