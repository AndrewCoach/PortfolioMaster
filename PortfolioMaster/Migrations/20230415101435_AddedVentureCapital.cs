using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioMaster.Migrations
{
    /// <inheritdoc />
    public partial class AddedVentureCapital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "VentureCapital_TotalValue",
                table: "Assets",
                type: "decimal(18,10)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VentureCapital_TotalValue",
                table: "Assets");
        }
    }
}
