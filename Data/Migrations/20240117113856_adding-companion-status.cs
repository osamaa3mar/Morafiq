using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _Morafiq.Data.Migrations
{
    public partial class addingcompanionstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanionStatus",
                table: "Companions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanionStatus",
                table: "Companions");
        }
    }
}
