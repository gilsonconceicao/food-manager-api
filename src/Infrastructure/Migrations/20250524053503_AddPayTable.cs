using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPayTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationDateTo",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "NumberOfInstallments",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ExternalPaymentId",
                table: "Orders",
                newName: "PaymentId");

            migrationBuilder.AddColumn<string>(
                name: "ExternalReference",
                table: "Orders",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Pays",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    PaymentTypeId = table.Column<string>(type: "text", nullable: true),
                    PaymentMethodId = table.Column<string>(type: "text", nullable: true),
                    CurrencyId = table.Column<string>(type: "text", nullable: true),
                    Installments = table.Column<int>(type: "integer", nullable: true),
                    TransactionAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    ExternalReference = table.Column<string>(type: "text", nullable: true),
                    NotificationUrl = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateLastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpirationDateTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    QrCode = table.Column<string>(type: "text", nullable: true),
                    QrCodeBase64 = table.Column<string>(type: "text", nullable: true),
                    CollectorId = table.Column<long>(type: "bigint", nullable: false),
                    IssuerId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pays_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pays_OrderId",
                table: "Pays",
                column: "OrderId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pays");

            migrationBuilder.DropColumn(
                name: "ExternalReference",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "Orders",
                newName: "ExternalPaymentId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDateTo",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfInstallments",
                table: "Orders",
                type: "integer",
                nullable: true);
        }
    }
}
