﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamFinder.Migrations
{
    /// <inheritdoc />
    public partial class addeddescriptionfieldforteams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Teams");
        }
    }
}
