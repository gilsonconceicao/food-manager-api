using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class refactorrelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Foods_Orders_OrderId",
                table: "Foods");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Client_ClientId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Foods_OrderId",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Foods");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClientId",
                table: "Orders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "OrdersFoodsRelationships",
                columns: table => new
                {
                    FoodId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    FoodId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    OrderId1 = table.Column<Guid>(type: "uuid", nullable: true)
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
                        name: "FK_OrdersFoodsRelationships_Foods_FoodId1",
                        column: x => x.FoodId1,
                        principalTable: "Foods",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrdersFoodsRelationships_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdersFoodsRelationships_Orders_OrderId1",
                        column: x => x.OrderId1,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrdersFoodsRelationships_FoodId1",
                table: "OrdersFoodsRelationships",
                column: "FoodId1");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersFoodsRelationships_OrderId",
                table: "OrdersFoodsRelationships",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersFoodsRelationships_OrderId1",
                table: "OrdersFoodsRelationships",
                column: "OrderId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Client_ClientId",
                table: "Orders",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Client_ClientId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "OrdersFoodsRelationships");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClientId",
                table: "Orders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "Foods",
                type: "uuid",
                nullable: true);

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
                name: "FK_Orders_Client_ClientId",
                table: "Orders",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id");
        }
    }
}
