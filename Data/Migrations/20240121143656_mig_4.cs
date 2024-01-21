using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class mig_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Demand",
                keyColumn: "DemandId",
                keyValue: 1,
                column: "UserNumber",
                value: 445566);

            migrationBuilder.UpdateData(
                table: "Demand",
                keyColumn: "DemandId",
                keyValue: 2,
                columns: new[] { "DemandNumber", "Description", "UserNumber" },
                values: new object[] { 222222, "Talep 2", 445566 });

            migrationBuilder.InsertData(
                table: "Info",
                columns: new[] { "InfoId", "IBAN", "InfoNumber", "InfoType", "Information", "InsertDate", "InsertUserNumber", "UpdateDate", "UpdateUserNumber", "UserNumber", "isActive", "isDefault" },
                values: new object[] { 1, "TR760009901234567800100001", 452134, "Ana Hesap", "Aktif Olan Banka Hesabı", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, null, null, 445566, false, true });

            migrationBuilder.InsertData(
                table: "Info",
                columns: new[] { "InfoId", "IBAN", "InfoNumber", "InfoType", "Information", "InsertDate", "InsertUserNumber", "UpdateDate", "UpdateUserNumber", "UserNumber", "isActive" },
                values: new object[] { 2, "TR760009901234567800100002", 652134, "Yan Hesap", "Banka Hesabı 2", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, null, null, 445566, false });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Info",
                keyColumn: "InfoId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Info",
                keyColumn: "InfoId",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Demand",
                keyColumn: "DemandId",
                keyValue: 1,
                column: "UserNumber",
                value: 112233);

            migrationBuilder.UpdateData(
                table: "Demand",
                keyColumn: "DemandId",
                keyValue: 2,
                columns: new[] { "DemandNumber", "Description", "UserNumber" },
                values: new object[] { 111111, "Talep 1", 112233 });

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
    }
}
