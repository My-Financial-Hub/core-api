using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialHub.Core.Infra.Migrations.Migrations
{
    public partial class addbalancetotransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_accounts_account_id",
                table: "transactions");

            migrationBuilder.RenameColumn(
                name: "account_id",
                table: "transactions",
                newName: "balance_id");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_account_id",
                table: "transactions",
                newName: "IX_transactions_balance_id");

            migrationBuilder.CreateIndex(
                name: "IX_balances_id",
                table: "balances",
                column: "id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_balances_balance_id",
                table: "transactions",
                column: "balance_id",
                principalTable: "balances",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_balances_balance_id",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "IX_balances_id",
                table: "balances");

            migrationBuilder.RenameColumn(
                name: "balance_id",
                table: "transactions",
                newName: "account_id");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_balance_id",
                table: "transactions",
                newName: "IX_transactions_account_id");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_accounts_account_id",
                table: "transactions",
                column: "account_id",
                principalTable: "accounts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
