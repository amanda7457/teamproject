using Microsoft.EntityFrameworkCore.Migrations;

namespace Group14_BevoBooks.Migrations
{
    public partial class Migration27 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DiscountAmount",
                table: "Discounts",
                newName: "DiscountAmountShipping");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmountPercent",
                table: "Discounts",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountAmountPercent",
                table: "Discounts");

            migrationBuilder.RenameColumn(
                name: "DiscountAmountShipping",
                table: "Discounts",
                newName: "DiscountAmount");
        }
    }
}
