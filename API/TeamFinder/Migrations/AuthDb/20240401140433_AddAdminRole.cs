using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamFinder.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class AddAdminRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66a83869-d054-4385-8f6f-2ad64ba78e3c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "780b504e-ea31-43fe-aaa0-21780e96077d", "AQAAAAEAACcQAAAAEBUb9JgenCSRWKUxSxo6gI0JTKTWIFh87+CxD668myOo1Ee94Nk7KZUPOnKeecP40w==", "6b4957da-7c5d-467d-a8fc-fab17a605317" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "298f9218-3e3a-4b21-9198-3cf08a40f191", "66a83869-d054-4385-8f6f-2ad64ba78e3c" },
                    { "a84035d4-a82f-4d10-a979-7a40f209256c", "66a83869-d054-4385-8f6f-2ad64ba78e3c" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66a83869-d054-4385-8f6f-2ad64ba78e3c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a0e79967-e740-4bb8-b271-b79d0e7ab8f1", "AQAAAAEAACcQAAAAEDqihy9/+4tobLaFf1Na3yjKEB+u+1lLeUWj9XSgEOMKo8Yx+AvEdspOfCtUqw2JLA==", "3cf38406-8f1e-42b8-9062-445340e75aac" });
        }
    }
}
