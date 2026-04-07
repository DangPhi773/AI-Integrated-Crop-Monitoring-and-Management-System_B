using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCropBedConfigAndAutoAllocate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "plot_length",
                schema: "public",
                table: "plots",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "plot_width",
                schema: "public",
                table: "plots",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "bed_length",
                schema: "public",
                table: "beds",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "bed_width",
                schema: "public",
                table: "beds",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "planting_pattern",
                schema: "public",
                table: "beds",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "row_count",
                schema: "public",
                table: "beds",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "crop_bed_config",
                schema: "public",
                columns: table => new
                {
                    config_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    crop_id = table.Column<Guid>(type: "uuid", nullable: false),
                    planting_pattern = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    row_spacing = table.Column<double>(type: "double precision", nullable: false),
                    plant_spacing = table.Column<double>(type: "double precision", nullable: false),
                    rows_per_bed = table.Column<int>(type: "integer", nullable: false),
                    bed_width_min = table.Column<double>(type: "double precision", nullable: true),
                    bed_width_max = table.Column<double>(type: "double precision", nullable: true),
                    path_width_min = table.Column<double>(type: "double precision", nullable: true),
                    path_width_max = table.Column<double>(type: "double precision", nullable: true),
                    bed_height = table.Column<double>(type: "double precision", nullable: true),
                    density_per_ha_min = table.Column<int>(type: "integer", nullable: true),
                    density_per_ha_max = table.Column<int>(type: "integer", nullable: true),
                    is_default = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("crop_bed_config_pkey", x => x.config_id);
                    table.ForeignKey(
                        name: "crop_bed_config_crop_id_fkey",
                        column: x => x.crop_id,
                        principalSchema: "public",
                        principalTable: "crops",
                        principalColumn: "crop_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ux_crop_bed_config_crop_pattern",
                schema: "public",
                table: "crop_bed_config",
                columns: new[] { "crop_id", "planting_pattern" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_crop_bed_config_default_per_crop",
                schema: "public",
                table: "crop_bed_config",
                column: "crop_id",
                unique: true,
                filter: "is_default = true");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "crop_bed_config",
                schema: "public");

            migrationBuilder.DropColumn(
                name: "plot_length",
                schema: "public",
                table: "plots");

            migrationBuilder.DropColumn(
                name: "plot_width",
                schema: "public",
                table: "plots");

            migrationBuilder.DropColumn(
                name: "bed_length",
                schema: "public",
                table: "beds");

            migrationBuilder.DropColumn(
                name: "bed_width",
                schema: "public",
                table: "beds");

            migrationBuilder.DropColumn(
                name: "planting_pattern",
                schema: "public",
                table: "beds");

            migrationBuilder.DropColumn(
                name: "row_count",
                schema: "public",
                table: "beds");
        }
    }
}
