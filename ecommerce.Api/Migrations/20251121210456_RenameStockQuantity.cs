using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ecommerce_crud.Migrations
{
    /// <inheritdoc />
    public partial class RenameStockQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "stockQuantity",
                table: "Products",
                newName: "StockQuantity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StockQuantity",
                table: "Products",
                newName: "stockQuantity");
        }
    }
}
