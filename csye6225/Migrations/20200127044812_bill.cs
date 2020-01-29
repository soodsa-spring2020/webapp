using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace csye6225.Migrations
{
    public partial class bill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bill",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    created_ts = table.Column<DateTime>(nullable: true),
                    updated_ts = table.Column<DateTime>(nullable: true),
                    owner_id = table.Column<Guid>(nullable: false),
                    vendor = table.Column<string>(nullable: false),
                    bill_date = table.Column<DateTime>(nullable: false),
                    due_date = table.Column<DateTime>(nullable: false),
                    amount_due = table.Column<double>(nullable: false),
                    categories = table.Column<string>(nullable: false),
                    payment_status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bill", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bill");
        }
    }
}
