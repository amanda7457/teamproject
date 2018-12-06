using Microsoft.EntityFrameworkCore.Migrations;

namespace Group14_BevoBooks.Migrations
{
    public partial class Migration10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CardNumber",
                table: "CreditCards",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 16);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CardNumber",
                table: "CreditCards",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
