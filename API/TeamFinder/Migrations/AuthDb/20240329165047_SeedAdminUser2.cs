using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamFinder.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class SeedAdminUser2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66a83869-d054-4385-8f6f-2ad64ba78e3c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e3969698-6701-4c78-a563-ec8e717c2271", "AQAAAAEAACcQAAAAENvHVkS5KhbFP0hQ7PvSv2LPdUrdeSONYZZWr+zswD+c9wel22TvgeJciiLaDNEdzA==", "167411cf-fd42-40f0-98be-80b55f094ec4" });
            
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "66a83869-d054-4385-8f6f-2ad64ba78e3c", 0, "d30ba144-91c0-41e6-8874-d16361b8d471", "admin@teamfinder.com", false, false, null, "ADMIN@TEAMFINDER.COM", "ADMIN@TEAMFINDER.COM", "AQAAAAEAACcQAAAAEBS5IwP2CGimK1YWnSdgDPFPyVOqfDdREynBhFwxU3FiF3RL5ou1TKKo3WKQDjs0+w==", null, false, "813173cd-3458-4485-8e75-8ff067b616b2", false, "admin@teamfinder.com" });
            
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66a83869-d054-4385-8f6f-2ad64ba78e3c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "34d989cc-24d1-402c-9340-e649fd4525ab", "AQAAAAEAACcQAAAAEAaPV9DHI8a4p8qNEThfUREKcRTbJdaYncYnayeht9tXM+Lzso+8KkMhnxmZ3I2OvQ==", "e62e4f26-483a-4d4a-88c9-b6b1829c8f6d" });
        }
    }
}
