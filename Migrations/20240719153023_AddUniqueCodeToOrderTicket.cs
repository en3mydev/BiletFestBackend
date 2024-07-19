using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiletFest.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueCodeToOrderTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UniqueCode",
                table: "OrderTickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueCode",
                table: "OrderTickets");
        }
    }
}
