using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orpheo.Migrations
{
    /// <inheritdoc />
    public partial class AddDataPublicariiToSong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataPublicarii",
                table: "Songs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataPublicarii",
                table: "Songs");
        }
    }
}
