using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupProjectDeployment.Data.Migrations
{
    public partial class changedShoppingCartModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ShoppingCart_ShoppingCartCartId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCart_UserId",
                table: "ShoppingCart");

            migrationBuilder.DropIndex(
                name: "IX_Products_ShoppingCartCartId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ShoppingCartCartId",
                table: "Products");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "ShoppingCart",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCart_ProductId",
                table: "ShoppingCart",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCart_UserId",
                table: "ShoppingCart",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCart_Products_ProductId",
                table: "ShoppingCart",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCart_Products_ProductId",
                table: "ShoppingCart");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCart_ProductId",
                table: "ShoppingCart");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCart_UserId",
                table: "ShoppingCart");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ShoppingCart");

            migrationBuilder.AddColumn<Guid>(
                name: "ShoppingCartCartId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCart_UserId",
                table: "ShoppingCart",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ShoppingCartCartId",
                table: "Products",
                column: "ShoppingCartCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ShoppingCart_ShoppingCartCartId",
                table: "Products",
                column: "ShoppingCartCartId",
                principalTable: "ShoppingCart",
                principalColumn: "CartId");
        }
    }
}
