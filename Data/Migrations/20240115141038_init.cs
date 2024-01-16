using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "User",
                schema: "dbo",
                columns: table => new
                {
                    UserNumber = table.Column<int>(type: "integer", nullable: false),
                    IdentityNumber = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false, defaultValue: "False"),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastActivityDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    InsertUserId = table.Column<int>(type: "integer", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<int>(type: "integer", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    isActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserNumber);
                });

            migrationBuilder.CreateTable(
                name: "Demand",
                schema: "dbo",
                columns: table => new
                {
                    DemandNumber = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DemandId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    isDefault = table.Column<bool>(type: "boolean", nullable: false),
                    DemandType = table.Column<int>(type: "integer", nullable: false),
                    RejectionResponse = table.Column<string>(type: "text", nullable: true),
                    InsertUserId = table.Column<int>(type: "integer", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<int>(type: "integer", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    isActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Demand", x => x.DemandNumber);
                    table.ForeignKey(
                        name: "FK_Demand_User_DemandId",
                        column: x => x.DemandId,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "UserNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Info",
                schema: "dbo",
                columns: table => new
                {
                    InfoNumber = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    IBAN = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Information = table.Column<string>(type: "text", nullable: false),
                    InfoType = table.Column<string>(type: "text", nullable: false),
                    isDefault = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    InsertUserId = table.Column<int>(type: "integer", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<int>(type: "integer", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    isActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Info", x => x.InfoNumber);
                    table.ForeignKey(
                        name: "FK_Info_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "UserNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Demand_DemandId",
                schema: "dbo",
                table: "Demand",
                column: "DemandId");

            migrationBuilder.CreateIndex(
                name: "IX_Demand_DemandNumber",
                schema: "dbo",
                table: "Demand",
                column: "DemandNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_UserNumber",
                schema: "dbo",
                table: "User",
                column: "UserNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_IdentityNumber",
                schema: "dbo",
                table: "User",
                column: "IdentityNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Info_UserId",
                schema: "dbo",
                table: "Info",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Info_InfoNumber",
                schema: "dbo",
                table: "Info",
                column: "InfoNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Info_Information_InfoType",
                schema: "dbo",
                table: "Info",
                columns: new[] { "Information", "InfoType" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Demand",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Info",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "User",
                schema: "dbo");
        }
    }
}
