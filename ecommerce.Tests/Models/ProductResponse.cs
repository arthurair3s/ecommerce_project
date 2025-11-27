using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.Tests.Models
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Model { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public string Specifications { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
