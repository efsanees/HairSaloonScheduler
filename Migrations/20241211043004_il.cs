using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HairSaloonScheduler.Migrations
{
    public partial class il : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_operations_employees_EmployeesEmployeeId",
                table: "operations");

            migrationBuilder.DropIndex(
                name: "IX_operations_EmployeesEmployeeId",
                table: "operations");

            migrationBuilder.DeleteData(
                table: "admins",
                keyColumn: "AdminId",
                keyValue: new Guid("71cc891d-33ab-450c-adf1-1c05606ddf89"));

            migrationBuilder.DropColumn(
                name: "EmployeesEmployeeId",
                table: "operations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EmployeesEmployeeId",
                table: "operations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "admins",
                columns: new[] { "AdminId", "AdminMail", "Password" },
                values: new object[] { new Guid("71cc891d-33ab-450c-adf1-1c05606ddf89"), "g221210034@sakarya.edu.tr", "sau" });

            migrationBuilder.CreateIndex(
                name: "IX_operations_EmployeesEmployeeId",
                table: "operations",
                column: "EmployeesEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_operations_employees_EmployeesEmployeeId",
                table: "operations",
                column: "EmployeesEmployeeId",
                principalTable: "employees",
                principalColumn: "EmployeeId");
        }
    }
}
