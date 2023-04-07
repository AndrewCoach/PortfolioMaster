using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioMaster.Migrations
{
    /// <inheritdoc />
    public partial class RefactoredAssetHoldingsToHoldTransactionInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PurchasePrice",
                table: "AssetHoldings",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "PurchaseDate",
                table: "AssetHoldings",
                newName: "TransactionDate");

            migrationBuilder.AddColumn<int>(
                name: "TransactionType",
                table: "AssetHoldings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "AssetHoldings");

            migrationBuilder.RenameColumn(
                name: "TransactionDate",
                table: "AssetHoldings",
                newName: "PurchaseDate");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "AssetHoldings",
                newName: "PurchasePrice");
        }
    }
}
