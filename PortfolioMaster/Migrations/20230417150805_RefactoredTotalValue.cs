using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioMaster.Migrations
{
    /// <inheritdoc />
    public partial class RefactoredTotalValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VentureCapital_TotalValue",
                table: "Assets");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalValue",
                table: "Assets",
                type: "decimal(18,10)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalValue",
                table: "Assets",
                type: "decimal(18,10)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)");

            migrationBuilder.AddColumn<decimal>(
                name: "VentureCapital_TotalValue",
                table: "Assets",
                type: "decimal(18,10)",
                nullable: true);
        }
    }
}
