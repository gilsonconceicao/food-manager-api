using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class renameentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_FoodOrders_FoodOrderId",
                table: "Client");

            migrationBuilder.DropForeignKey(
                name: "FK_Foods_FoodOrders_FoodOrderId",
                table: "Foods");

            migrationBuilder.DropTable(
                name: "FoodOrders");

            migrationBuilder.RenameColumn(
                name: "FoodOrderId",
                table: "Foods",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Foods_FoodOrderId",
                table: "Foods",
                newName: "IX_Foods_OrderId");

            migrationBuilder.RenameColumn(
                name: "FoodOrderId",
                table: "Client",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Client_FoodOrderId",
                table: "Client",
                newName: "IX_Client_OrderId");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestNumber = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Client_Orders_OrderId",
                table: "Client",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Foods_Orders_OrderId",
                table: "Foods",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_Orders_OrderId",
                table: "Client");

            migrationBuilder.DropForeignKey(
                name: "FK_Foods_Orders_OrderId",
                table: "Foods");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Foods",
                newName: "FoodOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Foods_OrderId",
                table: "Foods",
                newName: "IX_Foods_FoodOrderId");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Client",
                newName: "FoodOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Client_OrderId",
                table: "Client",
                newName: "IX_Client_FoodOrderId");

            migrationBuilder.CreateTable(
                name: "FoodOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RequestNumber = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodOrders", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Client_FoodOrders_FoodOrderId",
                table: "Client",
                column: "FoodOrderId",
                principalTable: "FoodOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Foods_FoodOrders_FoodOrderId",
                table: "Foods",
                column: "FoodOrderId",
                principalTable: "FoodOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
