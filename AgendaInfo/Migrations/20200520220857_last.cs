using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AgendaInfo.Migrations
{
    public partial class last : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_User_AdminID",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_AdminID",
                table: "User");

            migrationBuilder.DropColumn(
                name: "AdminID",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Service",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "RendezVous",
                maxLength: 244,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DayOff",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    Reason = table.Column<string>(maxLength: 244, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayOff", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DayOff");

            migrationBuilder.AddColumn<int>(
                name: "AdminID",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Service",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "RendezVous",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 244,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_AdminID",
                table: "User",
                column: "AdminID");

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_AdminID",
                table: "User",
                column: "AdminID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
