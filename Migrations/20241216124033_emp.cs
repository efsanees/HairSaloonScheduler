using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HairSaloonScheduler.Migrations
{
    public partial class emp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_statistics_EmployeeId",
                table: "statistics",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_statistics_employees_EmployeeId",
                table: "statistics",
                column: "EmployeeId",
                principalTable: "employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_statistics_employees_EmployeeId",
                table: "statistics");

            migrationBuilder.DropIndex(
                name: "IX_statistics_EmployeeId",
                table: "statistics");
        }
    }
}
