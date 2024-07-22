using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiletFest.Migrations
{
    /// <inheritdoc />
    public partial class NewDB2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FestivalID1",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_FestivalID1",
                table: "Tickets",
                column: "FestivalID1");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Festivals_FestivalID1",
                table: "Tickets",
                column: "FestivalID1",
                principalTable: "Festivals",
                principalColumn: "FestivalID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Festivals_FestivalID1",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_FestivalID1",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "FestivalID1",
                table: "Tickets");
        }
    }
}
