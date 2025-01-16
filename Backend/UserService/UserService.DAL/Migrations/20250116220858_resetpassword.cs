using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.DAL.Migrations
{
    /// <inheritdoc />
    public partial class resetpassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "Accounts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordResetTokenExpiresAt",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "PasswordResetTokenExpiresAt",
                table: "Accounts");
        }
    }
}
