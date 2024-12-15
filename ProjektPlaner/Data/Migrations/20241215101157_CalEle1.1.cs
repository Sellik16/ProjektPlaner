using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektPlaner.Data.Migrations
{
    /// <inheritdoc />
    public partial class CalEle11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarElement_CalendarGroup_GroupId",
                table: "CalendarElement");

            migrationBuilder.AddColumn<int>(
                name: "CalendarGroupId",
                table: "CalendarElement",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CalendarElement_CalendarGroupId",
                table: "CalendarElement",
                column: "CalendarGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarElement_CalendarGroup_CalendarGroupId",
                table: "CalendarElement",
                column: "CalendarGroupId",
                principalTable: "CalendarGroup",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarElement_CalendarGroup_GroupId",
                table: "CalendarElement",
                column: "GroupId",
                principalTable: "CalendarGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarElement_CalendarGroup_CalendarGroupId",
                table: "CalendarElement");

            migrationBuilder.DropForeignKey(
                name: "FK_CalendarElement_CalendarGroup_GroupId",
                table: "CalendarElement");

            migrationBuilder.DropIndex(
                name: "IX_CalendarElement_CalendarGroupId",
                table: "CalendarElement");

            migrationBuilder.DropColumn(
                name: "CalendarGroupId",
                table: "CalendarElement");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarElement_CalendarGroup_GroupId",
                table: "CalendarElement",
                column: "GroupId",
                principalTable: "CalendarGroup",
                principalColumn: "Id");
        }
    }
}
