using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupProjectDeployment.Data.Migrations
{
    public partial class cartAddName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userName",
                table: "ShoppingCart",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userName",
                table: "ShoppingCart");
        }
    }
}
