using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddBedAutoSplitFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "plot_margin",
                schema: "public",
                table: "plots",
                type: "double precision",
                nullable: false,
                defaultValue: 0.29999999999999999);

            migrationBuilder.AddColumn<double>(
                name: "bed_width_default",
                schema: "public",
                table: "crops",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "path_width_default",
                schema: "public",
                table: "crops",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "row_spacing",
                schema: "public",
                table: "crops",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "rows_per_bed",
                schema: "public",
                table: "crops",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "crop_id",
                schema: "public",
                table: "beds",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "path_width",
                schema: "public",
                table: "beds",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "plant_count",
                schema: "public",
                table: "beds",
                type: "integer",
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE public.crops c
                SET
                    bed_width_default  = cbc.bed_width_min,
                    path_width_default = cbc.path_width_min,
                    row_spacing        = cbc.row_spacing,
                    rows_per_bed       = cbc.rows_per_bed
                FROM public.crop_bed_config cbc
                WHERE cbc.crop_id = c.crop_id AND cbc.is_default = true;

                UPDATE public.crops c
                SET plant_spacing = cbc.plant_spacing
                FROM public.crop_bed_config cbc
                WHERE cbc.crop_id = c.crop_id AND cbc.is_default = true
                  AND c.plant_spacing IS NULL;
            ");

            migrationBuilder.DropTable(
                name: "crop_bed_config",
                schema: "public");

            migrationBuilder.CreateIndex(
                name: "IX_beds_crop_id",
                schema: "public",
                table: "beds",
                column: "crop_id");

            migrationBuilder.AddForeignKey(
                name: "beds_crop_id_fkey",
                schema: "public",
                table: "beds",
                column: "crop_id",
                principalSchema: "public",
                principalTable: "crops",
                principalColumn: "crop_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "beds_crop_id_fkey",
                schema: "public",
                table: "beds");

            migrationBuilder.DropIndex(
                name: "IX_beds_crop_id",
                schema: "public",
                table: "beds");

            migrationBuilder.DropColumn(
                name: "plot_margin",
                schema: "public",
                table: "plots");

            migrationBuilder.DropColumn(
                name: "bed_width_default",
                schema: "public",
                table: "crops");

            migrationBuilder.DropColumn(
                name: "path_width_default",
                schema: "public",
                table: "crops");

            migrationBuilder.DropColumn(
                name: "row_spacing",
                schema: "public",
                table: "crops");

            migrationBuilder.DropColumn(
                name: "rows_per_bed",
                schema: "public",
                table: "crops");

            migrationBuilder.DropColumn(
                name: "crop_id",
                schema: "public",
                table: "beds");

            migrationBuilder.DropColumn(
                name: "path_width",
                schema: "public",
                table: "beds");

            migrationBuilder.DropColumn(
                name: "plant_count",
                schema: "public",
                table: "beds");

            migrationBuilder.CreateTable(
                name: "crop_bed_config",
                schema: "public",
                columns: table => new
                {
                    config_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    crop_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bed_height = table.Column<double>(type: "double precision", nullable: true),
                    bed_width_max = table.Column<double>(type: "double precision", nullable: true),
                    bed_width_min = table.Column<double>(type: "double precision", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "now()"),
                    density_per_ha_max = table.Column<int>(type: "integer", nullable: true),
                    density_per_ha_min = table.Column<int>(type: "integer", nullable: true),
                    is_default = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    notes = table.Column<string>(type: "text", nullable: true),
                    path_width_max = table.Column<double>(type: "double precision", nullable: true),
                    path_width_min = table.Column<double>(type: "double precision", nullable: true),
                    plant_spacing = table.Column<double>(type: "double precision", nullable: false),
                    planting_pattern = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    row_spacing = table.Column<double>(type: "double precision", nullable: false),
                    rows_per_bed = table.Column<int>(type: "integer", nullable: false),
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
    }
}
