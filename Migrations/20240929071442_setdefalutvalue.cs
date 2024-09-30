using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_Webapi.Migrations
{
    /// <inheritdoc />
    public partial class setdefalutvalue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "status",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                column: "Password",
                value: "$2a$11$On840eqa2Y7x9P3JrYyIV.XUVgvmZ8dJX5ASS7j6S282w7rNPPhqu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "status",
                table: "Products",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                column: "Password",
                value: "$2a$11$3VWrFcUH7uF5R0AnpStVCesbqNqywAxv0xaqH.VFN1Ydjyw0Qw2Uu");
        }
    }
}
