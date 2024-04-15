using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamFinder.Migrations
{
    /// <inheritdoc />
    public partial class addCreatedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Activities");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Activities",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ApplicationUserId",
                table: "AspNetUsers",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_CreatedById",
                table: "Activities",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_AspNetUsers_CreatedById",
                table: "Activities",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_ApplicationUserId",
                table: "AspNetUsers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_AspNetUsers_CreatedById",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_ApplicationUserId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ApplicationUserId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_Activities_CreatedById",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Activities");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
