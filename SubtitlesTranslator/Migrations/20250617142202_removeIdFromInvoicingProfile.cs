using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubtitlesTranslator.Migrations
{
    /// <inheritdoc />
    public partial class removeIdFromInvoicingProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "InvoicingProfiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "InvoicingProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
