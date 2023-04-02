using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioMaster.Migrations
{
    /// <inheritdoc />
    public partial class RemoveQuantityPurchasePriceFromAssets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Portfolios_PortfolioId",
                table: "Stocks");

            migrationBuilder.DropTable(
                name: "Golds");

            migrationBuilder.DropTable(
                name: "PeerToPeerLoans");

            migrationBuilder.DropTable(
                name: "Silvers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stocks",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_Stocks_PortfolioId",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Stocks");

            migrationBuilder.RenameTable(
                name: "Stocks",
                newName: "Asset");

            migrationBuilder.AlterColumn<string>(
                name: "TickerSymbol",
                table: "Asset",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Exchange",
                table: "Asset",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Asset",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "InterestRate",
                table: "Asset",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MaturityDate",
                table: "Asset",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Asset",
                table: "Asset",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AssetHoldings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PortfolioId = table.Column<int>(type: "int", nullable: false),
                    AssetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetHoldings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetHoldings_Asset_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Asset",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetHoldings_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetHoldings_AssetId",
                table: "AssetHoldings",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetHoldings_PortfolioId",
                table: "AssetHoldings",
                column: "PortfolioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetHoldings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Asset",
                table: "Asset");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Asset");

            migrationBuilder.DropColumn(
                name: "InterestRate",
                table: "Asset");

            migrationBuilder.DropColumn(
                name: "MaturityDate",
                table: "Asset");

            migrationBuilder.RenameTable(
                name: "Asset",
                newName: "Stocks");

            migrationBuilder.AlterColumn<string>(
                name: "TickerSymbol",
                table: "Stocks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Exchange",
                table: "Stocks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "Stocks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "Stocks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "PurchasePrice",
                table: "Stocks",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "Stocks",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stocks",
                table: "Stocks",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Golds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PortfolioId = table.Column<int>(type: "int", nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Golds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Golds_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PeerToPeerLoans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InterestRate = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    MaturityDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PortfolioId = table.Column<int>(type: "int", nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeerToPeerLoans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PeerToPeerLoans_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Silvers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PortfolioId = table.Column<int>(type: "int", nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Silvers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Silvers_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_PortfolioId",
                table: "Stocks",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Golds_PortfolioId",
                table: "Golds",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_PeerToPeerLoans_PortfolioId",
                table: "PeerToPeerLoans",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Silvers_PortfolioId",
                table: "Silvers",
                column: "PortfolioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Portfolios_PortfolioId",
                table: "Stocks",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id");
        }
    }
}
