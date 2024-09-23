using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_Webapi.Migrations
{
    /// <inheritdoc />
    public partial class addAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "IsStatus", "Password", "Role", "UserEmail", "UserName" },
                values: new object[] { 9, true, "$2a$11$aomaGkow0/YIG8he6Ar4xuhTJisLtxHqtFrV8KcoLWmo792ljaF12", "Admin", "Admin.com", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "IsStatus", "Password", "Role", "UserEmail", "UserName" },
                values: new object[] { 8, true, "$2a$11$Jup.FpFCZpiuZINXD36DROJKBItaW5tZMb2KWHhfKUkx4Uivp4WIu", "Admin", "Adith1@gmial.com", "Adith" });
        }
    }
}
