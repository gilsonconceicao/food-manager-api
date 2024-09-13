using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addnumberaddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Address",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Address");
        }
    }
}
