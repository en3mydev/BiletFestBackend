using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiletFest.Migrations
{
    /// <inheritdoc />
    public partial class NusePoateX : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketCodes_Tickets_TicketID",
                table: "TicketCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Festivals_FestivalID",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketCodes",
                table: "TicketCodes");

            migrationBuilder.RenameTable(
                name: "TicketCodes",
                newName: "TicketCode");

            migrationBuilder.RenameColumn(
                name: "TicketCodeId",
                table: "TicketCode",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_TicketCodes_TicketID",
                table: "TicketCode",
                newName: "IX_TicketCode_TicketID");

            migrationBuilder.AlterColumn<int>(
                name: "TicketID",
                table: "TicketCode",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketCode",
                table: "TicketCode",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketCode_Tickets_TicketID",
                table: "TicketCode",
                column: "TicketID",
                principalTable: "Tickets",
                principalColumn: "TicketID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Festivals_FestivalID",
                table: "Tickets",
                column: "FestivalID",
                principalTable: "Festivals",
                principalColumn: "FestivalID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketCode_Tickets_TicketID",
                table: "TicketCode");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Festivals_FestivalID",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketCode",
                table: "TicketCode");

            migrationBuilder.RenameTable(
                name: "TicketCode",
                newName: "TicketCodes");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "TicketCodes",
                newName: "TicketCodeId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketCode_TicketID",
                table: "TicketCodes",
                newName: "IX_TicketCodes_TicketID");

            migrationBuilder.AlterColumn<int>(
                name: "TicketID",
                table: "TicketCodes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketCodes",
                table: "TicketCodes",
                column: "TicketCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketCodes_Tickets_TicketID",
                table: "TicketCodes",
                column: "TicketID",
                principalTable: "Tickets",
                principalColumn: "TicketID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Festivals_FestivalID",
                table: "Tickets",
                column: "FestivalID",
                principalTable: "Festivals",
                principalColumn: "FestivalID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
