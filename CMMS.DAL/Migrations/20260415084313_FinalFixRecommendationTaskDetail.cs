using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMMS.DAL.Migrations
{
    public partial class FinalFixRecommendationTaskDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "season_id",
                schema: "public",
                table: "recommendation_task_detail",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "farm_id",
                schema: "public",
                table: "recommendation_task_detail",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "assigned_to_worker_ids",
                schema: "public",
                table: "recommendation_task_detail",
                type: "jsonb",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "bed_ids",
                schema: "public",
                table: "recommendation_task_detail",
                type: "jsonb",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "plot_ids",
                schema: "public",
                table: "recommendation_task_detail",
                type: "jsonb",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AlterColumn<string>(
                name: "phone_number",
                schema: "public",
                table: "users",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "password",
                schema: "public",
                table: "users",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "fullname",
                schema: "public",
                table: "users",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RecommendationTaskDetailTaskDetailId",
                schema: "public",
                table: "worker_schedule",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_recommendation_task_detail_season_id",
                schema: "public",
                table: "recommendation_task_detail",
                column: "season_id");

            migrationBuilder.CreateIndex(
                name: "IX_recommendation_task_detail_farm_id",
                schema: "public",
                table: "recommendation_task_detail",
                column: "farm_id");

            migrationBuilder.CreateIndex(
                name: "IX_worker_schedule_RecommendationTaskDetailTaskDetailId",
                schema: "public",
                table: "worker_schedule",
                column: "RecommendationTaskDetailTaskDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_recommendation_task_detail_farms_farm_id",
                schema: "public",
                table: "recommendation_task_detail",
                column: "farm_id",
                principalSchema: "public",
                principalTable: "farms",
                principalColumn: "farm_id");

            migrationBuilder.AddForeignKey(
                name: "FK_recommendation_task_detail_seasons_season_id",
                schema: "public",
                table: "recommendation_task_detail",
                column: "season_id",
                principalSchema: "public",
                principalTable: "seasons",
                principalColumn: "season_id");

            migrationBuilder.AddForeignKey(
                name: "FK_worker_schedule_recommendation_task_detail_RecommendationTaskDetailTaskDetailId",
                schema: "public",
                table: "worker_schedule",
                column: "RecommendationTaskDetailTaskDetailId",
                principalSchema: "public",
                principalTable: "recommendation_task_detail",
                principalColumn: "task_detail_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_recommendation_task_detail_farms_farm_id", schema: "public", table: "recommendation_task_detail");
            migrationBuilder.DropForeignKey(name: "FK_recommendation_task_detail_seasons_season_id", schema: "public", table: "recommendation_task_detail");
            migrationBuilder.DropColumn(name: "season_id", schema: "public", table: "recommendation_task_detail");
            migrationBuilder.DropColumn(name: "farm_id", schema: "public", table: "recommendation_task_detail");
            migrationBuilder.DropColumn(name: "assigned_to_worker_ids", schema: "public", table: "recommendation_task_detail");
            migrationBuilder.DropColumn(name: "bed_ids", schema: "public", table: "recommendation_task_detail");
            migrationBuilder.DropColumn(name: "plot_ids", schema: "public", table: "recommendation_task_detail");
        }
    }
}