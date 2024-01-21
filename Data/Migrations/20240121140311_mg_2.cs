using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class mg_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Demand",
                columns: new[] { "DemandId", "DemandNumber", "DemandResponseType", "Description", "InsertDate", "InsertUserNumber", "RejectionResponse", "UpdateDate", "UpdateUserNumber", "UserNumber", "isActive" },
                values: new object[,]
                {
                    { 1, 111111, 0, "Talep 1", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, null, null, null, 112233, true },
                    { 2, 111111, 0, "Talep 1", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, null, null, null, 112233, true }
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserNumber",
                keyValue: 112233,
                column: "DateOfBirth",
                value: new DateTime(2009, 1, 21, 14, 3, 11, 564, DateTimeKind.Utc).AddTicks(5070));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserNumber",
                keyValue: 445566,
                column: "DateOfBirth",
                value: new DateTime(2004, 1, 21, 14, 3, 11, 564, DateTimeKind.Utc).AddTicks(5053));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Demand",
                keyColumn: "DemandId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Demand",
                keyColumn: "DemandId",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserNumber",
                keyValue: 112233,
                column: "DateOfBirth",
                value: new DateTime(2009, 1, 21, 14, 0, 2, 606, DateTimeKind.Utc).AddTicks(8078));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserNumber",
                keyValue: 445566,
                column: "DateOfBirth",
                value: new DateTime(2004, 1, 21, 14, 0, 2, 606, DateTimeKind.Utc).AddTicks(8066));
        }
    }
}
