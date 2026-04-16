using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddDiagnosisPricingAndPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "diagnosis_price_setting",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    farm_id = table.Column<Guid>(type: "uuid", nullable: false),
                    expert_id = table.Column<Guid>(type: "uuid", nullable: false),
                    month = table.Column<DateTime>(type: "date", nullable: false),
                    price_per_diagnosis = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("diagnosis_price_setting_pkey", x => x.id);
                    table.ForeignKey(
                        name: "diagnosis_price_setting_created_by_fkey",
                        column: x => x.created_by,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "diagnosis_price_setting_expert_fkey",
                        column: x => x.expert_id,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "diagnosis_price_setting_farm_fkey",
                        column: x => x.farm_id,
                        principalSchema: "public",
                        principalTable: "farms",
                        principalColumn: "farm_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "diagnosis_payment",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    price_setting_id = table.Column<Guid>(type: "uuid", nullable: false),
                    total_diagnoses = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "pending"),
                    payment_provider = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    provider_data = table.Column<string>(type: "jsonb", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    paid_at = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("diagnosis_payment_pkey", x => x.id);
                    table.ForeignKey(
                        name: "diagnosis_payment_price_setting_fkey",
                        column: x => x.price_setting_id,
                        principalSchema: "public",
                        principalTable: "diagnosis_price_setting",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_payment_provider",
                schema: "public",
                table: "diagnosis_payment",
                column: "payment_provider");

            migrationBuilder.CreateIndex(
                name: "idx_payment_status",
                schema: "public",
                table: "diagnosis_payment",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_diagnosis_payment_price_setting_id",
                schema: "public",
                table: "diagnosis_payment",
                column: "price_setting_id");

            migrationBuilder.CreateIndex(
                name: "diagnosis_price_setting_unique",
                schema: "public",
                table: "diagnosis_price_setting",
                columns: new[] { "farm_id", "expert_id", "month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_price_setting_farm_month",
                schema: "public",
                table: "diagnosis_price_setting",
                columns: new[] { "farm_id", "month" });

            migrationBuilder.CreateIndex(
                name: "IX_diagnosis_price_setting_created_by",
                schema: "public",
                table: "diagnosis_price_setting",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_diagnosis_price_setting_expert_id",
                schema: "public",
                table: "diagnosis_price_setting",
                column: "expert_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "diagnosis_payment",
                schema: "public");

            migrationBuilder.DropTable(
                name: "diagnosis_price_setting",
                schema: "public");
        }
    }
}
