using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HairSaloonScheduler.Migrations
{
    public partial class pk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "employeeAbilities",
                columns: table => new
                {
                    PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OperationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employeeAbilities", x => x.PK);
                    table.ForeignKey(
                        name: "FK_employeeAbilities_employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "employees",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_employeeAbilities_operations_OperationId",
                        column: x => x.OperationId,
                        principalTable: "operations",
                        principalColumn: "OperationId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_employeeAbilities_EmployeeId",
                table: "employeeAbilities",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_employeeAbilities_OperationId",
                table: "employeeAbilities",
                column: "OperationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "employeeAbilities");
        }
    }
}
