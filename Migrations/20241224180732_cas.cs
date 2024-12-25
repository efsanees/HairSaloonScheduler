using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HairSaloonScheduler.Migrations
{
    public partial class cas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employeeAbilities_employees_EmployeeId",
                table: "employeeAbilities");

            migrationBuilder.DropForeignKey(
                name: "FK_employeeAbilities_operations_OperationId",
                table: "employeeAbilities");

            migrationBuilder.AddForeignKey(
                name: "FK_employeeAbilities_employees_EmployeeId",
                table: "employeeAbilities",
                column: "EmployeeId",
                principalTable: "employees",
                principalColumn: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_employeeAbilities_operations_OperationId",
                table: "employeeAbilities",
                column: "OperationId",
                principalTable: "operations",
                principalColumn: "OperationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employeeAbilities_employees_EmployeeId",
                table: "employeeAbilities");

            migrationBuilder.DropForeignKey(
                name: "FK_employeeAbilities_operations_OperationId",
                table: "employeeAbilities");

            migrationBuilder.AddForeignKey(
                name: "FK_employeeAbilities_employees_EmployeeId",
                table: "employeeAbilities",
                column: "EmployeeId",
                principalTable: "employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_employeeAbilities_operations_OperationId",
                table: "employeeAbilities",
                column: "OperationId",
                principalTable: "operations",
                principalColumn: "OperationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
