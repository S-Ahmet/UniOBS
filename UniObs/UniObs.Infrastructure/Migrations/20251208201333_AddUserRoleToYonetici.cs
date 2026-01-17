using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniObs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRoleToYonetici : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "role",
                table: "yoneticiler",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "role",
                table: "yoneticiler");
        }
    }
}
