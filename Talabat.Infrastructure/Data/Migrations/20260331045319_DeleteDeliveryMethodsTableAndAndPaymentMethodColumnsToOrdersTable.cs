using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeleteDeliveryMethodsTableAndAndPaymentMethodColumnsToOrdersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_DeliveryMethod_DeliveryMethodId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "DeliveryMethod");

            migrationBuilder.DropIndex(
                name: "IX_Order_DeliveryMethodId",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "DeliveryMethodId",
                table: "Order",
                newName: "PaymentMethod_Id");

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod_Logo",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod_NameAr",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod_NameEn",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "PaymentMethod_Redirect",
                table: "Order",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod_Logo",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PaymentMethod_NameAr",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PaymentMethod_NameEn",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PaymentMethod_Redirect",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "PaymentMethod_Id",
                table: "Order",
                newName: "DeliveryMethodId");

            migrationBuilder.CreateTable(
                name: "DeliveryMethod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cost = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DeliveryTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryMethod", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_DeliveryMethodId",
                table: "Order",
                column: "DeliveryMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_DeliveryMethod_DeliveryMethodId",
                table: "Order",
                column: "DeliveryMethodId",
                principalTable: "DeliveryMethod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
