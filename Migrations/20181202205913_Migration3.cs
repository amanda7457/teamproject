using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Group14_BevoBooks.Migrations
{
    public partial class Migration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultReorderID",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReorderQuantityDefaultReorderID",
                table: "BookOrders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReorderQuantity",
                columns: table => new
                {
                    DefaultReorderID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DefaultQuantity = table.Column<decimal>(nullable: false),
                    ManagerSetId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReorderQuantity", x => x.DefaultReorderID);
                    table.ForeignKey(
                        name: "FK_ReorderQuantity_AspNetUsers_ManagerSetId",
                        column: x => x.ManagerSetId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DefaultReorderID",
                table: "Orders",
                column: "DefaultReorderID");

            migrationBuilder.CreateIndex(
                name: "IX_BookOrders_ReorderQuantityDefaultReorderID",
                table: "BookOrders",
                column: "ReorderQuantityDefaultReorderID");

            migrationBuilder.CreateIndex(
                name: "IX_ReorderQuantity_ManagerSetId",
                table: "ReorderQuantity",
                column: "ManagerSetId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookOrders_ReorderQuantity_ReorderQuantityDefaultReorderID",
                table: "BookOrders",
                column: "ReorderQuantityDefaultReorderID",
                principalTable: "ReorderQuantity",
                principalColumn: "DefaultReorderID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ReorderQuantity_DefaultReorderID",
                table: "Orders",
                column: "DefaultReorderID",
                principalTable: "ReorderQuantity",
                principalColumn: "DefaultReorderID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookOrders_ReorderQuantity_ReorderQuantityDefaultReorderID",
                table: "BookOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ReorderQuantity_DefaultReorderID",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "ReorderQuantity");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DefaultReorderID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_BookOrders_ReorderQuantityDefaultReorderID",
                table: "BookOrders");

            migrationBuilder.DropColumn(
                name: "DefaultReorderID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReorderQuantityDefaultReorderID",
                table: "BookOrders");
        }
    }
}
