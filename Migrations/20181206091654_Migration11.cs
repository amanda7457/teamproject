using Microsoft.EntityFrameworkCore.Migrations;

namespace Group14_BevoBooks.Migrations
{
    public partial class Migration11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Reviews",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Reviews");
        }
    }
}
