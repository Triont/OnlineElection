using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineElection.Migrations
{
    public partial class init006 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeEnd",
                table: "Elections",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Elections",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTimeEnd",
                table: "Elections");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Elections");
        }
    }
}
