using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SplitPlotMarginIntoLengthAndWidth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "plot_margin",
                schema: "public",
                table: "plots");

            migrationBuilder.AddColumn<double>(
                name: "plot_margin_length",
                schema: "public",
                table: "plots",
                type: "double precision",
                nullable: false,
                defaultValue: 1.0);

            migrationBuilder.AddColumn<double>(
                name: "plot_margin_width",
                schema: "public",
                table: "plots",
                type: "double precision",
                nullable: false,
                defaultValue: 0.3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "plot_margin_length",
                schema: "public",
                table: "plots");

            migrationBuilder.DropColumn(
                name: "plot_margin_width",
                schema: "public",
                table: "plots");

            migrationBuilder.AddColumn<double>(
                name: "plot_margin",
                schema: "public",
                table: "plots",
                type: "double precision",
                nullable: false,
                defaultValue: 0.3);
        }
    }
}
