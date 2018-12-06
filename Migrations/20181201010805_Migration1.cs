using Microsoft.EntityFrameworkCore.Migrations;

namespace Group14_BevoBooks.Migrations
{
    public partial class Migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreditCards_AspNetUsers_AppUserId",
                table: "CreditCards");

            migrationBuilder.AddColumn<int>(
                name: "CreditCardID",
                table: "Orders",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "CreditCards",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreditCardID",
                table: "Orders",
                column: "CreditCardID");

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCards_AspNetUsers_AppUserId",
                table: "CreditCards",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CreditCards_CreditCardID",
                table: "Orders",
                column: "CreditCardID",
                principalTable: "CreditCards",
                principalColumn: "CreditCardID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreditCards_AspNetUsers_AppUserId",
                table: "CreditCards");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CreditCards_CreditCardID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CreditCardID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreditCardID",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "CreditCards",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCards_AspNetUsers_AppUserId",
                table: "CreditCards",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
