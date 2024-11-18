using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class relationshipsentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrdersFoodsRelationships");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Orders",
                newName: "UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "Foods",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Foods_OrderId",
                table: "Foods",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Foods_Orders_OrderId",
                table: "Foods",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_User_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Foods_Orders_OrderId",
                table: "Foods");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_User_UserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Foods_OrderId",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Foods");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Orders",
                newName: "ClientId");

            migrationBuilder.CreateTable(
                name: "OrdersFoodsRelationships",
                columns: table => new
                {
                    FoodId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersFoodsRelationships", x => new { x.FoodId, x.OrderId });
                    table.ForeignKey(
                        name: "FK_OrdersFoodsRelationships_Foods_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdersFoodsRelationships_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrdersFoodsRelationships_OrderId",
                table: "OrdersFoodsRelationships",
                column: "OrderId");
        }
    }
}
