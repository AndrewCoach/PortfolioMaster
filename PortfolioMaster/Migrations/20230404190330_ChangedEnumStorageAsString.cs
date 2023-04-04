using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioMaster.Migrations
{
    /// <inheritdoc />
    public partial class ChangedEnumStorageAsString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MetalType",
                table: "PreciousMetalPrices",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MetalType",
                table: "PreciousMetalPrices",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");
        }
    }
}
