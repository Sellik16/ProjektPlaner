using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektPlaner.Data.Migrations
{
    /// <inheritdoc />
    public partial class GroupsAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "CalendarGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FounderId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalendarGroup_AspNetUsers_FounderId",
                        column: x => x.FounderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CalendarGroupId",
                table: "AspNetUsers",
                column: "CalendarGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CalendarGroupId1",
                table: "AspNetUsers",
                column: "CalendarGroupId1");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarGroup_FounderId",
                table: "CalendarGroup",
                column: "FounderId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_CalendarGroup_CalendarGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_CalendarGroup_CalendarGroupId1",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CalendarGroup");

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
        }
    }
}
