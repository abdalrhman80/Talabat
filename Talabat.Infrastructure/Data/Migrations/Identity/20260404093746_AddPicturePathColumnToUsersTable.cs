using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Infrastructure.Data.Migrations.Identity
{
    /// <inheritdoc />
    public partial class AddPicturePathColumnToUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PicturePath",
                schema: "auth",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PicturePath",
                schema: "auth",
                table: "Users");
        }
    }
}
