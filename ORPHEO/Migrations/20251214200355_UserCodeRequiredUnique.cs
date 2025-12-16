using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orpheo.Migrations
{
    /// <inheritdoc />
    public partial class UserCodeRequiredUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserCode",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserCode",
                table: "AspNetUsers",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserCode",
                table: "AspNetUsers",
                column: "UserCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserCode",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserCode",
                table: "AspNetUsers",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserCode",
                table: "AspNetUsers",
                column: "UserCode",
                unique: true,
                filter: "[UserCode] IS NOT NULL");
        }
    }
}
