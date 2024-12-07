using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HairSaloonScheduler.Migrations
{
    public partial class cr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "admins",
                keyColumn: "AdminId",
                keyValue: new Guid("37986b67-6927-4723-b630-baf1de5dcf33"));

            migrationBuilder.InsertData(
                table: "admins",
                columns: new[] { "AdminId", "AdminMail", "Password" },
                values: new object[] { new Guid("d7b9ce2d-b94e-4517-865e-ed5634184dcf"), "g221210034@sakarya.edu.tr", "sau" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "admins",
                keyColumn: "AdminId",
                keyValue: new Guid("d7b9ce2d-b94e-4517-865e-ed5634184dcf"));

            migrationBuilder.InsertData(
                table: "admins",
                columns: new[] { "AdminId", "AdminMail", "Password" },
                values: new object[] { new Guid("37986b67-6927-4723-b630-baf1de5dcf33"), "g221210034@sakarya.edu.tr", "sau" });
        }
    }
}
