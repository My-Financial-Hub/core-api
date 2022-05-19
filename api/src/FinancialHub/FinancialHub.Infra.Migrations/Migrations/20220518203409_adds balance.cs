using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialHub.Infra.Migrations.Migrations
{
    public partial class addsbalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "currency",
                table: "accounts");

            migrationBuilder.CreateTable(
                name: "Balances",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "varchar(200)", nullable: true),
                    currency = table.Column<string>(type: "varchar(50)", nullable: true),
                    amount = table.Column<decimal>(type: "money", nullable: false),
                    account_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    creation_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    update_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Balances", x => x.id);
                    table.ForeignKey(
                        name: "FK_Balances_accounts_account_id",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Balances_account_id",
                table: "Balances",
                column: "account_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Balances");

            migrationBuilder.AddColumn<string>(
                name: "currency",
                table: "accounts",
                type: "varchar(50)",
                nullable: true);
        }
    }
}
