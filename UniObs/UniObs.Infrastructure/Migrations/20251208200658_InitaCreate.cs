using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniObs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitaCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "yoneticiler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    sifre = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FailedAccessCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    LockoutEnd = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_yoneticiler", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "yoneticiler");
        }
    }
}
