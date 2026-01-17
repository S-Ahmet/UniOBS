using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniObs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTwoFactorToYonetici : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "two_factor_code",
                table: "yoneticiler",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "two_factor_expires_at",
                table: "yoneticiler",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "two_factor_code",
                table: "yoneticiler");

            migrationBuilder.DropColumn(
                name: "two_factor_expires_at",
                table: "yoneticiler");
        }
    }
}
