using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddIotDevicesAndData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "iot_devices",
                schema: "public",
                columns: table => new
                {
                    device_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bed_id = table.Column<Guid>(type: "uuid", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true),
                    installation_date = table.Column<DateTime>(type: "date", nullable: true),
                    latitude = table.Column<double>(type: "double precision", nullable: true),
                    longitude = table.Column<double>(type: "double precision", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iot_devices", x => x.device_id);
                    table.ForeignKey(
                        name: "fk_iot_devices_beds",
                        column: x => x.bed_id,
                        principalSchema: "public",
                        principalTable: "beds",
                        principalColumn: "bed_id");
                });

            migrationBuilder.CreateTable(
                name: "iot_data",
                schema: "public",
                columns: table => new
                {
                    sensor_data_id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_id = table.Column<Guid>(type: "uuid", nullable: true),
                    season_id = table.Column<Guid>(type: "uuid", nullable: true),
                    recorded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    type = table.Column<string>(type: "text", nullable: true),
                    value = table.Column<double>(type: "double precision", nullable: true),
                    unit = table.Column<string>(type: "text", nullable: true),
                    is_alert = table.Column<bool>(type: "boolean", nullable: true),
                    min = table.Column<double>(type: "double precision", nullable: true),
                    max = table.Column<double>(type: "double precision", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iot_data", x => x.sensor_data_id);
                    table.ForeignKey(
                        name: "fk_iot_data_iot_devices",
                        column: x => x.device_id,
                        principalSchema: "public",
                        principalTable: "iot_devices",
                        principalColumn: "device_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_iot_data_device_id",
                schema: "public",
                table: "iot_data",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_iot_devices_bed_id",
                schema: "public",
                table: "iot_devices",
                column: "bed_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "iot_data",
                schema: "public");

            migrationBuilder.DropTable(
                name: "iot_devices",
                schema: "public");
        }
    }
}
