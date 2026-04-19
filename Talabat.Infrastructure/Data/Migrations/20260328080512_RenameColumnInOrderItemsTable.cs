using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumnInOrderItemsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Product_PictureBath",
                table: "OrderItem",
                newName: "Product_PicturePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Product_PicturePath",
                table: "OrderItem",
                newName: "Product_PictureBath");
        }
    }
}
