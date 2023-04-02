using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioMaster.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAndPortfolioModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "Stocks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "Silvers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "PeerToPeerLoans",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "Golds",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Portfolios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portfolios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Portfolios_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_PortfolioId",
                table: "Stocks",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Silvers_PortfolioId",
                table: "Silvers",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_PeerToPeerLoans_PortfolioId",
                table: "PeerToPeerLoans",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Golds_PortfolioId",
                table: "Golds",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolios_UserId",
                table: "Portfolios",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Golds_Portfolios_PortfolioId",
                table: "Golds",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PeerToPeerLoans_Portfolios_PortfolioId",
                table: "PeerToPeerLoans",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Silvers_Portfolios_PortfolioId",
                table: "Silvers",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Portfolios_PortfolioId",
                table: "Stocks",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Golds_Portfolios_PortfolioId",
                table: "Golds");

            migrationBuilder.DropForeignKey(
                name: "FK_PeerToPeerLoans_Portfolios_PortfolioId",
                table: "PeerToPeerLoans");

            migrationBuilder.DropForeignKey(
                name: "FK_Silvers_Portfolios_PortfolioId",
                table: "Silvers");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Portfolios_PortfolioId",
                table: "Stocks");

            migrationBuilder.DropTable(
                name: "Portfolios");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Stocks_PortfolioId",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_Silvers_PortfolioId",
                table: "Silvers");

            migrationBuilder.DropIndex(
                name: "IX_PeerToPeerLoans_PortfolioId",
                table: "PeerToPeerLoans");

            migrationBuilder.DropIndex(
                name: "IX_Golds_PortfolioId",
                table: "Golds");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "Silvers");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "PeerToPeerLoans");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "Golds");
        }
    }
}
