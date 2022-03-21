using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data_Layer.Migrations
{
    public partial class UpdateBasketItemTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Adults",
                table: "BasketItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckIn",
                table: "BasketItems",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckOut",
                table: "BasketItems",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Childrens",
                table: "BasketItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Infants",
                table: "BasketItems",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adults",
                table: "BasketItems");

            migrationBuilder.DropColumn(
                name: "CheckIn",
                table: "BasketItems");

            migrationBuilder.DropColumn(
                name: "CheckOut",
                table: "BasketItems");

            migrationBuilder.DropColumn(
                name: "Childrens",
                table: "BasketItems");

            migrationBuilder.DropColumn(
                name: "Infants",
                table: "BasketItems");
        }
    }
}
