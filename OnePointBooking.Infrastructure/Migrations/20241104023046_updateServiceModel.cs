using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnePointBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateServiceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Rooms_RoomId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "NameOfServices",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Services",
                newName: "RoomServiceId");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Services",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceName",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Services",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Rooms_RoomId",
                table: "Services",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Rooms_RoomId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ServiceName",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "RoomServiceId",
                table: "Services",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Services",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "NameOfServices",
                table: "Services",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Rooms_RoomId",
                table: "Services",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }
    }
}
