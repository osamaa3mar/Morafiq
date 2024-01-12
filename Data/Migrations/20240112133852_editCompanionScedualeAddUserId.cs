using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _Morafiq.Data.Migrations
{
    public partial class editCompanionScedualeAddUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CompanionSchedule",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CompanionSchedule_UserId",
                table: "CompanionSchedule",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanionSchedule_AspNetUsers_UserId",
                table: "CompanionSchedule",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanionSchedule_AspNetUsers_UserId",
                table: "CompanionSchedule");

            migrationBuilder.DropIndex(
                name: "IX_CompanionSchedule_UserId",
                table: "CompanionSchedule");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CompanionSchedule");
        }
    }
}
