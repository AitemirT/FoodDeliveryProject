using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class AddedPropertyToOrderDishes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Dishes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_OrderId",
                table: "Dishes",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_Orders_OrderId",
                table: "Dishes",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Orders_OrderId",
                table: "Dishes");

            migrationBuilder.DropIndex(
                name: "IX_Dishes_OrderId",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Dishes");
        }
    }
}
