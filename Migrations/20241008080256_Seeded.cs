using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_Webapi.Migrations
{
    /// <inheritdoc />
    public partial class Seeded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                column: "Password",
                value: "$2a$11$nZXdBWXZVdak6sLKLPtaEuYWhK6zn0SEdoCF/m7t6BSg8IN/n9gjK");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "IsStatus", "Password", "Phone", "Role", "UserEmail", "UserName" },
                values: new object[] { 16, true, "$2a$11$nZXdBWXZVdak6sLKLPtaEuYWhK6zn0SEdoCF/m7t6BSg8IN/n9gjK", null, "Admin", "Admin@.gmail.com", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                column: "Password",
                value: "$2a$11$zZAbUttQDVRrbeqWP7S33.HDZbiyiz8WojyK6riHOmT86tllpzQ82");
        }
    }
}
