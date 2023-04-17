using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioMaster.Migrations
{
    /// <inheritdoc />
    public partial class AddedCryptoPrices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CryptoAssetPrice",
                table: "CryptoAssetPrice");

            migrationBuilder.RenameTable(
                name: "CryptoAssetPrice",
                newName: "CryptoAssetPrices");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CryptoAssetPrices",
                table: "CryptoAssetPrices",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CryptoAssetPrices",
                table: "CryptoAssetPrices");

            migrationBuilder.RenameTable(
                name: "CryptoAssetPrices",
                newName: "CryptoAssetPrice");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CryptoAssetPrice",
                table: "CryptoAssetPrice",
                column: "Id");
        }
    }
}
