using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceContractFarmAndPaymentKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM public.diagnosis_payment;");

            migrationBuilder.DropForeignKey(
                name: "diagnosis_contract_farm_fkey",
                schema: "public",
                table: "diagnosis_contract");

            migrationBuilder.DropForeignKey(
                name: "diagnosis_payment_contract_fkey",
                schema: "public",
                table: "diagnosis_payment");

            migrationBuilder.DropIndex(
                name: "IX_harvest_detail_harvest_id",
                schema: "public",
                table: "harvest_detail");

            migrationBuilder.DropIndex(
                name: "idx_contract_farm_expert",
                schema: "public",
                table: "diagnosis_contract");

            migrationBuilder.DropIndex(
                name: "IX_diagnosis_contract_expert_id",
                schema: "public",
                table: "diagnosis_contract");

            migrationBuilder.DropColumn(
                name: "farm_id",
                schema: "public",
                table: "diagnosis_contract");

            migrationBuilder.RenameColumn(
                name: "contract_id",
                schema: "public",
                table: "diagnosis_payment",
                newName: "specialist_id");

            migrationBuilder.RenameIndex(
                name: "diagnosis_payment_contract_month_unique",
                schema: "public",
                table: "diagnosis_payment",
                newName: "diagnosis_payment_specialist_month_unique");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "public",
                table: "diagnosis_payment",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "paid",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldDefaultValue: "pending");

            migrationBuilder.CreateTable(
                name: "diagnosis_payment_item",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    payment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    diagnosis_result_id = table.Column<Guid>(type: "uuid", nullable: false),
                    contract_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("diagnosis_payment_item_pkey", x => x.id);
                    table.ForeignKey(
                        name: "diagnosis_payment_item_contract_fkey",
                        column: x => x.contract_id,
                        principalSchema: "public",
                        principalTable: "diagnosis_contract",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "diagnosis_payment_item_payment_fkey",
                        column: x => x.payment_id,
                        principalSchema: "public",
                        principalTable: "diagnosis_payment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "diagnosis_payment_item_result_fkey",
                        column: x => x.diagnosis_result_id,
                        principalSchema: "public",
                        principalTable: "diagnosis_result",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_harvest_detail_harvest_bed_unique",
                schema: "public",
                table: "harvest_detail",
                columns: new[] { "harvest_id", "bed_id" },
                unique: true,
                filter: "bed_id IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "idx_contract_expert_status",
                schema: "public",
                table: "diagnosis_contract",
                columns: new[] { "expert_id", "status" });

            migrationBuilder.CreateIndex(
                name: "diagnosis_payment_item_result_unique",
                schema: "public",
                table: "diagnosis_payment_item",
                column: "diagnosis_result_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_payment_item_payment",
                schema: "public",
                table: "diagnosis_payment_item",
                column: "payment_id");

            migrationBuilder.CreateIndex(
                name: "IX_diagnosis_payment_item_contract_id",
                schema: "public",
                table: "diagnosis_payment_item",
                column: "contract_id");

            migrationBuilder.AddForeignKey(
                name: "diagnosis_payment_specialist_fkey",
                schema: "public",
                table: "diagnosis_payment",
                column: "specialist_id",
                principalSchema: "public",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "diagnosis_payment_specialist_fkey",
                schema: "public",
                table: "diagnosis_payment");

            migrationBuilder.DropTable(
                name: "diagnosis_payment_item",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "ix_harvest_detail_harvest_bed_unique",
                schema: "public",
                table: "harvest_detail");

            migrationBuilder.DropIndex(
                name: "idx_contract_expert_status",
                schema: "public",
                table: "diagnosis_contract");

            migrationBuilder.RenameColumn(
                name: "specialist_id",
                schema: "public",
                table: "diagnosis_payment",
                newName: "contract_id");

            migrationBuilder.RenameIndex(
                name: "diagnosis_payment_specialist_month_unique",
                schema: "public",
                table: "diagnosis_payment",
                newName: "diagnosis_payment_contract_month_unique");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "public",
                table: "diagnosis_payment",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "pending",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldDefaultValue: "paid");

            migrationBuilder.AddColumn<Guid>(
                name: "farm_id",
                schema: "public",
                table: "diagnosis_contract",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_harvest_detail_harvest_id",
                schema: "public",
                table: "harvest_detail",
                column: "harvest_id");

            migrationBuilder.CreateIndex(
                name: "idx_contract_farm_expert",
                schema: "public",
                table: "diagnosis_contract",
                columns: new[] { "farm_id", "expert_id" });

            migrationBuilder.CreateIndex(
                name: "IX_diagnosis_contract_expert_id",
                schema: "public",
                table: "diagnosis_contract",
                column: "expert_id");

            migrationBuilder.AddForeignKey(
                name: "diagnosis_contract_farm_fkey",
                schema: "public",
                table: "diagnosis_contract",
                column: "farm_id",
                principalSchema: "public",
                principalTable: "farms",
                principalColumn: "farm_id",
                onDelete: ReferentialAction.Cascade);

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
    }
}
