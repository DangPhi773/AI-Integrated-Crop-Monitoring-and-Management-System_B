using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddActualHarvestFieldsToHarvestDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "actual_harvest_date",
                schema: "public",
                table: "harvest_detail",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "actual_quantity",
                schema: "public",
                table: "harvest_detail",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "actual_weight_kg",
                schema: "public",
                table: "harvest_detail",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "harvest_notes",
                schema: "public",
                table: "harvest_detail",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "actual_harvest_date",
                schema: "public",
                table: "harvest_detail");

            migrationBuilder.DropColumn(
                name: "actual_quantity",
                schema: "public",
                table: "harvest_detail");

            migrationBuilder.DropColumn(
                name: "actual_weight_kg",
                schema: "public",
                table: "harvest_detail");

            migrationBuilder.DropColumn(
                name: "harvest_notes",
                schema: "public",
                table: "harvest_detail");
        }
    }
}
