using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RedesignIotSensorData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "iot_data_sensor_id_fkey",
                schema: "public",
                table: "iot_data");

            migrationBuilder.DropTable(
                name: "iot_sensor",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_iot_data_sensor_id",
                schema: "public",
                table: "iot_data");

            migrationBuilder.DropColumn(
                name: "sensor_id",
                schema: "public",
                table: "iot_data");

            migrationBuilder.DropColumn(
                name: "type",
                schema: "public",
                table: "iot_data");

            migrationBuilder.DropColumn(
                name: "unit",
                schema: "public",
                table: "iot_data");

            migrationBuilder.RenameColumn(
                name: "value",
                schema: "public",
                table: "iot_data",
                newName: "temperature");

            migrationBuilder.RenameColumn(
                name: "min",
                schema: "public",
                table: "iot_data",
                newName: "soil_moisture");

            migrationBuilder.RenameColumn(
                name: "max",
                schema: "public",
                table: "iot_data",
                newName: "light");

            migrationBuilder.AddColumn<string>(
                name: "alert_config_json",
                schema: "public",
                table: "iot_devices",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_alert",
                schema: "public",
                table: "iot_data",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "humidity",
                schema: "public",
                table: "iot_data",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_raining",
                schema: "public",
                table: "iot_data",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "alert_config_json",
                schema: "public",
                table: "iot_devices");

            migrationBuilder.DropColumn(
                name: "humidity",
                schema: "public",
                table: "iot_data");

            migrationBuilder.DropColumn(
                name: "is_raining",
                schema: "public",
                table: "iot_data");

            migrationBuilder.RenameColumn(
                name: "temperature",
                schema: "public",
                table: "iot_data",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "soil_moisture",
                schema: "public",
                table: "iot_data",
                newName: "min");

            migrationBuilder.RenameColumn(
                name: "light",
                schema: "public",
                table: "iot_data",
                newName: "max");

            migrationBuilder.AlterColumn<bool>(
                name: "is_alert",
                schema: "public",
                table: "iot_data",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "sensor_id",
                schema: "public",
                table: "iot_data",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "type",
                schema: "public",
                table: "iot_data",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "unit",
                schema: "public",
                table: "iot_data",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "iot_sensor",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    device_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    max_value = table.Column<double>(type: "double precision", nullable: true),
                    min_value = table.Column<double>(type: "double precision", nullable: true),
                    sensor_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    sensor_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    sensor_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    unit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("iot_sensor_pkey", x => x.id);
                    table.ForeignKey(
                        name: "iot_sensor_device_id_fkey",
                        column: x => x.device_id,
                        principalSchema: "public",
                        principalTable: "iot_devices",
                        principalColumn: "device_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_iot_data_sensor_id",
                schema: "public",
                table: "iot_data",
                column: "sensor_id");

            migrationBuilder.CreateIndex(
                name: "IX_iot_sensor_device_id",
                schema: "public",
                table: "iot_sensor",
                column: "device_id");

            migrationBuilder.AddForeignKey(
                name: "iot_data_sensor_id_fkey",
                schema: "public",
                table: "iot_data",
                column: "sensor_id",
                principalSchema: "public",
                principalTable: "iot_sensor",
                principalColumn: "id");
        }
    }
}
