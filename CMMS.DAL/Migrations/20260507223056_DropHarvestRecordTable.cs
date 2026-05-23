using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DropHarvestRecordTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "harvest_record",
                schema: "public");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "harvest_record",
                schema: "public",
                columns: table => new
                {
                    harvest_record_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    harvest_id = table.Column<Guid>(type: "uuid", nullable: false),
                    buyer_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "now()"),
                    harvest_date = table.Column<DateOnly>(type: "date", nullable: false),
                    notes = table.Column<string>(type: "text", nullable: true),
                    quantity = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    sale_channel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    sale_date = table.Column<DateOnly>(type: "date", nullable: true),
                    sold_quantity = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    total_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    unit_price = table.Column<decimal>(type: "numeric(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("harvest_record_pkey", x => x.harvest_record_id);
                    table.ForeignKey(
                        name: "harvest_record_harvest_id_fkey",
                        column: x => x.harvest_id,
                        principalSchema: "public",
                        principalTable: "harvest",
                        principalColumn: "harvest_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_harvest_record_harvest_date",
                schema: "public",
                table: "harvest_record",
                column: "harvest_date");

            migrationBuilder.CreateIndex(
                name: "idx_harvest_record_sale_date",
                schema: "public",
                table: "harvest_record",
                column: "sale_date");

            migrationBuilder.CreateIndex(
                name: "IX_harvest_record_harvest_id",
                schema: "public",
                table: "harvest_record",
                column: "harvest_id");
        }
    }
}
