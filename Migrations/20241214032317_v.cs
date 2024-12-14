using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HairSaloonScheduler.Migrations
{
    public partial class v : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_availabilities_EmployeeId",
                table: "availabilities",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_availabilities_employees_EmployeeId",
                table: "availabilities",
                column: "EmployeeId",
                principalTable: "employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_availabilities_employees_EmployeeId",
                table: "availabilities");

            migrationBuilder.DropIndex(
                name: "IX_availabilities_EmployeeId",
                table: "availabilities");
        }
    }
}
