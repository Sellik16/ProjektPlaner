using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektPlaner.Data.Migrations
{
    /// <inheritdoc />
    public partial class GroupsAdd_01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarGroup_AspNetUsers_FounderId",
                table: "CalendarGroup");

            migrationBuilder.AlterColumn<string>(
                name: "FounderId",
                table: "CalendarGroup",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarGroup_AspNetUsers_FounderId",
                table: "CalendarGroup",
                column: "FounderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarGroup_AspNetUsers_FounderId",
                table: "CalendarGroup");

            migrationBuilder.AlterColumn<string>(
                name: "FounderId",
                table: "CalendarGroup",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarGroup_AspNetUsers_FounderId",
                table: "CalendarGroup",
                column: "FounderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
