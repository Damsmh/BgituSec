﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BgituSec.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UniqueUsername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Name",
                table: "Users",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Name",
                table: "Users");
        }
    }
}
