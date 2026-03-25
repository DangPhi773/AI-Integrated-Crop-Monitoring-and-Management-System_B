using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddArrayColumnsToTaskDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Safely drop old constraints/indexes/columns (may already be gone)
            migrationBuilder.Sql("ALTER TABLE public.crops DROP CONSTRAINT IF EXISTS \"FK_crops_soil_soil_id\";");
            migrationBuilder.Sql("ALTER TABLE public.task_detail DROP CONSTRAINT IF EXISTS \"task_detail_assigned_to_worker_id_fkey\";");
            migrationBuilder.Sql("ALTER TABLE public.task_detail DROP CONSTRAINT IF EXISTS \"task_detail_bed_id_fkey\";");
            migrationBuilder.Sql("DROP INDEX IF EXISTS public.\"IX_task_detail_assigned_to_worker_id\";");
            migrationBuilder.Sql("DROP INDEX IF EXISTS public.\"IX_task_detail_bed_id\";");
            migrationBuilder.Sql("DROP INDEX IF EXISTS public.\"IX_crops_soil_id\";");
            migrationBuilder.Sql("ALTER TABLE public.task_detail DROP COLUMN IF EXISTS assigned_to_worker_id;");
            migrationBuilder.Sql("ALTER TABLE public.task_detail DROP COLUMN IF EXISTS bed_id;");
            migrationBuilder.Sql("ALTER TABLE public.crops DROP COLUMN IF EXISTS soil_id;");

            // Add new array columns
            migrationBuilder.Sql("ALTER TABLE public.task_detail ADD COLUMN IF NOT EXISTS assigned_to_worker_ids uuid[] NOT NULL DEFAULT '{}';");
            migrationBuilder.Sql("ALTER TABLE public.task_detail ADD COLUMN IF NOT EXISTS bed_ids uuid[] NOT NULL DEFAULT '{}';");
            migrationBuilder.Sql("ALTER TABLE public.task_detail ADD COLUMN IF NOT EXISTS plot_ids uuid[] NOT NULL DEFAULT '{}';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE public.task_detail DROP COLUMN IF EXISTS assigned_to_worker_ids;");
            migrationBuilder.Sql("ALTER TABLE public.task_detail DROP COLUMN IF EXISTS bed_ids;");
            migrationBuilder.Sql("ALTER TABLE public.task_detail DROP COLUMN IF EXISTS plot_ids;");
        }
    }
}
