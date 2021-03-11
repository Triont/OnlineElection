using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineElection.Migrations
{
    public partial class init004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailWasConfirmed",
                table: "People",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailWasConfirmed",
                table: "People");
        }
    }
}
