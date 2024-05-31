using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamFinder.Migrations
{
    /// <inheritdoc />
    public partial class addwinnerresulttoactivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WinnerResults_ActivityId",
                table: "WinnerResults");

            migrationBuilder.CreateIndex(
                name: "IX_WinnerResults_ActivityId",
                table: "WinnerResults",
                column: "ActivityId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WinnerResults_ActivityId",
                table: "WinnerResults");

            migrationBuilder.CreateIndex(
                name: "IX_WinnerResults_ActivityId",
                table: "WinnerResults",
                column: "ActivityId");
        }
    }
}
