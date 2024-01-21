using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RoleUser",
                columns: new[] { "RoleId", "UserNumber" },
                values: new object[,]
                {
                    { 1, 112233 },
                    { 2, 445566 }
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserNumber",
                keyValue: 112233,
                column: "DateOfBirth",
                value: new DateTime(2009, 1, 21, 12, 11, 7, 136, DateTimeKind.Utc).AddTicks(1582));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserNumber",
                keyValue: 445566,
                column: "DateOfBirth",
                value: new DateTime(2004, 1, 21, 12, 11, 7, 136, DateTimeKind.Utc).AddTicks(1571));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoleUser",
                keyColumns: new[] { "RoleId", "UserNumber" },
                keyValues: new object[] { 1, 112233 });

            migrationBuilder.DeleteData(
                table: "RoleUser",
                keyColumns: new[] { "RoleId", "UserNumber" },
                keyValues: new object[] { 2, 445566 });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserNumber",
                keyValue: 112233,
                column: "DateOfBirth",
                value: new DateTime(2009, 1, 21, 12, 6, 36, 918, DateTimeKind.Utc).AddTicks(3493));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserNumber",
                keyValue: 445566,
                column: "DateOfBirth",
                value: new DateTime(2004, 1, 21, 12, 6, 36, 918, DateTimeKind.Utc).AddTicks(3480));
        }
    }
}
