using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnePointBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateRoomPackageModelIsKalventisFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsKalventis",
                table: "RoomPackages",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsKalventis",
                table: "RoomPackages");
        }
    }
}
