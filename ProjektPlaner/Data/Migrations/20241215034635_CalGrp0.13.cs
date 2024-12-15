using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektPlaner.Data.Migrations
{
    /// <inheritdoc />
    public partial class CalGrp013 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_CalendarGroup_CalendarGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_CalendarGroup_CalendarGroupId1",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CalendarGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CalendarGroupId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CalendarGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CalendarGroupId1",
                table: "AspNetUsers");

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
                name: "CalendarGroupAdministrator");

            migrationBuilder.DropTable(
                name: "CalendarGroupUser");

            migrationBuilder.AddColumn<int>(
                name: "CalendarGroupId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CalendarGroupId1",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CalendarGroupId",
                table: "AspNetUsers",
                column: "CalendarGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CalendarGroupId1",
                table: "AspNetUsers",
                column: "CalendarGroupId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_CalendarGroup_CalendarGroupId",
                table: "AspNetUsers",
                column: "CalendarGroupId",
                principalTable: "CalendarGroup",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_CalendarGroup_CalendarGroupId1",
                table: "AspNetUsers",
                column: "CalendarGroupId1",
                principalTable: "CalendarGroup",
                principalColumn: "Id");
        }
    }
}
