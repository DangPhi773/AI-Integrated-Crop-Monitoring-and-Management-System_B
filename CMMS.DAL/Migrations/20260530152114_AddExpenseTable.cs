using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddExpenseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "expense",
                schema: "public",
                columns: table => new
                {
                    expense_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    season_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    amount = table.Column<decimal>(type: "numeric(14,2)", nullable: false),
                    spent_at = table.Column<DateOnly>(type: "date", nullable: false),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("expense_pkey", x => x.expense_id);
                    table.ForeignKey(
                        name: "expense_created_by_fkey",
                        column: x => x.created_by,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "expense_season_fkey",
                        column: x => x.season_id,
                        principalSchema: "public",
                        principalTable: "seasons",
                        principalColumn: "season_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_expense_season",
                schema: "public",
                table: "expense",
                column: "season_id");

            migrationBuilder.CreateIndex(
                name: "idx_expense_season_category",
                schema: "public",
                table: "expense",
                columns: new[] { "season_id", "category" });

            migrationBuilder.CreateIndex(
                name: "IX_expense_created_by",
                schema: "public",
                table: "expense",
                column: "created_by");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "expense",
                schema: "public");
        }
    }
}
