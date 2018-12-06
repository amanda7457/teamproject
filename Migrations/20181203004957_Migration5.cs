using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Group14_BevoBooks.Migrations
{
    public partial class Migration5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookOrderDetails");

            migrationBuilder.AddColumn<bool>(
                name: "InReorderList",
                table: "BookOrders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "BookOrders",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "BookOrders",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InReorderList",
                table: "BookOrders");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "BookOrders");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "BookOrders");

            migrationBuilder.CreateTable(
                name: "BookOrderDetails",
                columns: table => new
                {
                    BookOrderDetailID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BookID = table.Column<int>(nullable: true),
                    OrderBookOrderID = table.Column<int>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
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
                name: "IX_BookOrderDetails_BookID",
                table: "BookOrderDetails",
                column: "BookID");

            migrationBuilder.CreateIndex(
                name: "IX_BookOrderDetails_OrderBookOrderID",
                table: "BookOrderDetails",
                column: "OrderBookOrderID");
        }
    }
}
