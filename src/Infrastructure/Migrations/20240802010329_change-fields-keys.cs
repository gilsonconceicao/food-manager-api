using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changefieldskeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersFoodsRelationships_Foods_FoodId1",
                table: "OrdersFoodsRelationships");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdersFoodsRelationships_Orders_OrderId1",
                table: "OrdersFoodsRelationships");

            migrationBuilder.DropIndex(
                name: "IX_OrdersFoodsRelationships_FoodId1",
                table: "OrdersFoodsRelationships");

            migrationBuilder.DropIndex(
                name: "IX_OrdersFoodsRelationships_OrderId1",
                table: "OrdersFoodsRelationships");

            migrationBuilder.DropColumn(
                name: "FoodId1",
                table: "OrdersFoodsRelationships");

            migrationBuilder.DropColumn(
                name: "OrderId1",
                table: "OrdersFoodsRelationships");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FoodId1",
                table: "OrdersFoodsRelationships",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId1",
                table: "OrdersFoodsRelationships",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrdersFoodsRelationships_FoodId1",
                table: "OrdersFoodsRelationships",
                column: "FoodId1");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersFoodsRelationships_OrderId1",
                table: "OrdersFoodsRelationships",
                column: "OrderId1");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersFoodsRelationships_Foods_FoodId1",
                table: "OrdersFoodsRelationships",
                column: "FoodId1",
                principalTable: "Foods",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersFoodsRelationships_Orders_OrderId1",
                table: "OrdersFoodsRelationships",
                column: "OrderId1",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
