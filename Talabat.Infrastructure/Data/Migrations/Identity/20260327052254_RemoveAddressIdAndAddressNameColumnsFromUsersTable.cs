using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Infrastructure.Data.Migrations.Identity
{
    /// <inheritdoc />
    public partial class RemoveAddressIdAndAddressNameColumnsFromUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_Id",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Address_Name",
                schema: "auth",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Address_Id",
                schema: "auth",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Name",
                schema: "auth",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
