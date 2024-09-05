using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class manytomanyrelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_Users_UserId",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Foods_Orders_OrderId",
                table: "Foods");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Foods_OrderId",
                table: "Foods");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Foods",
                newName: "OrderFoodRelatedOrderId");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderFoodRelatedFoodId",
                table: "Orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrderFoodRelatedOrderId",
                table: "Orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrderFoodRelatedFoodId",
                table: "Foods",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderFoodRelateds",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    FoodId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderFoodRelateds", x => new { x.FoodId, x.OrderId });
                    table.ForeignKey(
                        name: "FK_OrderFoodRelateds_Foods_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderFoodRelateds_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderFoodRelatedFoodId_OrderFoodRelatedOrderId",
                table: "Orders",
                columns: new[] { "OrderFoodRelatedFoodId", "OrderFoodRelatedOrderId" });

            migrationBuilder.CreateIndex(
                name: "IX_Foods_OrderFoodRelatedFoodId_OrderFoodRelatedOrderId",
                table: "Foods",
                columns: new[] { "OrderFoodRelatedFoodId", "OrderFoodRelatedOrderId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderFoodRelateds_OrderId",
                table: "OrderFoodRelateds",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Users_UserId",
                table: "Address",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Foods_OrderFoodRelateds_OrderFoodRelatedFoodId_OrderFoodRel~",
                table: "Foods",
                columns: new[] { "OrderFoodRelatedFoodId", "OrderFoodRelatedOrderId" },
                principalTable: "OrderFoodRelateds",
                principalColumns: new[] { "FoodId", "OrderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderFoodRelateds_OrderFoodRelatedFoodId_OrderFoodRe~",
                table: "Orders",
                columns: new[] { "OrderFoodRelatedFoodId", "OrderFoodRelatedOrderId" },
                principalTable: "OrderFoodRelateds",
                principalColumns: new[] { "FoodId", "OrderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_Users_UserId",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Foods_OrderFoodRelateds_OrderFoodRelatedFoodId_OrderFoodRel~",
                table: "Foods");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderFoodRelateds_OrderFoodRelatedFoodId_OrderFoodRe~",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "OrderFoodRelateds");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderFoodRelatedFoodId_OrderFoodRelatedOrderId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Foods_OrderFoodRelatedFoodId_OrderFoodRelatedOrderId",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "OrderFoodRelatedFoodId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderFoodRelatedOrderId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderFoodRelatedFoodId",
                table: "Foods");

            migrationBuilder.RenameColumn(
                name: "OrderFoodRelatedOrderId",
                table: "Foods",
                newName: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Foods_OrderId",
                table: "Foods",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Users_UserId",
                table: "Address",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Foods_Orders_OrderId",
                table: "Foods",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
