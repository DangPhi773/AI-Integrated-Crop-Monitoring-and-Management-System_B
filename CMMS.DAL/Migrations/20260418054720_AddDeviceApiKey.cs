using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddDeviceApiKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "api_key_hash",
                schema: "public",
                table: "iot_devices",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "api_key_rotated_at",
                schema: "public",
                table: "iot_devices",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_iot_devices_api_key_hash",
                schema: "public",
                table: "iot_devices",
                column: "api_key_hash",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_iot_devices_api_key_hash",
                schema: "public",
                table: "iot_devices");

            migrationBuilder.DropColumn(
                name: "api_key_hash",
                schema: "public",
                table: "iot_devices");

            migrationBuilder.DropColumn(
                name: "api_key_rotated_at",
                schema: "public",
                table: "iot_devices");
        }
    }
}
