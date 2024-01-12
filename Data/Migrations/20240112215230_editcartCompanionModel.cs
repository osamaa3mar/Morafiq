using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _Morafiq.Data.Migrations
{
    public partial class editcartCompanionModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanionQuantity",
                table: "CartCompanions");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "CartCompanions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "CartCompanions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "CartCompanions");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "CartCompanions");

            migrationBuilder.AddColumn<int>(
                name: "CompanionQuantity",
                table: "CartCompanions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
