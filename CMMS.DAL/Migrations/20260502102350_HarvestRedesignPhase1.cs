using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class HarvestRedesignPhase1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "beds_crop_id_fkey",
                schema: "public",
                table: "beds");

            migrationBuilder.DropForeignKey(
                name: "FK_crop_growth_stages_crops_CropId",
                schema: "public",
                table: "crop_growth_stages");

            migrationBuilder.DropForeignKey(
                name: "FK_CropGrowthTasks_CropGrowthStages",
                schema: "public",
                table: "crop_growth_tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_growth_tracking_crop_growth_stages_StageId",
                schema: "public",
                table: "growth_tracking");

            migrationBuilder.DropForeignKey(
                name: "FK_growth_tracking_seasons_detail_SeasonDetailId",
                schema: "public",
                table: "growth_tracking");

            migrationBuilder.DropForeignKey(
                name: "report_created_by_fkey",
                schema: "public",
                table: "report");

            migrationBuilder.DropForeignKey(
                name: "FK_soil_crop_compatibility_crops_CropId",
                schema: "public",
                table: "soil_crop_compatibility");

            migrationBuilder.DropForeignKey(
                name: "FK_soil_crop_compatibility_soil_SoilId",
                schema: "public",
                table: "soil_crop_compatibility");

            migrationBuilder.DropIndex(
                name: "IX_beds_crop_id",
                schema: "public",
                table: "beds");

            migrationBuilder.DropColumn(
                name: "TaskName",
                schema: "public",
                table: "crop_growth_tasks");

            migrationBuilder.DropColumn(
                name: "crop_id",
                schema: "public",
                table: "beds");

            migrationBuilder.DropColumn(
                name: "planting_pattern",
                schema: "public",
                table: "beds");

            migrationBuilder.RenameColumn(
                name: "Note",
                schema: "public",
                table: "soil_crop_compatibility",
                newName: "note");

            migrationBuilder.RenameColumn(
                name: "Compatibility",
                schema: "public",
                table: "soil_crop_compatibility",
                newName: "compatibility");

            migrationBuilder.RenameColumn(
                name: "SoilId",
                schema: "public",
                table: "soil_crop_compatibility",
                newName: "soil_id");

            migrationBuilder.RenameColumn(
                name: "CropId",
                schema: "public",
                table: "soil_crop_compatibility",
                newName: "crop_id");

            migrationBuilder.RenameColumn(
                name: "ComptId",
                schema: "public",
                table: "soil_crop_compatibility",
                newName: "compt_id");

            migrationBuilder.RenameIndex(
                name: "IX_soil_crop_compatibility_SoilId",
                schema: "public",
                table: "soil_crop_compatibility",
                newName: "IX_soil_crop_compatibility_soil_id");

            migrationBuilder.RenameIndex(
                name: "IX_soil_crop_compatibility_CropId",
                schema: "public",
                table: "soil_crop_compatibility",
                newName: "IX_soil_crop_compatibility_crop_id");

            migrationBuilder.RenameColumn(
                name: "created_by",
                schema: "public",
                table: "report",
                newName: "worker_id");

            migrationBuilder.RenameIndex(
                name: "IX_report_created_by",
                schema: "public",
                table: "report",
                newName: "IX_report_worker_id");

            migrationBuilder.RenameColumn(
                name: "bed_created_at",
                schema: "public",
                table: "plots",
                newName: "plot_created_at");

            migrationBuilder.RenameColumn(
                name: "Notes",
                schema: "public",
                table: "growth_tracking",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "public",
                table: "growth_tracking",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TrackingStatus",
                schema: "public",
                table: "growth_tracking",
                newName: "tracking_status");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                schema: "public",
                table: "growth_tracking",
                newName: "start_date");

            migrationBuilder.RenameColumn(
                name: "StageId",
                schema: "public",
                table: "growth_tracking",
                newName: "stage_id");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedBy",
                schema: "public",
                table: "growth_tracking",
                newName: "last_updated_by");

            migrationBuilder.RenameColumn(
                name: "LastObservedAt",
                schema: "public",
                table: "growth_tracking",
                newName: "last_observed_at");

            migrationBuilder.RenameColumn(
                name: "HealthStatus",
                schema: "public",
                table: "growth_tracking",
                newName: "health_status");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                schema: "public",
                table: "growth_tracking",
                newName: "end_date");

            migrationBuilder.RenameColumn(
                name: "DelayReason",
                schema: "public",
                table: "growth_tracking",
                newName: "delay_reason");

            migrationBuilder.RenameColumn(
                name: "DelayDays",
                schema: "public",
                table: "growth_tracking",
                newName: "delay_days");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "public",
                table: "growth_tracking",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ActualYield",
                schema: "public",
                table: "growth_tracking",
                newName: "actual_yield");

            migrationBuilder.RenameColumn(
                name: "ActualHeight",
                schema: "public",
                table: "growth_tracking",
                newName: "actual_height");

            migrationBuilder.RenameColumn(
                name: "SeasonDetailId",
                schema: "public",
                table: "growth_tracking",
                newName: "harvest_detail_id");

            migrationBuilder.RenameIndex(
                name: "IX_growth_tracking_StageId",
                schema: "public",
                table: "growth_tracking",
                newName: "IX_growth_tracking_stage_id");

            migrationBuilder.RenameIndex(
                name: "IX_growth_tracking_SeasonDetailId",
                schema: "public",
                table: "growth_tracking",
                newName: "IX_growth_tracking_harvest_detail_id");

            migrationBuilder.RenameColumn(
                name: "Priority",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "priority");

            migrationBuilder.RenameColumn(
                name: "Notes",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Frequency",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "frequency");

            migrationBuilder.RenameColumn(
                name: "TaskDescription",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "task_description");

            migrationBuilder.RenameColumn(
                name: "StageId",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "stage_id");

            migrationBuilder.RenameColumn(
                name: "RequiredTools",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "required_tools");

            migrationBuilder.RenameColumn(
                name: "RequiredMaterials",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "required_materials");

            migrationBuilder.RenameColumn(
                name: "QuantityUnit",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "quantity_unit");

            migrationBuilder.RenameColumn(
                name: "QuantityPerUnit",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "quantity_per_unit");

            migrationBuilder.RenameColumn(
                name: "IsMandatory",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "is_mandatory");

            migrationBuilder.RenameColumn(
                name: "DurationMinutes",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "duration_minutes");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "GrowthTaskId",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "growth_task_id");

            migrationBuilder.RenameIndex(
                name: "IX_crop_growth_tasks_StageId",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "IX_crop_growth_tasks_stage_id");

            migrationBuilder.RenameColumn(
                name: "Notes",
                schema: "public",
                table: "crop_growth_stages",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "public",
                table: "crop_growth_stages",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TemperatureMin",
                schema: "public",
                table: "crop_growth_stages",
                newName: "temperature_min");

            migrationBuilder.RenameColumn(
                name: "StageName",
                schema: "public",
                table: "crop_growth_stages",
                newName: "stage_name");

            migrationBuilder.RenameColumn(
                name: "StageDescription",
                schema: "public",
                table: "crop_growth_stages",
                newName: "stage_description");

            migrationBuilder.RenameColumn(
                name: "SoilMoistureMin",
                schema: "public",
                table: "crop_growth_stages",
                newName: "soil_moisture_min");

            migrationBuilder.RenameColumn(
                name: "HumidityMin",
                schema: "public",
                table: "crop_growth_stages",
                newName: "humidity_min");

            migrationBuilder.RenameColumn(
                name: "GrowthIndicators",
                schema: "public",
                table: "crop_growth_stages",
                newName: "growth_indicators");

            migrationBuilder.RenameColumn(
                name: "CropId",
                schema: "public",
                table: "crop_growth_stages",
                newName: "crop_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "public",
                table: "crop_growth_stages",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CommonDiseases",
                schema: "public",
                table: "crop_growth_stages",
                newName: "common_diseases");

            migrationBuilder.RenameIndex(
                name: "IX_crop_growth_stages_CropId",
                schema: "public",
                table: "crop_growth_stages",
                newName: "IX_crop_growth_stages_crop_id");

            migrationBuilder.AddColumn<Guid>(
                name: "task_id",
                schema: "public",
                table: "crop_growth_tasks",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "harvest",
                schema: "public",
                columns: table => new
                {
                    harvest_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    plot_id = table.Column<Guid>(type: "uuid", nullable: false),
                    season_id = table.Column<Guid>(type: "uuid", nullable: false),
                    crop_id = table.Column<Guid>(type: "uuid", nullable: false),
                    expected_date = table.Column<DateOnly>(type: "date", nullable: true),
                    expected_quantity = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    unit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "planned"),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("harvest_pkey", x => x.harvest_id);
                    table.ForeignKey(
                        name: "harvest_crop_id_fkey",
                        column: x => x.crop_id,
                        principalSchema: "public",
                        principalTable: "crops",
                        principalColumn: "crop_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "harvest_plot_id_fkey",
                        column: x => x.plot_id,
                        principalSchema: "public",
                        principalTable: "plots",
                        principalColumn: "plot_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "harvest_season_id_fkey",
                        column: x => x.season_id,
                        principalSchema: "public",
                        principalTable: "seasons",
                        principalColumn: "season_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "harvest_detail",
                schema: "public",
                columns: table => new
                {
                    harvest_detail_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    harvest_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bed_id = table.Column<Guid>(type: "uuid", nullable: true),
                    crop_quantity = table.Column<int>(type: "integer", nullable: true),
                    start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("harvest_detail_pkey", x => x.harvest_detail_id);
                    table.ForeignKey(
                        name: "harvest_detail_bed_id_fkey",
                        column: x => x.bed_id,
                        principalSchema: "public",
                        principalTable: "beds",
                        principalColumn: "bed_id");
                    table.ForeignKey(
                        name: "harvest_detail_harvest_id_fkey",
                        column: x => x.harvest_id,
                        principalSchema: "public",
                        principalTable: "harvest",
                        principalColumn: "harvest_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "harvest_record",
                schema: "public",
                columns: table => new
                {
                    harvest_record_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    harvest_id = table.Column<Guid>(type: "uuid", nullable: false),
                    harvest_date = table.Column<DateOnly>(type: "date", nullable: false),
                    quantity = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    sale_date = table.Column<DateOnly>(type: "date", nullable: true),
                    sold_quantity = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    unit_price = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    total_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    buyer_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    sale_channel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "now()")
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

            migrationBuilder.Sql(@"
                INSERT INTO public.harvest (
                    harvest_id, plot_id, season_id, crop_id,
                    expected_date, expected_quantity, status, created_at
                )
                SELECT
                    gen_random_uuid(),
                    b.plot_id,
                    sd.season_id,
                    sd.crop_id,
                    MIN(sd.season_expected_harvest_date),
                    NULLIF(SUM(COALESCE(sd.crop_quantity, 0)), 0),
                    CASE
                        WHEN MAX(sd.total_harvest_yield) IS NOT NULL THEN 'completed'
                        WHEN MAX(sd.end_date) IS NOT NULL AND MAX(sd.end_date) < CURRENT_DATE THEN 'completed'
                        ELSE 'planned'
                    END,
                    now()
                FROM public.seasons_detail sd
                JOIN public.beds b ON b.bed_id = sd.bed_id
                WHERE sd.season_id IS NOT NULL
                  AND sd.crop_id IS NOT NULL
                  AND b.plot_id IS NOT NULL
                GROUP BY b.plot_id, sd.season_id, sd.crop_id;

                INSERT INTO public.harvest_detail (
                    harvest_detail_id, harvest_id, bed_id, crop_quantity, start_date, end_date
                )
                SELECT
                    sd.season_detail_id,
                    h.harvest_id,
                    sd.bed_id,
                    sd.crop_quantity,
                    sd.start_date,
                    sd.end_date
                FROM public.seasons_detail sd
                JOIN public.beds b ON b.bed_id = sd.bed_id
                JOIN public.harvest h
                    ON h.season_id = sd.season_id
                   AND h.crop_id   = sd.crop_id
                   AND h.plot_id   = b.plot_id
                WHERE sd.season_id IS NOT NULL
                  AND sd.crop_id   IS NOT NULL
                  AND b.plot_id    IS NOT NULL;

                INSERT INTO public.harvest_record (
                    harvest_record_id, harvest_id, harvest_date, quantity, notes, created_at
                )
                SELECT
                    gen_random_uuid(),
                    h.harvest_id,
                    COALESCE(sd.end_date, sd.start_date, CURRENT_DATE),
                    sd.total_harvest_yield,
                    'Migrated from seasons_detail.total_harvest_yield',
                    now()
                FROM public.seasons_detail sd
                JOIN public.beds b ON b.bed_id = sd.bed_id
                JOIN public.harvest h
                    ON h.season_id = sd.season_id
                   AND h.crop_id   = sd.crop_id
                   AND h.plot_id   = b.plot_id
                WHERE sd.season_id IS NOT NULL
                  AND sd.crop_id   IS NOT NULL
                  AND b.plot_id    IS NOT NULL
                  AND sd.total_harvest_yield IS NOT NULL
                  AND sd.total_harvest_yield > 0;
            ");

            migrationBuilder.DropTable(
                name: "seasons_detail",
                schema: "public");

            migrationBuilder.CreateIndex(
                name: "IX_crop_growth_tasks_task_id",
                schema: "public",
                table: "crop_growth_tasks",
                column: "task_id");

            migrationBuilder.CreateIndex(
                name: "IX_harvest_crop_id",
                schema: "public",
                table: "harvest",
                column: "crop_id");

            migrationBuilder.CreateIndex(
                name: "IX_harvest_plot_id",
                schema: "public",
                table: "harvest",
                column: "plot_id");

            migrationBuilder.CreateIndex(
                name: "IX_harvest_season_id",
                schema: "public",
                table: "harvest",
                column: "season_id");

            migrationBuilder.CreateIndex(
                name: "IX_harvest_detail_bed_id",
                schema: "public",
                table: "harvest_detail",
                column: "bed_id");

            migrationBuilder.CreateIndex(
                name: "IX_harvest_detail_harvest_id",
                schema: "public",
                table: "harvest_detail",
                column: "harvest_id");

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

            migrationBuilder.AddForeignKey(
                name: "crop_growth_stages_crop_id_fkey",
                schema: "public",
                table: "crop_growth_stages",
                column: "crop_id",
                principalSchema: "public",
                principalTable: "crops",
                principalColumn: "crop_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "crop_growth_tasks_stage_id_fkey",
                schema: "public",
                table: "crop_growth_tasks",
                column: "stage_id",
                principalSchema: "public",
                principalTable: "crop_growth_stages",
                principalColumn: "stage_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "crop_growth_tasks_task_id_fkey",
                schema: "public",
                table: "crop_growth_tasks",
                column: "task_id",
                principalSchema: "public",
                principalTable: "tasks",
                principalColumn: "task_id");

            migrationBuilder.AddForeignKey(
                name: "growth_tracking_harvest_detail_id_fkey",
                schema: "public",
                table: "growth_tracking",
                column: "harvest_detail_id",
                principalSchema: "public",
                principalTable: "harvest_detail",
                principalColumn: "harvest_detail_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "growth_tracking_stage_id_fkey",
                schema: "public",
                table: "growth_tracking",
                column: "stage_id",
                principalSchema: "public",
                principalTable: "crop_growth_stages",
                principalColumn: "stage_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "report_worker_id_fkey",
                schema: "public",
                table: "report",
                column: "worker_id",
                principalSchema: "public",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "soil_crop_compatibility_crop_id_fkey",
                schema: "public",
                table: "soil_crop_compatibility",
                column: "crop_id",
                principalSchema: "public",
                principalTable: "crops",
                principalColumn: "crop_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "soil_crop_compatibility_soil_id_fkey",
                schema: "public",
                table: "soil_crop_compatibility",
                column: "soil_id",
                principalSchema: "public",
                principalTable: "soil",
                principalColumn: "soil_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "crop_growth_stages_crop_id_fkey",
                schema: "public",
                table: "crop_growth_stages");

            migrationBuilder.DropForeignKey(
                name: "crop_growth_tasks_stage_id_fkey",
                schema: "public",
                table: "crop_growth_tasks");

            migrationBuilder.DropForeignKey(
                name: "crop_growth_tasks_task_id_fkey",
                schema: "public",
                table: "crop_growth_tasks");

            migrationBuilder.DropForeignKey(
                name: "growth_tracking_harvest_detail_id_fkey",
                schema: "public",
                table: "growth_tracking");

            migrationBuilder.DropForeignKey(
                name: "growth_tracking_stage_id_fkey",
                schema: "public",
                table: "growth_tracking");

            migrationBuilder.DropForeignKey(
                name: "report_worker_id_fkey",
                schema: "public",
                table: "report");

            migrationBuilder.DropForeignKey(
                name: "soil_crop_compatibility_crop_id_fkey",
                schema: "public",
                table: "soil_crop_compatibility");

            migrationBuilder.DropForeignKey(
                name: "soil_crop_compatibility_soil_id_fkey",
                schema: "public",
                table: "soil_crop_compatibility");

            migrationBuilder.DropTable(
                name: "harvest_detail",
                schema: "public");

            migrationBuilder.DropTable(
                name: "harvest_record",
                schema: "public");

            migrationBuilder.DropTable(
                name: "harvest",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_crop_growth_tasks_task_id",
                schema: "public",
                table: "crop_growth_tasks");

            migrationBuilder.DropColumn(
                name: "task_id",
                schema: "public",
                table: "crop_growth_tasks");

            migrationBuilder.RenameColumn(
                name: "note",
                schema: "public",
                table: "soil_crop_compatibility",
                newName: "Note");

            migrationBuilder.RenameColumn(
                name: "compatibility",
                schema: "public",
                table: "soil_crop_compatibility",
                newName: "Compatibility");

            migrationBuilder.RenameColumn(
                name: "soil_id",
                schema: "public",
                table: "soil_crop_compatibility",
                newName: "SoilId");

            migrationBuilder.RenameColumn(
                name: "crop_id",
                schema: "public",
                table: "soil_crop_compatibility",
                newName: "CropId");

            migrationBuilder.RenameColumn(
                name: "compt_id",
                schema: "public",
                table: "soil_crop_compatibility",
                newName: "ComptId");

            migrationBuilder.RenameIndex(
                name: "IX_soil_crop_compatibility_soil_id",
                schema: "public",
                table: "soil_crop_compatibility",
                newName: "IX_soil_crop_compatibility_SoilId");

            migrationBuilder.RenameIndex(
                name: "IX_soil_crop_compatibility_crop_id",
                schema: "public",
                table: "soil_crop_compatibility",
                newName: "IX_soil_crop_compatibility_CropId");

            migrationBuilder.RenameColumn(
                name: "worker_id",
                schema: "public",
                table: "report",
                newName: "created_by");

            migrationBuilder.RenameIndex(
                name: "IX_report_worker_id",
                schema: "public",
                table: "report",
                newName: "IX_report_created_by");

            migrationBuilder.RenameColumn(
                name: "plot_created_at",
                schema: "public",
                table: "plots",
                newName: "bed_created_at");

            migrationBuilder.RenameColumn(
                name: "notes",
                schema: "public",
                table: "growth_tracking",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                schema: "public",
                table: "growth_tracking",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "tracking_status",
                schema: "public",
                table: "growth_tracking",
                newName: "TrackingStatus");

            migrationBuilder.RenameColumn(
                name: "start_date",
                schema: "public",
                table: "growth_tracking",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "stage_id",
                schema: "public",
                table: "growth_tracking",
                newName: "StageId");

            migrationBuilder.RenameColumn(
                name: "last_updated_by",
                schema: "public",
                table: "growth_tracking",
                newName: "LastUpdatedBy");

            migrationBuilder.RenameColumn(
                name: "last_observed_at",
                schema: "public",
                table: "growth_tracking",
                newName: "LastObservedAt");

            migrationBuilder.RenameColumn(
                name: "health_status",
                schema: "public",
                table: "growth_tracking",
                newName: "HealthStatus");

            migrationBuilder.RenameColumn(
                name: "end_date",
                schema: "public",
                table: "growth_tracking",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "delay_reason",
                schema: "public",
                table: "growth_tracking",
                newName: "DelayReason");

            migrationBuilder.RenameColumn(
                name: "delay_days",
                schema: "public",
                table: "growth_tracking",
                newName: "DelayDays");

            migrationBuilder.RenameColumn(
                name: "created_at",
                schema: "public",
                table: "growth_tracking",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "actual_yield",
                schema: "public",
                table: "growth_tracking",
                newName: "ActualYield");

            migrationBuilder.RenameColumn(
                name: "actual_height",
                schema: "public",
                table: "growth_tracking",
                newName: "ActualHeight");

            migrationBuilder.RenameColumn(
                name: "harvest_detail_id",
                schema: "public",
                table: "growth_tracking",
                newName: "SeasonDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_growth_tracking_stage_id",
                schema: "public",
                table: "growth_tracking",
                newName: "IX_growth_tracking_StageId");

            migrationBuilder.RenameIndex(
                name: "IX_growth_tracking_harvest_detail_id",
                schema: "public",
                table: "growth_tracking",
                newName: "IX_growth_tracking_SeasonDetailId");

            migrationBuilder.RenameColumn(
                name: "priority",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "Priority");

            migrationBuilder.RenameColumn(
                name: "notes",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "frequency",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "Frequency");

            migrationBuilder.RenameColumn(
                name: "task_description",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "TaskDescription");

            migrationBuilder.RenameColumn(
                name: "stage_id",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "StageId");

            migrationBuilder.RenameColumn(
                name: "required_tools",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "RequiredTools");

            migrationBuilder.RenameColumn(
                name: "required_materials",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "RequiredMaterials");

            migrationBuilder.RenameColumn(
                name: "quantity_unit",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "QuantityUnit");

            migrationBuilder.RenameColumn(
                name: "quantity_per_unit",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "QuantityPerUnit");

            migrationBuilder.RenameColumn(
                name: "is_mandatory",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "IsMandatory");

            migrationBuilder.RenameColumn(
                name: "duration_minutes",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "DurationMinutes");

            migrationBuilder.RenameColumn(
                name: "created_at",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "growth_task_id",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "GrowthTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_crop_growth_tasks_stage_id",
                schema: "public",
                table: "crop_growth_tasks",
                newName: "IX_crop_growth_tasks_StageId");

            migrationBuilder.RenameColumn(
                name: "notes",
                schema: "public",
                table: "crop_growth_stages",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                schema: "public",
                table: "crop_growth_stages",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "temperature_min",
                schema: "public",
                table: "crop_growth_stages",
                newName: "TemperatureMin");

            migrationBuilder.RenameColumn(
                name: "stage_name",
                schema: "public",
                table: "crop_growth_stages",
                newName: "StageName");

            migrationBuilder.RenameColumn(
                name: "stage_description",
                schema: "public",
                table: "crop_growth_stages",
                newName: "StageDescription");

            migrationBuilder.RenameColumn(
                name: "soil_moisture_min",
                schema: "public",
                table: "crop_growth_stages",
                newName: "SoilMoistureMin");

            migrationBuilder.RenameColumn(
                name: "humidity_min",
                schema: "public",
                table: "crop_growth_stages",
                newName: "HumidityMin");

            migrationBuilder.RenameColumn(
                name: "growth_indicators",
                schema: "public",
                table: "crop_growth_stages",
                newName: "GrowthIndicators");

            migrationBuilder.RenameColumn(
                name: "crop_id",
                schema: "public",
                table: "crop_growth_stages",
                newName: "CropId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                schema: "public",
                table: "crop_growth_stages",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "common_diseases",
                schema: "public",
                table: "crop_growth_stages",
                newName: "CommonDiseases");

            migrationBuilder.RenameIndex(
                name: "IX_crop_growth_stages_crop_id",
                schema: "public",
                table: "crop_growth_stages",
                newName: "IX_crop_growth_stages_CropId");

            migrationBuilder.AddColumn<string>(
                name: "TaskName",
                schema: "public",
                table: "crop_growth_tasks",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "crop_id",
                schema: "public",
                table: "beds",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "planting_pattern",
                schema: "public",
                table: "beds",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "seasons_detail",
                schema: "public",
                columns: table => new
                {
                    season_detail_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    bed_id = table.Column<Guid>(type: "uuid", nullable: true),
                    crop_id = table.Column<Guid>(type: "uuid", nullable: true),
                    season_id = table.Column<Guid>(type: "uuid", nullable: true),
                    crop_quantity = table.Column<int>(type: "integer", nullable: true),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    season_expected_harvest_date = table.Column<DateOnly>(type: "date", nullable: true),
                    start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    total_harvest_yield = table.Column<decimal>(type: "numeric(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("seasons_detail_pkey", x => x.season_detail_id);
                    table.ForeignKey(
                        name: "seasons_detail_bed_id_fkey",
                        column: x => x.bed_id,
                        principalSchema: "public",
                        principalTable: "beds",
                        principalColumn: "bed_id");
                    table.ForeignKey(
                        name: "seasons_detail_crop_id_fkey",
                        column: x => x.crop_id,
                        principalSchema: "public",
                        principalTable: "crops",
                        principalColumn: "crop_id");
                    table.ForeignKey(
                        name: "seasons_detail_season_id_fkey",
                        column: x => x.season_id,
                        principalSchema: "public",
                        principalTable: "seasons",
                        principalColumn: "season_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_beds_crop_id",
                schema: "public",
                table: "beds",
                column: "crop_id");

            migrationBuilder.CreateIndex(
                name: "IX_seasons_detail_bed_id",
                schema: "public",
                table: "seasons_detail",
                column: "bed_id");

            migrationBuilder.CreateIndex(
                name: "IX_seasons_detail_crop_id",
                schema: "public",
                table: "seasons_detail",
                column: "crop_id");

            migrationBuilder.CreateIndex(
                name: "IX_seasons_detail_season_id",
                schema: "public",
                table: "seasons_detail",
                column: "season_id");

            migrationBuilder.AddForeignKey(
                name: "beds_crop_id_fkey",
                schema: "public",
                table: "beds",
                column: "crop_id",
                principalSchema: "public",
                principalTable: "crops",
                principalColumn: "crop_id");

            migrationBuilder.AddForeignKey(
                name: "FK_crop_growth_stages_crops_CropId",
                schema: "public",
                table: "crop_growth_stages",
                column: "CropId",
                principalSchema: "public",
                principalTable: "crops",
                principalColumn: "crop_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CropGrowthTasks_CropGrowthStages",
                schema: "public",
                table: "crop_growth_tasks",
                column: "StageId",
                principalSchema: "public",
                principalTable: "crop_growth_stages",
                principalColumn: "stage_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_growth_tracking_crop_growth_stages_StageId",
                schema: "public",
                table: "growth_tracking",
                column: "StageId",
                principalSchema: "public",
                principalTable: "crop_growth_stages",
                principalColumn: "stage_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_growth_tracking_seasons_detail_SeasonDetailId",
                schema: "public",
                table: "growth_tracking",
                column: "SeasonDetailId",
                principalSchema: "public",
                principalTable: "seasons_detail",
                principalColumn: "season_detail_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "report_created_by_fkey",
                schema: "public",
                table: "report",
                column: "created_by",
                principalSchema: "public",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_soil_crop_compatibility_crops_CropId",
                schema: "public",
                table: "soil_crop_compatibility",
                column: "CropId",
                principalSchema: "public",
                principalTable: "crops",
                principalColumn: "crop_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_soil_crop_compatibility_soil_SoilId",
                schema: "public",
                table: "soil_crop_compatibility",
                column: "SoilId",
                principalSchema: "public",
                principalTable: "soil",
                principalColumn: "soil_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
