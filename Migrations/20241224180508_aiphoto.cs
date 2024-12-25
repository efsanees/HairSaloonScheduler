using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HairSaloonScheduler.Migrations
{
    public partial class aiphoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hairStyleModels");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "hairStyleModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hairStyleModels", x => x.Id);
                });
        }
    }
}
