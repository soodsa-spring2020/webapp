using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace csye6225.Migrations
{
    public partial class account : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    first_name = table.Column<string>(nullable: false),
                    last_name = table.Column<string>(nullable: false),
                    password = table.Column<string>(nullable: false),
                    email_address = table.Column<string>(nullable: false),
                    account_created = table.Column<DateTime>(nullable: true),
                    account_updated = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
