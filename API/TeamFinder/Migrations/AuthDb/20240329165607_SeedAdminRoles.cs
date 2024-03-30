using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamFinder.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class SeedAdminRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66a83869-d054-4385-8f6f-2ad64ba78e3c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ea5183ef-d75c-410d-9733-c63d3f7a04e3", "AQAAAAEAACcQAAAAEN+2vBw7U8XL897ehGdDmo78husM688Gl5d2uAMPpvvZf+Poq7dzXZ58cnsgK98NeQ==", "2fcf9161-5538-4653-918a-494b2335917d" });
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
                values: new object[] { "e3969698-6701-4c78-a563-ec8e717c2271", "AQAAAAEAACcQAAAAENvHVkS5KhbFP0hQ7PvSv2LPdUrdeSONYZZWr+zswD+c9wel22TvgeJciiLaDNEdzA==", "167411cf-fd42-40f0-98be-80b55f094ec4" });
        }
    }
}
