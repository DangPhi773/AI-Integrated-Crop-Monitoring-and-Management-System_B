using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RedesignDiseaseReportFlow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE public.notification DROP CONSTRAINT IF EXISTS notification_pest_detection_id_fkey;");
            migrationBuilder.Sql("ALTER TABLE public.recommendation DROP CONSTRAINT IF EXISTS recommendation_pest_detection_id_fkey;");
            migrationBuilder.Sql("ALTER TABLE public.report DROP CONSTRAINT IF EXISTS report_worker_id_fkey;");

            migrationBuilder.Sql("DROP TABLE IF EXISTS public.pest_detections CASCADE;");
            migrationBuilder.Sql("DROP TABLE IF EXISTS public.image_analysis_result CASCADE;");
            migrationBuilder.Sql("DROP TABLE IF EXISTS public.image_analyses CASCADE;");
            migrationBuilder.Sql("DROP TABLE IF EXISTS public.photo CASCADE;");

            migrationBuilder.Sql(@"
                DO $$ BEGIN
                    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema='public' AND table_name='report' AND column_name='worker_id') THEN
                        ALTER TABLE public.report RENAME COLUMN worker_id TO season_id;
                    END IF;
                END $$;");
            migrationBuilder.Sql(@"ALTER INDEX IF EXISTS ""IX_report_worker_id"" RENAME TO ""IX_report_season_id"";");

            migrationBuilder.Sql(@"
                DO $$ BEGIN
                    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema='public' AND table_name='recommendation' AND column_name='pest_detection_id') THEN
                        ALTER TABLE public.recommendation RENAME COLUMN pest_detection_id TO diagnosis_id;
                    END IF;
                END $$;");
            migrationBuilder.Sql(@"ALTER INDEX IF EXISTS ""IX_recommendation_pest_detection_id"" RENAME TO ""IX_recommendation_diagnosis_id"";");

            migrationBuilder.Sql(@"
                DO $$ BEGIN
                    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema='public' AND table_name='notification' AND column_name='pest_detection_id') THEN
                        ALTER TABLE public.notification RENAME COLUMN pest_detection_id TO report_id;
                    END IF;
                END $$;");
            migrationBuilder.Sql(@"ALTER INDEX IF EXISTS ""IX_notification_pest_detection_id"" RENAME TO ""IX_notification_report_id"";");

            migrationBuilder.AddColumn<string>(
                name: "ai_results_json",
                schema: "public",
                table: "report",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "bed_id",
                schema: "public",
                table: "report",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "created_by",
                schema: "public",
                table: "report",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "owner_id",
                schema: "public",
                table: "report",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "plot_id",
                schema: "public",
                table: "report",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "report_no",
                schema: "public",
                table: "report",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "report_type",
                schema: "public",
                table: "report",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                schema: "public",
                table: "report",
                type: "timestamp",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "diagnosis_id",
                schema: "public",
                table: "notification",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "device_code",
                schema: "public",
                table: "iot_devices",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_active_at",
                schema: "public",
                table: "iot_devices",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "raw_data",
                schema: "public",
                table: "iot_data",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "sensor_id",
                schema: "public",
                table: "iot_data",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "attachment",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    object_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    object_id = table.Column<Guid>(type: "uuid", nullable: false),
                    attachment_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    file_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    file_url = table.Column<string>(type: "text", nullable: false),
                    cloudinary_public_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    cloudinary_secure_url = table.Column<string>(type: "text", nullable: true),
                    file_extension = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    mime_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    file_size = table.Column<long>(type: "bigint", nullable: true),
                    uploaded_by = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("attachment_pkey", x => x.id);
                    table.ForeignKey(
                        name: "attachment_uploaded_by_fkey",
                        column: x => x.uploaded_by,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "diagnosis_result",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    report_id = table.Column<Guid>(type: "uuid", nullable: false),
                    diagnosed_by = table.Column<Guid>(type: "uuid", nullable: false),
                    disease_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    conclusion = table.Column<string>(type: "text", nullable: false),
                    recommended_action = table.Column<string>(type: "text", nullable: false),
                    severity_level = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("diagnosis_result_pkey", x => x.id);
                    table.ForeignKey(
                        name: "diagnosis_result_diagnosed_by_fkey",
                        column: x => x.diagnosed_by,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "diagnosis_result_report_id_fkey",
                        column: x => x.report_id,
                        principalSchema: "public",
                        principalTable: "report",
                        principalColumn: "report_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "iot_sensor",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    device_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sensor_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    sensor_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    sensor_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    unit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    min_value = table.Column<double>(type: "double precision", nullable: true),
                    max_value = table.Column<double>(type: "double precision", nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("iot_sensor_pkey", x => x.id);
                    table.ForeignKey(
                        name: "iot_sensor_device_id_fkey",
                        column: x => x.device_id,
                        principalSchema: "public",
                        principalTable: "iot_devices",
                        principalColumn: "device_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "report_assignment",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    report_id = table.Column<Guid>(type: "uuid", nullable: false),
                    assigned_by = table.Column<Guid>(type: "uuid", nullable: false),
                    assigned_to = table.Column<Guid>(type: "uuid", nullable: false),
                    assigned_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    note = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("report_assignment_pkey", x => x.id);
                    table.ForeignKey(
                        name: "report_assignment_assigned_by_fkey",
                        column: x => x.assigned_by,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "report_assignment_assigned_to_fkey",
                        column: x => x.assigned_to,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "report_assignment_report_id_fkey",
                        column: x => x.report_id,
                        principalSchema: "public",
                        principalTable: "report",
                        principalColumn: "report_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "report_environment_snapshot",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    report_id = table.Column<Guid>(type: "uuid", nullable: false),
                    temperature = table.Column<double>(type: "double precision", nullable: true),
                    humidity = table.Column<double>(type: "double precision", nullable: true),
                    soil_moisture = table.Column<double>(type: "double precision", nullable: true),
                    rainfall = table.Column<double>(type: "double precision", nullable: true),
                    light_intensity = table.Column<double>(type: "double precision", nullable: true),
                    recorded_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    source_device_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("report_environment_snapshot_pkey", x => x.id);
                    table.ForeignKey(
                        name: "report_env_snapshot_device_id_fkey",
                        column: x => x.source_device_id,
                        principalSchema: "public",
                        principalTable: "iot_devices",
                        principalColumn: "device_id");
                    table.ForeignKey(
                        name: "report_env_snapshot_report_id_fkey",
                        column: x => x.report_id,
                        principalSchema: "public",
                        principalTable: "report",
                        principalColumn: "report_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_report_bed_id",
                schema: "public",
                table: "report",
                column: "bed_id");

            migrationBuilder.CreateIndex(
                name: "IX_report_created_by",
                schema: "public",
                table: "report",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_report_owner_id",
                schema: "public",
                table: "report",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_report_plot_id",
                schema: "public",
                table: "report",
                column: "plot_id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_diagnosis_id",
                schema: "public",
                table: "notification",
                column: "diagnosis_id");

            migrationBuilder.CreateIndex(
                name: "IX_iot_data_sensor_id",
                schema: "public",
                table: "iot_data",
                column: "sensor_id");

            migrationBuilder.CreateIndex(
                name: "idx_attachment_object",
                schema: "public",
                table: "attachment",
                columns: new[] { "object_type", "object_id" });

            migrationBuilder.CreateIndex(
                name: "IX_attachment_uploaded_by",
                schema: "public",
                table: "attachment",
                column: "uploaded_by");

            migrationBuilder.CreateIndex(
                name: "IX_diagnosis_result_diagnosed_by",
                schema: "public",
                table: "diagnosis_result",
                column: "diagnosed_by");

            migrationBuilder.CreateIndex(
                name: "IX_diagnosis_result_report_id",
                schema: "public",
                table: "diagnosis_result",
                column: "report_id");

            migrationBuilder.CreateIndex(
                name: "IX_iot_sensor_device_id",
                schema: "public",
                table: "iot_sensor",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_report_assignment_assigned_by",
                schema: "public",
                table: "report_assignment",
                column: "assigned_by");

            migrationBuilder.CreateIndex(
                name: "IX_report_assignment_assigned_to",
                schema: "public",
                table: "report_assignment",
                column: "assigned_to");

            migrationBuilder.CreateIndex(
                name: "IX_report_assignment_report_id",
                schema: "public",
                table: "report_assignment",
                column: "report_id");

            migrationBuilder.CreateIndex(
                name: "IX_report_environment_snapshot_report_id",
                schema: "public",
                table: "report_environment_snapshot",
                column: "report_id");

            migrationBuilder.CreateIndex(
                name: "IX_report_environment_snapshot_source_device_id",
                schema: "public",
                table: "report_environment_snapshot",
                column: "source_device_id");

            migrationBuilder.AddForeignKey(
                name: "iot_data_sensor_id_fkey",
                schema: "public",
                table: "iot_data",
                column: "sensor_id",
                principalSchema: "public",
                principalTable: "iot_sensor",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "notification_diagnosis_id_fkey",
                schema: "public",
                table: "notification",
                column: "diagnosis_id",
                principalSchema: "public",
                principalTable: "diagnosis_result",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "notification_report_id_fkey",
                schema: "public",
                table: "notification",
                column: "report_id",
                principalSchema: "public",
                principalTable: "report",
                principalColumn: "report_id");

            migrationBuilder.AddForeignKey(
                name: "recommendation_diagnosis_id_fkey",
                schema: "public",
                table: "recommendation",
                column: "diagnosis_id",
                principalSchema: "public",
                principalTable: "diagnosis_result",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "report_bed_id_fkey",
                schema: "public",
                table: "report",
                column: "bed_id",
                principalSchema: "public",
                principalTable: "beds",
                principalColumn: "bed_id");

            migrationBuilder.AddForeignKey(
                name: "report_created_by_fkey",
                schema: "public",
                table: "report",
                column: "created_by",
                principalSchema: "public",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "report_owner_id_fkey",
                schema: "public",
                table: "report",
                column: "owner_id",
                principalSchema: "public",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "report_plot_id_fkey",
                schema: "public",
                table: "report",
                column: "plot_id",
                principalSchema: "public",
                principalTable: "plots",
                principalColumn: "plot_id");

            migrationBuilder.AddForeignKey(
                name: "report_season_id_fkey",
                schema: "public",
                table: "report",
                column: "season_id",
                principalSchema: "public",
                principalTable: "seasons",
                principalColumn: "season_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "iot_data_sensor_id_fkey",
                schema: "public",
                table: "iot_data");

            migrationBuilder.DropForeignKey(
                name: "notification_diagnosis_id_fkey",
                schema: "public",
                table: "notification");

            migrationBuilder.DropForeignKey(
                name: "notification_report_id_fkey",
                schema: "public",
                table: "notification");

            migrationBuilder.DropForeignKey(
                name: "recommendation_diagnosis_id_fkey",
                schema: "public",
                table: "recommendation");

            migrationBuilder.DropForeignKey(
                name: "report_bed_id_fkey",
                schema: "public",
                table: "report");

            migrationBuilder.DropForeignKey(
                name: "report_created_by_fkey",
                schema: "public",
                table: "report");

            migrationBuilder.DropForeignKey(
                name: "report_owner_id_fkey",
                schema: "public",
                table: "report");

            migrationBuilder.DropForeignKey(
                name: "report_plot_id_fkey",
                schema: "public",
                table: "report");

            migrationBuilder.DropForeignKey(
                name: "report_season_id_fkey",
                schema: "public",
                table: "report");

            migrationBuilder.DropTable(
                name: "attachment",
                schema: "public");

            migrationBuilder.DropTable(
                name: "diagnosis_result",
                schema: "public");

            migrationBuilder.DropTable(
                name: "iot_sensor",
                schema: "public");

            migrationBuilder.DropTable(
                name: "report_assignment",
                schema: "public");

            migrationBuilder.DropTable(
                name: "report_environment_snapshot",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_report_bed_id",
                schema: "public",
                table: "report");

            migrationBuilder.DropIndex(
                name: "IX_report_created_by",
                schema: "public",
                table: "report");

            migrationBuilder.DropIndex(
                name: "IX_report_owner_id",
                schema: "public",
                table: "report");

            migrationBuilder.DropIndex(
                name: "IX_report_plot_id",
                schema: "public",
                table: "report");

            migrationBuilder.DropIndex(
                name: "IX_notification_diagnosis_id",
                schema: "public",
                table: "notification");

            migrationBuilder.DropIndex(
                name: "IX_iot_data_sensor_id",
                schema: "public",
                table: "iot_data");

            migrationBuilder.DropColumn(
                name: "ai_results_json",
                schema: "public",
                table: "report");

            migrationBuilder.DropColumn(
                name: "bed_id",
                schema: "public",
                table: "report");

            migrationBuilder.DropColumn(
                name: "created_by",
                schema: "public",
                table: "report");

            migrationBuilder.DropColumn(
                name: "owner_id",
                schema: "public",
                table: "report");

            migrationBuilder.DropColumn(
                name: "plot_id",
                schema: "public",
                table: "report");

            migrationBuilder.DropColumn(
                name: "report_no",
                schema: "public",
                table: "report");

            migrationBuilder.DropColumn(
                name: "report_type",
                schema: "public",
                table: "report");

            migrationBuilder.DropColumn(
                name: "updated_at",
                schema: "public",
                table: "report");

            migrationBuilder.DropColumn(
                name: "diagnosis_id",
                schema: "public",
                table: "notification");

            migrationBuilder.DropColumn(
                name: "device_code",
                schema: "public",
                table: "iot_devices");

            migrationBuilder.DropColumn(
                name: "last_active_at",
                schema: "public",
                table: "iot_devices");

            migrationBuilder.DropColumn(
                name: "raw_data",
                schema: "public",
                table: "iot_data");

            migrationBuilder.DropColumn(
                name: "sensor_id",
                schema: "public",
                table: "iot_data");

            migrationBuilder.RenameColumn(
                name: "season_id",
                schema: "public",
                table: "report",
                newName: "worker_id");

            migrationBuilder.RenameIndex(
                name: "IX_report_season_id",
                schema: "public",
                table: "report",
                newName: "IX_report_worker_id");

            migrationBuilder.RenameColumn(
                name: "diagnosis_id",
                schema: "public",
                table: "recommendation",
                newName: "pest_detection_id");

            migrationBuilder.RenameIndex(
                name: "IX_recommendation_diagnosis_id",
                schema: "public",
                table: "recommendation",
                newName: "IX_recommendation_pest_detection_id");

            migrationBuilder.RenameColumn(
                name: "report_id",
                schema: "public",
                table: "notification",
                newName: "pest_detection_id");

            migrationBuilder.RenameIndex(
                name: "IX_notification_report_id",
                schema: "public",
                table: "notification",
                newName: "IX_notification_pest_detection_id");

            migrationBuilder.CreateTable(
                name: "photo",
                schema: "public",
                columns: table => new
                {
                    photo_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    season_detail_id = table.Column<Guid>(type: "uuid", nullable: true),
                    humidity_at_the_moment_take_photo = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    photo_date = table.Column<DateOnly>(type: "date", nullable: true),
                    photo_source = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    photo_time = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    rainfall_at_the_moment_take_photo = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    soil_moisture_at_the_moment_take_photo = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    temperature_at_the_moment_take_photo = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    uploaded_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("photo_pkey", x => x.photo_id);
                    table.ForeignKey(
                        name: "photo_season_detail_id_fkey",
                        column: x => x.season_detail_id,
                        principalSchema: "public",
                        principalTable: "seasons_detail",
                        principalColumn: "season_detail_id");
                });

            migrationBuilder.CreateTable(
                name: "image_analyses",
                schema: "public",
                columns: table => new
                {
                    image_analysis_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    photo_id = table.Column<Guid>(type: "uuid", nullable: true),
                    ai_latency_ms = table.Column<int>(type: "integer", nullable: true),
                    ai_model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ai_model_version = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ai_prompt = table.Column<string>(type: "text", nullable: true),
                    ai_provider = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ai_raw_response_json = table.Column<string>(type: "jsonb", nullable: true),
                    ai_request_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ai_tokens = table.Column<int>(type: "integer", nullable: true),
                    analysis_status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    analysis_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("image_analyses_pkey", x => x.image_analysis_id);
                    table.ForeignKey(
                        name: "image_analyses_photo_id_fkey",
                        column: x => x.photo_id,
                        principalSchema: "public",
                        principalTable: "photo",
                        principalColumn: "photo_id");
                });

            migrationBuilder.CreateTable(
                name: "image_analysis_result",
                schema: "public",
                columns: table => new
                {
                    image_analysis_result_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    image_analysis_id = table.Column<Guid>(type: "uuid", nullable: true),
                    ai_confidence = table.Column<decimal>(type: "numeric(5,4)", nullable: true),
                    ai_label = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ai_severity = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    bounding_box_json = table.Column<string>(type: "jsonb", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "now()"),
                    extra_data_json = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("image_analysis_result_pkey", x => x.image_analysis_result_id);
                    table.ForeignKey(
                        name: "image_analysis_result_image_analysis_id_fkey",
                        column: x => x.image_analysis_id,
                        principalSchema: "public",
                        principalTable: "image_analyses",
                        principalColumn: "image_analysis_id");
                });

            migrationBuilder.CreateTable(
                name: "pest_detections",
                schema: "public",
                columns: table => new
                {
                    pest_detection_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    image_analysis_result_id = table.Column<Guid>(type: "uuid", nullable: true),
                    season_id = table.Column<Guid>(type: "uuid", nullable: true),
                    specialist_id = table.Column<Guid>(type: "uuid", nullable: true),
                    confidence_source = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "now()"),
                    detected_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    detection_status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    general_label = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    general_severity = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    review_notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pest_detections_pkey", x => x.pest_detection_id);
                    table.ForeignKey(
                        name: "pest_detections_image_analysis_result_id_fkey",
                        column: x => x.image_analysis_result_id,
                        principalSchema: "public",
                        principalTable: "image_analysis_result",
                        principalColumn: "image_analysis_result_id");
                    table.ForeignKey(
                        name: "pest_detections_season_id_fkey",
                        column: x => x.season_id,
                        principalSchema: "public",
                        principalTable: "seasons",
                        principalColumn: "season_id");
                    table.ForeignKey(
                        name: "pest_detections_specialist_id_fkey",
                        column: x => x.specialist_id,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_image_analyses_photo_id",
                schema: "public",
                table: "image_analyses",
                column: "photo_id");

            migrationBuilder.CreateIndex(
                name: "IX_image_analysis_result_image_analysis_id",
                schema: "public",
                table: "image_analysis_result",
                column: "image_analysis_id");

            migrationBuilder.CreateIndex(
                name: "IX_pest_detections_image_analysis_result_id",
                schema: "public",
                table: "pest_detections",
                column: "image_analysis_result_id");

            migrationBuilder.CreateIndex(
                name: "IX_pest_detections_season_id",
                schema: "public",
                table: "pest_detections",
                column: "season_id");

            migrationBuilder.CreateIndex(
                name: "IX_pest_detections_specialist_id",
                schema: "public",
                table: "pest_detections",
                column: "specialist_id");

            migrationBuilder.CreateIndex(
                name: "IX_photo_season_detail_id",
                schema: "public",
                table: "photo",
                column: "season_detail_id");

            migrationBuilder.AddForeignKey(
                name: "notification_pest_detection_id_fkey",
                schema: "public",
                table: "notification",
                column: "pest_detection_id",
                principalSchema: "public",
                principalTable: "pest_detections",
                principalColumn: "pest_detection_id");

            migrationBuilder.AddForeignKey(
                name: "recommendation_pest_detection_id_fkey",
                schema: "public",
                table: "recommendation",
                column: "pest_detection_id",
                principalSchema: "public",
                principalTable: "pest_detections",
                principalColumn: "pest_detection_id");

            migrationBuilder.AddForeignKey(
                name: "report_worker_id_fkey",
                schema: "public",
                table: "report",
                column: "worker_id",
                principalSchema: "public",
                principalTable: "users",
                principalColumn: "user_id");
        }
    }
}
