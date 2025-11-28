using ecommerce_crud.DTO;
using ecommerce_crud.Models.Enums;
using Reqnroll;

namespace ecommerce.Tests.Support
{
    [Binding]
    public class ProductTransforms
    {
        [StepArgumentTransformation]
        public ProductCreateDto TransformTableToProductDto(Table table)
        {
            var row = table.Rows[0];

            return new ProductCreateDto
            {
                Model = row["Model"],
                ReleaseDate = DateTime.SpecifyKind(
                    DateTime.Parse(row["ReleaseDate"]),
                    DateTimeKind.Utc
                ),
                Specifications = row["Specifications"],
                Price = decimal.Parse(row["Price"]),
                StockQuantity = int.Parse(row["StockQuantity"]),
                Type = Enum.Parse<ProductType>(row["Type"])
            };
        }
    }
}