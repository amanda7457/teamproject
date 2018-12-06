using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Group14_BevoBooks.Migrations
{
    public partial class Migration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "BookOrders");

            migrationBuilder.AlterColumn<string>(
                name: "PromoCode",
                table: "Discounts",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "BookOrders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BookOrderDetails",
                columns: table => new
                {
                    BookOrderDetailID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    OrderBookOrderID = table.Column<int>(nullable: true),
                    BookID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookOrderDetails", x => x.BookOrderDetailID);
                    table.ForeignKey(
                        name: "FK_BookOrderDetails_Books_BookID",
                        column: x => x.BookID,
                        principalTable: "Books",
                        principalColumn: "BookID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookOrderDetails_BookOrders_OrderBookOrderID",
                        column: x => x.OrderBookOrderID,
                        principalTable: "BookOrders",
                        principalColumn: "BookOrderID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookOrders_AppUserId",
                table: "BookOrders",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BookOrderDetails_BookID",
                table: "BookOrderDetails",
                column: "BookID");

            migrationBuilder.CreateIndex(
                name: "IX_BookOrderDetails_OrderBookOrderID",
                table: "BookOrderDetails",
                column: "OrderBookOrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_BookOrders_AspNetUsers_AppUserId",
                table: "BookOrders",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookOrders_AspNetUsers_AppUserId",
                table: "BookOrders");

            migrationBuilder.DropTable(
                name: "BookOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_BookOrders_AppUserId",
                table: "BookOrders");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "BookOrders");

            migrationBuilder.AlterColumn<string>(
                name: "PromoCode",
                table: "Discounts",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "BookOrders",
                nullable: false,
                defaultValue: 0);
        }
    }
}
