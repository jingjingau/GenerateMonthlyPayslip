using Microsoft.EntityFrameworkCore.Migrations;

namespace Payslip.API.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaxRateLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    TaxRateTypeInternal = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    TaxableIncomeLowerBound = table.Column<int>(type: "int", nullable: false),
                    TaxableIncomeUpperBound = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxRateLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    FinancialYearStart = table.Column<int>(type: "int", nullable: false),
                    FinancialYearEnd = table.Column<int>(type: "int", nullable: false),
                    TaxRateLevelId = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxRates_TaxRateLevels_TaxRateLevelId",
                        column: x => x.TaxRateLevelId,
                        principalTable: "TaxRateLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaxRates_TaxRateLevelId",
                table: "TaxRates",
                column: "TaxRateLevelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxRates");

            migrationBuilder.DropTable(
                name: "TaxRateLevels");
        }
    }
}
