using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _Morafiq.Data.Migrations
{
    public partial class addingdescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "CompanionSchedule",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "CompanionSchedule");
        }
    }
}
