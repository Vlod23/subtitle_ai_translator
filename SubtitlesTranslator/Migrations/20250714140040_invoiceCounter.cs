using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubtitlesTranslator.Migrations
{
    /// <inheritdoc />
    public partial class invoiceCounter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvoiceCounter",
                columns: table => new
                {
                    Year = table.Column<int>(type: "int", nullable: false),
                    LastNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceCounter", x => x.Year);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceCounter");
        }
    }
}
