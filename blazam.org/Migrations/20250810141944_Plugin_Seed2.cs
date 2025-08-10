using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blazam.org.Migrations
{
    /// <inheritdoc />
    public partial class Plugin_Seed2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plugins_Users_AuthorId",
                table: "Plugins");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Plugins",
                newName: "UploaderId");

            migrationBuilder.RenameIndex(
                name: "IX_Plugins_AuthorId",
                table: "Plugins",
                newName: "IX_Plugins_UploaderId");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Plugins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Plugins_Users_UploaderId",
                table: "Plugins",
                column: "UploaderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plugins_Users_UploaderId",
                table: "Plugins");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "Plugins");

            migrationBuilder.RenameColumn(
                name: "UploaderId",
                table: "Plugins",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Plugins_UploaderId",
                table: "Plugins",
                newName: "IX_Plugins_AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plugins_Users_AuthorId",
                table: "Plugins",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
