using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceGatewayWithContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM public.diagnosis_payment;");

            migrationBuilder.DropForeignKey(
                name: "diagnosis_payment_price_setting_fkey",
                schema: "public",
                table: "diagnosis_payment");

            migrationBuilder.DropTable(
                name: "diagnosis_price_setting",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "idx_payment_provider",
                schema: "public",
                table: "diagnosis_payment");

            migrationBuilder.DropIndex(
                name: "IX_diagnosis_payment_price_setting_id",
                schema: "public",
                table: "diagnosis_payment");

            migrationBuilder.DropColumn(
                name: "PayOSOrderCode",
                schema: "public",
                table: "diagnosis_payment");

            migrationBuilder.DropColumn(
                name: "payment_provider",
                schema: "public",
                table: "diagnosis_payment");

            migrationBuilder.DropColumn(
                name: "provider_data",
                schema: "public",
                table: "diagnosis_payment");

            migrationBuilder.RenameColumn(
                name: "price_setting_id",
                schema: "public",
                table: "diagnosis_payment",
                newName: "contract_id");

            migrationBuilder.AddColumn<string>(
                name: "bill_image_url",
                schema: "public",
                table: "diagnosis_payment",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "bill_public_id",
                schema: "public",
                table: "diagnosis_payment",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "month",
                schema: "public",
                table: "diagnosis_payment",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "diagnosis_contract",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    contract_code = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    farm_id = table.Column<Guid>(type: "uuid", nullable: false),
                    expert_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bank_account = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    bank_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    account_holder = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    price_per_diagnosis = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "active"),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("diagnosis_contract_pkey", x => x.id);
                    table.ForeignKey(
                        name: "diagnosis_contract_created_by_fkey",
                        column: x => x.created_by,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "diagnosis_contract_expert_fkey",
                        column: x => x.expert_id,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "diagnosis_contract_farm_fkey",
                        column: x => x.farm_id,
                        principalSchema: "public",
                        principalTable: "farms",
                        principalColumn: "farm_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "diagnosis_payment_contract_month_unique",
                schema: "public",
                table: "diagnosis_payment",
                columns: new[] { "contract_id", "month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "diagnosis_contract_code_unique",
                schema: "public",
                table: "diagnosis_contract",
                column: "contract_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_contract_farm_expert",
                schema: "public",
                table: "diagnosis_contract",
                columns: new[] { "farm_id", "expert_id" });

            migrationBuilder.CreateIndex(
                name: "IX_diagnosis_contract_created_by",
                schema: "public",
                table: "diagnosis_contract",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_diagnosis_contract_expert_id",
                schema: "public",
                table: "diagnosis_contract",
                column: "expert_id");

            migrationBuilder.AddForeignKey(
                name: "diagnosis_payment_contract_fkey",
                schema: "public",
                table: "diagnosis_payment",
                column: "contract_id",
                principalSchema: "public",
                principalTable: "diagnosis_contract",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "diagnosis_payment_contract_fkey",
                schema: "public",
                table: "diagnosis_payment");

            migrationBuilder.DropTable(
                name: "diagnosis_contract",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "diagnosis_payment_contract_month_unique",
                schema: "public",
                table: "diagnosis_payment");

            migrationBuilder.DropColumn(
                name: "bill_image_url",
                schema: "public",
                table: "diagnosis_payment");

            migrationBuilder.DropColumn(
                name: "bill_public_id",
                schema: "public",
                table: "diagnosis_payment");

            migrationBuilder.DropColumn(
                name: "month",
                schema: "public",
                table: "diagnosis_payment");

            migrationBuilder.RenameColumn(
                name: "contract_id",
                schema: "public",
                table: "diagnosis_payment",
                newName: "price_setting_id");

            migrationBuilder.AddColumn<long>(
                name: "PayOSOrderCode",
                schema: "public",
                table: "diagnosis_payment",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "payment_provider",
                schema: "public",
                table: "diagnosis_payment",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "provider_data",
                schema: "public",
                table: "diagnosis_payment",
                type: "jsonb",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "diagnosis_price_setting",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    expert_id = table.Column<Guid>(type: "uuid", nullable: false),
                    farm_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    month = table.Column<DateTime>(type: "date", nullable: false),
                    notes = table.Column<string>(type: "text", nullable: true),
                    price_per_diagnosis = table.Column<decimal>(type: "numeric(12,2)", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "idx_payment_provider",
                schema: "public",
                table: "diagnosis_payment",
                column: "payment_provider");

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

            migrationBuilder.AddForeignKey(
                name: "diagnosis_payment_price_setting_fkey",
                schema: "public",
                table: "diagnosis_payment",
                column: "price_setting_id",
                principalSchema: "public",
                principalTable: "diagnosis_price_setting",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
