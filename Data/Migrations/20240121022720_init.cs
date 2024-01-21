using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserNumber = table.Column<int>(type: "integer", nullable: false),
                    IdentityNumber = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false, defaultValue: "False"),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastActivityDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    InsertUserNumber = table.Column<int>(type: "integer", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserNumber = table.Column<int>(type: "integer", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    isActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserNumber);
                });

            migrationBuilder.CreateTable(
                name: "Demand",
                columns: table => new
                {
                    DemandId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserNumber = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    DemandNumber = table.Column<int>(type: "integer", nullable: false),
                    DemandResponseType = table.Column<int>(type: "integer", nullable: false),
                    RejectionResponse = table.Column<string>(type: "text", nullable: true),
                    InsertUserNumber = table.Column<int>(type: "integer", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserNumber = table.Column<int>(type: "integer", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    isActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Demand", x => x.DemandId);
                    table.ForeignKey(
                        name: "FK_Demand_User_UserNumber",
                        column: x => x.UserNumber,
                        principalTable: "User",
                        principalColumn: "UserNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Info",
                columns: table => new
                {
                    InfoId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserNumber = table.Column<int>(type: "integer", nullable: false),
                    IBAN = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Information = table.Column<string>(type: "text", nullable: false),
                    InfoNumber = table.Column<int>(type: "integer", nullable: false),
                    InfoType = table.Column<string>(type: "text", nullable: false),
                    isDefault = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    InsertUserNumber = table.Column<int>(type: "integer", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserNumber = table.Column<int>(type: "integer", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    isActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Info", x => x.InfoId);
                    table.ForeignKey(
                        name: "FK_Info_User_UserNumber",
                        column: x => x.UserNumber,
                        principalTable: "User",
                        principalColumn: "UserNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleUser",
                columns: table => new
                {
                    RolesId = table.Column<int>(type: "integer", nullable: false),
                    UsersUserNumber = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RolesId, x.UsersUserNumber });
                    table.ForeignKey(
                        name: "FK_RoleUser_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_User_UsersUserNumber",
                        column: x => x.UsersUserNumber,
                        principalTable: "User",
                        principalColumn: "UserNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRefreshTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRefreshTokens", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserRefreshTokens_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "admin" },
                    { 2, "employee" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserNumber", "DateOfBirth", "Email", "FirstName", "IdentityNumber", "InsertDate", "InsertUserNumber", "LastActivityDate", "LastName", "UpdateDate", "UpdateUserNumber" },
                values: new object[,]
                {
                    { 112233, new DateTime(2009, 1, 21, 2, 27, 20, 280, DateTimeKind.Utc).AddTicks(1956), "ahmetkızılkaya@gmail.com", "Ahmet", "34332211002", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kızılkaya", null, null },
                    { 445566, new DateTime(2004, 1, 21, 2, 27, 20, 280, DateTimeKind.Utc).AddTicks(1942), "erencalbay@gmail.com", "Eren", "44332211002", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Çalbay", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Demand_DemandId",
                table: "Demand",
                column: "DemandId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Demand_UserNumber",
                table: "Demand",
                column: "UserNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Info_InfoId",
                table: "Info",
                column: "InfoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Info_Information_InfoType_InfoNumber",
                table: "Info",
                columns: new[] { "Information", "InfoType", "InfoNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Info_UserNumber",
                table: "Info",
                column: "UserNumber");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UsersUserNumber",
                table: "RoleUser",
                column: "UsersUserNumber");

            migrationBuilder.CreateIndex(
                name: "IX_User_IdentityNumber",
                table: "User",
                column: "IdentityNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_UserNumber",
                table: "User",
                column: "UserNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Demand");

            migrationBuilder.DropTable(
                name: "Info");

            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.DropTable(
                name: "UserRefreshTokens");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
