using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace csye6225.Migrations
{
    public partial class bill_attachment_Fk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "attachment",
                table: "Bill");

            migrationBuilder.AddColumn<Guid>(
                name: "bill_id",
                table: "File",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_File_bill_id",
                table: "File",
                column: "bill_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_File_Bill_bill_id",
                table: "File",
                column: "bill_id",
                principalTable: "Bill",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_Bill_bill_id",
                table: "File");

            migrationBuilder.DropIndex(
                name: "IX_File_bill_id",
                table: "File");

            migrationBuilder.DropColumn(
                name: "bill_id",
                table: "File");

            migrationBuilder.AddColumn<Guid>(
                name: "attachment",
                table: "Bill",
                type: "uuid",
                nullable: true);
        }
    }
}
