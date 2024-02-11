using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamFinder.Migrations
{
    /// <inheritdoc />
    public partial class createrelationshipbetweenActivityUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ActivityId",
                table: "Updates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Updates_ActivityId",
                table: "Updates",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Updates_Activities_ActivityId",
                table: "Updates",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Updates_Activities_ActivityId",
                table: "Updates");

            migrationBuilder.DropIndex(
                name: "IX_Updates_ActivityId",
                table: "Updates");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "Updates");
        }
    }
}
