using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamFinder.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class changeidentityusermodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66a83869-d054-4385-8f6f-2ad64ba78e3c");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CourseOfStudy",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GitHubUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GraduationYear",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Interests",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedInUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PortfolioUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Skills",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "University",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Bio", "ConcurrencyStamp", "CourseOfStudy", "Discriminator", "Email", "EmailConfirmed", "FirstName", "GitHubUrl", "GraduationYear", "Interests", "LastName", "LinkedInUrl", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PortfolioUrl", "SecurityStamp", "Skills", "TwoFactorEnabled", "University", "UserName" },
                values: new object[] { "66a83869-d054-4385-8f6f-2ad64ba78e3c", 0, null, "a0e79967-e740-4bb8-b271-b79d0e7ab8f1", null, "ApplicationUser", "admin@teamfinder.com", false, null, null, null, null, null, null, false, null, "ADMIN@TEAMFINDER.COM", "ADMIN@TEAMFINDER.COM", "AQAAAAEAACcQAAAAEDqihy9/+4tobLaFf1Na3yjKEB+u+1lLeUWj9XSgEOMKo8Yx+AvEdspOfCtUqw2JLA==", null, false, null, "3cf38406-8f1e-42b8-9062-445340e75aac", null, false, null, "admin@teamfinder.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CourseOfStudy",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GitHubUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GraduationYear",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Interests",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LinkedInUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PortfolioUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Skills",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "University",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66a83869-d054-4385-8f6f-2ad64ba78e3c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "626ac067-a3f0-46b5-ac00-05a216b129d2", "AQAAAAEAACcQAAAAEPDrbww1cKgyTQSGsT33/NJ9cFlGFLCm+iVtKRIyCGeXjmTZE29RDvKVKUhe/ltJLQ==", "2ac75d02-cf57-49db-87ce-6e32b4bb8d4e" });
        }
    }
}
