using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialHub.Auth.Infra.Data.Migrations
{
    public partial class Add_Credential_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_credential",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    login = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    password = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    creation_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    update_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_credential", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_credential_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_credential_user_id",
                table: "user_credential",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_credential");
        }
    }
}
