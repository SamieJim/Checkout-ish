using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Payment.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Card",
                columns: table => new
                {
                    CardNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameOnCard = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cvv = table.Column<int>(type: "int", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Card", x => x.CardNumber);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_Card_CardNumber",
                        column: x => x.CardNumber,
                        principalTable: "Card",
                        principalColumn: "CardNumber",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CardNumber",
                table: "Transaction",
                column: "CardNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Card");
        }
    }
}
