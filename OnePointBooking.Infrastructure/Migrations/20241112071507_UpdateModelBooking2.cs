using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnePointBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelBooking2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Bookings",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Bookings");
        }
    }
}
