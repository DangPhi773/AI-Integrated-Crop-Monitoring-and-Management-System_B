using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RenameStageThresholdsToMax : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "temperature_min",
                schema: "public",
                table: "crop_growth_stages",
                newName: "temperature_max");

            migrationBuilder.RenameColumn(
                name: "humidity_min",
                schema: "public",
                table: "crop_growth_stages",
                newName: "humidity_max");

            migrationBuilder.RenameColumn(
                name: "soil_moisture_min",
                schema: "public",
                table: "crop_growth_stages",
                newName: "soil_moisture_max");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "temperature_max",
                schema: "public",
                table: "crop_growth_stages",
                newName: "temperature_min");

            migrationBuilder.RenameColumn(
                name: "humidity_max",
                schema: "public",
                table: "crop_growth_stages",
                newName: "humidity_min");

            migrationBuilder.RenameColumn(
                name: "soil_moisture_max",
                schema: "public",
                table: "crop_growth_stages",
                newName: "soil_moisture_min");
        }
    }
}
