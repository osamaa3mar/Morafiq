using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _Morafiq.Data.Migrations
{
    public partial class editSecdualModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ScheduleDate",
                table: "CompanionSchedule",
                newName: "StartDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "CompanionSchedule",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "CompanionSchedule");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "CompanionSchedule",
                newName: "ScheduleDate");
        }
    }
}
