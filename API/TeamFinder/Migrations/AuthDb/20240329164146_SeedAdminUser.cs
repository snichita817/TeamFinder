using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamFinder.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66a83869-d054-4385-8f6f-2ad64ba78e3c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3fda100f-c25f-4f21-beff-a9f0890a7834", "AQAAAAEAACcQAAAAEPBarbP9KG8iETN/QaGeDPjJHOcJCyTIWPZaszrdfLXvQvmjEtjXnE0bSICQzgtjdA==", "a7c6100e-24bd-4107-9776-e944b5f795f5" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66a83869-d054-4385-8f6f-2ad64ba78e3c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d30ba144-91c0-41e6-8874-d16361b8d471", "AQAAAAEAACcQAAAAEBS5IwP2CGimK1YWnSdgDPFPyVOqfDdREynBhFwxU3FiF3RL5ou1TKKo3WKQDjs0+w==", "813173cd-3458-4485-8e75-8ff067b616b2" });
        }
    }
}
