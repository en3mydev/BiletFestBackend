using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiletFest.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDBAddVouchers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    VoucherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoucherCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoucherDiscount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.VoucherId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vouchers");
        }
    }
}
