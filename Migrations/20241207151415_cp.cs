using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HairSaloonScheduler.Migrations
{
    public partial class cp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "admins",
                keyColumn: "AdminId",
                keyValue: new Guid("d7b9ce2d-b94e-4517-865e-ed5634184dcf"));

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "users",
                newName: "Username");

            migrationBuilder.AlterColumn<string>(
                name: "UserMail",
                table: "users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AddColumn<string>(
                name: "ConfirmPassword",
                table: "users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "admins",
                columns: new[] { "AdminId", "AdminMail", "Password" },
                values: new object[] { new Guid("71cc891d-33ab-450c-adf1-1c05606ddf89"), "g221210034@sakarya.edu.tr", "sau" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "admins",
                keyColumn: "AdminId",
                keyValue: new Guid("71cc891d-33ab-450c-adf1-1c05606ddf89"));

            migrationBuilder.DropColumn(
                name: "ConfirmPassword",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "users",
                newName: "UserName");

            migrationBuilder.AlterColumn<string>(
                name: "UserMail",
                table: "users",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "admins",
                columns: new[] { "AdminId", "AdminMail", "Password" },
                values: new object[] { new Guid("d7b9ce2d-b94e-4517-865e-ed5634184dcf"), "g221210034@sakarya.edu.tr", "sau" });
        }
    }
}
