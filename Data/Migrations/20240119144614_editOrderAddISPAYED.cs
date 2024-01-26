using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _Morafiq.Data.Migrations
{
    public partial class editOrderAddISPAYED : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPayed",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPayed",
                table: "Orders");
        }
    }
}
