using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_Webapi.Migrations
{
    /// <inheritdoc />
    public partial class seededadmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                column: "Password",
                value: "$2a$11$o9ZNLEu6Yqmyza86MCnOUOk.ZeNXnhD2fqctB3/CX1nw829j5qJly");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 16,
                column: "Password",
                value: "$2a$11$o9ZNLEu6Yqmyza86MCnOUOk.ZeNXnhD2fqctB3/CX1nw829j5qJly");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "IsStatus", "Password", "Phone", "Role", "UserEmail", "UserName" },
                values: new object[] { 18, true, "$2a$11$o9ZNLEu6Yqmyza86MCnOUO3LdpODZX4iHheIiBAuzh37pVAtZHzue", null, "Admin", "Admin@gmail.com", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                column: "Password",
                value: "$2a$11$nZXdBWXZVdak6sLKLPtaEuYWhK6zn0SEdoCF/m7t6BSg8IN/n9gjK");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 16,
                column: "Password",
                value: "$2a$11$nZXdBWXZVdak6sLKLPtaEuYWhK6zn0SEdoCF/m7t6BSg8IN/n9gjK");
        }
    }
}
