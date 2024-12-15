using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektPlaner.Data.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalendarGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FounderId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalendarGroup_AspNetUsers_FounderId",
                        column: x => x.FounderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CalendarElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalendarElement_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalendarElement_CalendarGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "CalendarGroup",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CalendarGroupAdministrator",
                columns: table => new
                {
                    AdminId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CalendarGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarGroupAdministrator", x => new { x.AdminId, x.CalendarGroupId });
                    table.ForeignKey(
                        name: "FK_CalendarGroupAdministrator_AspNetUsers_AdminId",
                        column: x => x.AdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalendarGroupAdministrator_CalendarGroup_CalendarGroupId",
                        column: x => x.CalendarGroupId,
                        principalTable: "CalendarGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalendarGroupUser",
                columns: table => new
                {
                    CalendarGroupId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarGroupUser", x => new { x.CalendarGroupId, x.UserId });
                    table.ForeignKey(
                        name: "FK_CalendarGroupUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalendarGroupUser_CalendarGroup_CalendarGroupId",
                        column: x => x.CalendarGroupId,
                        principalTable: "CalendarGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalendarElement_GroupId",
                table: "CalendarElement",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarElement_UserId",
                table: "CalendarElement",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarGroup_FounderId",
                table: "CalendarGroup",
                column: "FounderId");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarGroupAdministrator_CalendarGroupId",
                table: "CalendarGroupAdministrator",
                column: "CalendarGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarGroupUser_UserId",
                table: "CalendarGroupUser",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalendarElement");

            migrationBuilder.DropTable(
                name: "CalendarGroupAdministrator");

            migrationBuilder.DropTable(
                name: "CalendarGroupUser");

            migrationBuilder.DropTable(
                name: "CalendarGroup");
        }
    }
}
