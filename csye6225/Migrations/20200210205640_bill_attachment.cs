using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace csye6225.Migrations
{
    public partial class bill_attachment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "attachment",
                table: "Bill",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    file_name = table.Column<string>(nullable: false),
                    file_size = table.Column<long>(nullable: false),
                    file_ext = table.Column<string>(nullable: false),
                    url = table.Column<string>(nullable: false),
                    hash_code = table.Column<int>(nullable: false),
                    upload_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropColumn(
                name: "attachment",
                table: "Bill");
        }
    }
}
