using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewModelCartDish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Carts_CartId",
                table: "Dishes");

            migrationBuilder.DropIndex(
                name: "IX_Dishes_CartId",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "Dishes");

            migrationBuilder.CreateTable(
                name: "CartDishes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CartId = table.Column<int>(type: "integer", nullable: false),
                    DishId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartDishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartDishes_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartDishes_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartDishes_CartId",
                table: "CartDishes",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartDishes_DishId",
                table: "CartDishes",
                column: "DishId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartDishes");

            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "Dishes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_CartId",
                table: "Dishes",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_Carts_CartId",
                table: "Dishes",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id");
        }
    }
}
