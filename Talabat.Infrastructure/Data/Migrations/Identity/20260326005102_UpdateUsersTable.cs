using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Infrastructure.Data.Migrations.Identity
{
    /// <inheritdoc />
    public partial class UpdateUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailConfirmationCode",
                schema: "auth",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailConfirmationCodeExpiresAt",
                schema: "auth",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailConfirmationToken",
                schema: "auth",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordResetCode",
                schema: "auth",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordResetCodeExpiresAt",
                schema: "auth",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                schema: "auth",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmationCode",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailConfirmationCodeExpiresAt",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailConfirmationToken",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordResetCode",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordResetCodeExpiresAt",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                schema: "auth",
                table: "Users");
        }
    }
}
