using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RefactorTaskAndDetailStructure_V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "tasks_assigned_to_worker_id_fkey", schema: "public", table: "tasks");
            migrationBuilder.DropForeignKey(name: "tasks_season_id_fkey", schema: "public", table: "tasks");

            migrationBuilder.Sql("DROP INDEX IF EXISTS public.\"IX_tasks_season_id\";");
            migrationBuilder.Sql("DROP INDEX IF EXISTS public.\"IX_tasks_assigned_to_worker_id\";");

            migrationBuilder.DropColumn(name: "assigned_to_worker_id", schema: "public", table: "tasks");
            migrationBuilder.DropColumn(name: "season_id", schema: "public", table: "tasks");

            migrationBuilder.AddColumn<Guid>(
                name: "assigned_to_worker_id",
                schema: "public",
                table: "task_detail",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_task_detail_assigned_to_worker_id",
                schema: "public",
                table: "task_detail",
                column: "assigned_to_worker_id");

            migrationBuilder.AddForeignKey(
                name: "task_detail_assigned_to_worker_id_fkey",
                schema: "public",
                table: "task_detail",
                column: "assigned_to_worker_id",
                principalSchema: "public",
                principalTable: "users",
                principalColumn: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
