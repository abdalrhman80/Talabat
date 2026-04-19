using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Talabat.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDeliveryMethodsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeliveryMethodId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DeliveryMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShortName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DeliveryTime = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryMethods", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "DeliveryMethods",
                columns: new[] { "Id", "Cost", "DeliveryTime", "Description", "ShortName" },
                values: new object[,]
                {
                    { 1, 75.00m, "1-2 Days", "Fastest delivery time", "Express" },
                    { 2, 50.00m, "2-5 Days", "Get it within 5 days", "Standard" },
                    { 3, 25.00m, "5-10 Days", "Slower but cheap", "Economy" },
                    { 4, 0.00m, "1-2 Weeks", "Free! You get what you pay for", "Free Shipping" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryMethodId",
                table: "Orders",
                column: "DeliveryMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryMethods_DeliveryMethodId",
                table: "Orders",
                column: "DeliveryMethodId",
                principalTable: "DeliveryMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryMethods_DeliveryMethodId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "DeliveryMethods");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DeliveryMethodId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryMethodId",
                table: "Orders");
        }
    }
}
