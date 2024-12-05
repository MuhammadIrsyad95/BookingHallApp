using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnePointBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCompanytodb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Companies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Services_RoomId",
                table: "Services",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_UserId",
                table: "Companies",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_AspNetUsers_UserId",
                table: "Companies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Rooms_RoomId",
                table: "Services",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_AspNetUsers_UserId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Rooms_RoomId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_RoomId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Companies_UserId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Companies");
        }
    }
}
