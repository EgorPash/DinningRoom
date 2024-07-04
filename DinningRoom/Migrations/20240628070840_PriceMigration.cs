using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DinningRoom.Migrations
{
    public partial class PriceMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "StringsOfOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "StringsOfOrders");
        }
    }
}
