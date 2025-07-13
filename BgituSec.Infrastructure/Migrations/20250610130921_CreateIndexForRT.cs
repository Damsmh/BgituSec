using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BgituSec.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateIndexForRT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                newName: "IX_UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_UserId",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_UserId");
        }
    }
}
