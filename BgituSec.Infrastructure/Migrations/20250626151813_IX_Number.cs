using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BgituSec.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IX_Number : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Number",
                table: "Buildings",
                column: "Number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Number",
                table: "Buildings");
        }
    }
}
