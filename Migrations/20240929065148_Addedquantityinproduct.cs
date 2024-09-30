using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_Webapi.Migrations
{
    /// <inheritdoc />
    public partial class Addedquantityinproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                column: "Password",
                value: "$2a$11$3VWrFcUH7uF5R0AnpStVCesbqNqywAxv0xaqH.VFN1Ydjyw0Qw2Uu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                column: "Password",
                value: "$2a$11$MYI4ObEi68GDB3EtLD9MmeYZziIi6UpsU1SSFhtmbiKRsTSSRlc/.");
        }
    }
}
