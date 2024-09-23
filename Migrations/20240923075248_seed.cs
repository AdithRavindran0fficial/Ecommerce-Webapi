using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_Webapi.Migrations
{
    /// <inheritdoc />
    public partial class seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "IsStatus", "Password", "Role", "UserEmail", "UserName" },
                values: new object[] { 8, true, "$2a$11$Jup.FpFCZpiuZINXD36DROJKBItaW5tZMb2KWHhfKUkx4Uivp4WIu", "Admin", "Adith1@gmial.com", "Adith" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "IsStatus", "Password", "Role", "UserEmail", "UserName" },
                values: new object[] { 1, true, "$2a$11$6OT2NVsklh.wffPeSPDRWumtwzNmV5NGxg.ITKzR9Bc0N3.q0Okwm", "Admin", "Adith1@gmial.com", "Adith" });
        }
    }
}
