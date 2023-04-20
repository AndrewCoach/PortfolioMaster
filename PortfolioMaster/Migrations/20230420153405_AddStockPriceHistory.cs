using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioMaster.Migrations
{
    /// <inheritdoc />
    public partial class AddStockPriceHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Alpha",
                table: "Assets",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DebtToEquityRatio",
                table: "Assets",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DividendCAGR",
                table: "Assets",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DividendYield",
                table: "Assets",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EarningsPerShare",
                table: "Assets",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "High52WeekPrice",
                table: "Assets",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Low52WeekPrice",
                table: "Assets",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MarketCapitalization",
                table: "Assets",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceToBookRatio",
                table: "Assets",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceToEarningsRatio",
                table: "Assets",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceToSalesRatio",
                table: "Assets",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ReturnOnAssets",
                table: "Assets",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ReturnOnEquity",
                table: "Assets",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StockPriceHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockPriceHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockPriceHistories_Assets_StockId",
                        column: x => x.StockId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockPriceHistories_StockId",
                table: "StockPriceHistories",
                column: "StockId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockPriceHistories");

            migrationBuilder.DropColumn(
                name: "Alpha",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "DebtToEquityRatio",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "DividendCAGR",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "DividendYield",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "EarningsPerShare",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "High52WeekPrice",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "Low52WeekPrice",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "MarketCapitalization",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "PriceToBookRatio",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "PriceToEarningsRatio",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "PriceToSalesRatio",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "ReturnOnAssets",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "ReturnOnEquity",
                table: "Assets");
        }
    }
}
