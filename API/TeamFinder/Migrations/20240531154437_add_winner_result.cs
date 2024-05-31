using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamFinder.Migrations
{
    /// <inheritdoc />
    public partial class addwinnerresult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WinnerResultId",
                table: "Teams",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WinnerResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WinnerResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WinnerResults_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teams_WinnerResultId",
                table: "Teams",
                column: "WinnerResultId");

            migrationBuilder.CreateIndex(
                name: "IX_WinnerResults_ActivityId",
                table: "WinnerResults",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_WinnerResults_WinnerResultId",
                table: "Teams",
                column: "WinnerResultId",
                principalTable: "WinnerResults",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_WinnerResults_WinnerResultId",
                table: "Teams");

            migrationBuilder.DropTable(
                name: "WinnerResults");

            migrationBuilder.DropIndex(
                name: "IX_Teams_WinnerResultId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "WinnerResultId",
                table: "Teams");
        }
    }
}
