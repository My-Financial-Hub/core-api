using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialHub.Auth.Infra.Data.Migrations
{
    public partial class Adds_User_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    birth_name = table.Column<DateTime>(type: "datetime2", nullable: true),
                    email = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    creation_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    update_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
