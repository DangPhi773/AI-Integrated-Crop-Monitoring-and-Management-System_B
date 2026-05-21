using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RenameDiagnosisPKColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                schema: "public",
                table: "diagnosis_result",
                newName: "diagnosis_result_id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "public",
                table: "diagnosis_payment_item",
                newName: "diagnosis_payment_item_id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "public",
                table: "diagnosis_payment",
                newName: "diagnosis_payment_id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "public",
                table: "diagnosis_contract",
                newName: "diagnosis_contract_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "diagnosis_result_id",
                schema: "public",
                table: "diagnosis_result",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "diagnosis_payment_item_id",
                schema: "public",
                table: "diagnosis_payment_item",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "diagnosis_payment_id",
                schema: "public",
                table: "diagnosis_payment",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "diagnosis_contract_id",
                schema: "public",
                table: "diagnosis_contract",
                newName: "id");
        }
    }
}
