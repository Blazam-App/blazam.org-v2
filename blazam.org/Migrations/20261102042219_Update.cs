using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blazam.org.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Users",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(21)",
                oldMaxLength: 21);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Discriminator", "Email", "IsVerified", "PasswordHash", "Username" },
                values: new object[] { 1, new DateTime(2025, 8, 18, 18, 31, 27, 316, DateTimeKind.Utc).AddTicks(3058), "PluginUser", "support@blazam.org", true, "C9A61E2F844812FC0E3C3F8444E62E06", "Blazam" });
        }
    }
}
