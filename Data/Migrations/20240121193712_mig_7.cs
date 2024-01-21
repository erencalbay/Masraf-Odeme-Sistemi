using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class mig_7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Receipt",
                table: "Demand",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Demand",
                keyColumn: "DemandId",
                keyValue: 1,
                column: "Receipt",
                value: "Receipts/Mavi.jpg");

            migrationBuilder.UpdateData(
                table: "Demand",
                keyColumn: "DemandId",
                keyValue: 2,
                column: "Receipt",
                value: "Receipts/Kırmızı.jpg");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserNumber",
                keyValue: 112233,
                column: "DateOfBirth",
                value: new DateTime(2009, 1, 21, 19, 37, 12, 131, DateTimeKind.Utc).AddTicks(6195));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserNumber",
                keyValue: 445566,
                column: "DateOfBirth",
                value: new DateTime(2004, 1, 21, 19, 37, 12, 131, DateTimeKind.Utc).AddTicks(6180));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Receipt",
                table: "Demand");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserNumber",
                keyValue: 112233,
                column: "DateOfBirth",
                value: new DateTime(2009, 1, 21, 14, 36, 55, 940, DateTimeKind.Utc).AddTicks(7011));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserNumber",
                keyValue: 445566,
                column: "DateOfBirth",
                value: new DateTime(2004, 1, 21, 14, 36, 55, 940, DateTimeKind.Utc).AddTicks(6992));
        }
    }
}
