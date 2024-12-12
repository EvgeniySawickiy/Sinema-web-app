using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class add_price_to_showtimes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TicketPrice",
                table: "Showtimes",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketPrice",
                table: "Showtimes");
        }
    }
}
