using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddBedIdToTaskDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE public.iot_data DROP CONSTRAINT IF EXISTS \"FK_iot_data_seasons_season_id\";");

            migrationBuilder.AddColumn<Guid>(
                name: "bed_id",
                schema: "public",
                table: "task_detail",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_task_detail_bed_id",
                schema: "public",
                table: "task_detail",
                column: "bed_id");

            migrationBuilder.AddForeignKey(
                name: "fk_iot_data_seasons",
                schema: "public",
                table: "iot_data",
                column: "season_id",
                principalSchema: "public",
                principalTable: "seasons",
                principalColumn: "season_id");

            migrationBuilder.AddForeignKey(
                name: "task_detail_bed_id_fkey",
                schema: "public",
                table: "task_detail",
                column: "bed_id",
                principalSchema: "public",
                principalTable: "beds",
                principalColumn: "bed_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_iot_data_seasons",
                schema: "public",
                table: "iot_data");

            migrationBuilder.DropForeignKey(
                name: "task_detail_bed_id_fkey",
                schema: "public",
                table: "task_detail");

            migrationBuilder.DropIndex(
                name: "IX_task_detail_bed_id",
                schema: "public",
                table: "task_detail");

            migrationBuilder.DropColumn(
                name: "bed_id",
                schema: "public",
                table: "task_detail");

            migrationBuilder.AddForeignKey(
                name: "FK_iot_data_seasons_season_id",
                schema: "public",
                table: "iot_data",
                column: "season_id",
                principalSchema: "public",
                principalTable: "seasons",
                principalColumn: "season_id");
        }
    }
}
