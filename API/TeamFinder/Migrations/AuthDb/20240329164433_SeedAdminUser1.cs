using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamFinder.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class SeedAdminUser1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66a83869-d054-4385-8f6f-2ad64ba78e3c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "34d989cc-24d1-402c-9340-e649fd4525ab", "AQAAAAEAACcQAAAAEAaPV9DHI8a4p8qNEThfUREKcRTbJdaYncYnayeht9tXM+Lzso+8KkMhnxmZ3I2OvQ==", "e62e4f26-483a-4d4a-88c9-b6b1829c8f6d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66a83869-d054-4385-8f6f-2ad64ba78e3c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3fda100f-c25f-4f21-beff-a9f0890a7834", "AQAAAAEAACcQAAAAEPBarbP9KG8iETN/QaGeDPjJHOcJCyTIWPZaszrdfLXvQvmjEtjXnE0bSICQzgtjdA==", "a7c6100e-24bd-4107-9776-e944b5f795f5" });
        }
    }
}
