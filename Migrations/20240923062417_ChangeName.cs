using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_Webapi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$6OT2NVsklh.wffPeSPDRWumtwzNmV5NGxg.ITKzR9Bc0N3.q0Okwm");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$9kXlBkf1/RBy0GiTINKCbuL6MwNJ5gQVyxvAdd1G8C8qudsHYW1Uy");
        }
    }
}
