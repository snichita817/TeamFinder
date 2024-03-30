using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamFinder.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class addOrganizerRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5a0f7a7e-c62f-49db-bc0f-9b0061917e77", "5a0f7a7e-c62f-49db-bc0f-9b0061917e77", "Organizer", "ORGANIZER" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66a83869-d054-4385-8f6f-2ad64ba78e3c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "626ac067-a3f0-46b5-ac00-05a216b129d2", "AQAAAAEAACcQAAAAEPDrbww1cKgyTQSGsT33/NJ9cFlGFLCm+iVtKRIyCGeXjmTZE29RDvKVKUhe/ltJLQ==", "2ac75d02-cf57-49db-87ce-6e32b4bb8d4e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5a0f7a7e-c62f-49db-bc0f-9b0061917e77");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66a83869-d054-4385-8f6f-2ad64ba78e3c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ea5183ef-d75c-410d-9733-c63d3f7a04e3", "AQAAAAEAACcQAAAAEN+2vBw7U8XL897ehGdDmo78husM688Gl5d2uAMPpvvZf+Poq7dzXZ58cnsgK98NeQ==", "2fcf9161-5538-4653-918a-494b2335917d" });
        }
    }
}
