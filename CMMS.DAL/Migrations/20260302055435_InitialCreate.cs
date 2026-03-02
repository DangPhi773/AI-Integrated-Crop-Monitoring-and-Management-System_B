using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:auth.aal_level", "aal1,aal2,aal3")
                .Annotation("Npgsql:Enum:auth.code_challenge_method", "s256,plain")
                .Annotation("Npgsql:Enum:auth.factor_status", "unverified,verified")
                .Annotation("Npgsql:Enum:auth.factor_type", "totp,webauthn,phone")
                .Annotation("Npgsql:Enum:auth.oauth_authorization_status", "pending,approved,denied,expired")
                .Annotation("Npgsql:Enum:auth.oauth_client_type", "public,confidential")
                .Annotation("Npgsql:Enum:auth.oauth_registration_type", "dynamic,manual")
                .Annotation("Npgsql:Enum:auth.oauth_response_type", "code")
                .Annotation("Npgsql:Enum:auth.one_time_token_type", "confirmation_token,reauthentication_token,recovery_token,email_change_token_new,email_change_token_current,phone_change_token")
                .Annotation("Npgsql:Enum:realtime.action", "INSERT,UPDATE,DELETE,TRUNCATE,ERROR")
                .Annotation("Npgsql:Enum:realtime.equality_op", "eq,neq,lt,lte,gt,gte,in")
                .Annotation("Npgsql:Enum:storage.buckettype", "STANDARD,ANALYTICS,VECTOR")
                .Annotation("Npgsql:PostgresExtension:extensions.pg_stat_statements", ",,")
                .Annotation("Npgsql:PostgresExtension:extensions.pgcrypto", ",,")
                .Annotation("Npgsql:PostgresExtension:extensions.uuid-ossp", ",,")
                .Annotation("Npgsql:PostgresExtension:graphql.pg_graphql", ",,")
                .Annotation("Npgsql:PostgresExtension:vault.supabase_vault", ",,");

            migrationBuilder.CreateTable(
                name: "farms",
                columns: table => new
                {
                    farm_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    farm_name = table.Column<string>(type: "text", nullable: false),
                    farm_location = table.Column<string>(type: "text", nullable: true),
                    farm_area = table.Column<decimal>(type: "numeric", nullable: true),
                    farm_status = table.Column<string>(type: "text", nullable: true),
                    farm_created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("farms_pkey", x => x.farm_id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    role_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    role_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("role_pkey", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "soil",
                columns: table => new
                {
                    soil_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "text", nullable: false),
                    science_name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("soil_pkey", x => x.soil_id);
                });

            migrationBuilder.CreateTable(
                name: "seasons",
                columns: table => new
                {
                    season_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    farm_id = table.Column<Guid>(type: "uuid", nullable: true),
                    season_name = table.Column<string>(type: "text", nullable: true),
                    season_start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    season_end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    season_notes = table.Column<string>(type: "text", nullable: true),
                    season_created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("seasons_pkey", x => x.season_id);
                    table.ForeignKey(
                        name: "seasons_farm_id_fkey",
                        column: x => x.farm_id,
                        principalTable: "farms",
                        principalColumn: "farm_id");
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    role_id = table.Column<Guid>(type: "uuid", nullable: true),
                    email = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: true),
                    fullname = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.user_id);
                    table.ForeignKey(
                        name: "users_role_id_fkey",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "role_id");
                });

            migrationBuilder.CreateTable(
                name: "crops",
                columns: table => new
                {
                    crop_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    soil_id = table.Column<Guid>(type: "uuid", nullable: true),
                    crop_name = table.Column<string>(type: "text", nullable: false),
                    crop_scientific_name = table.Column<string>(type: "text", nullable: true),
                    crop_default_growth_days = table.Column<int>(type: "integer", nullable: true),
                    crop_quantities = table.Column<int>(type: "integer", nullable: true),
                    crop_status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("crops_pkey", x => x.crop_id);
                    table.ForeignKey(
                        name: "crops_soil_id_fkey",
                        column: x => x.soil_id,
                        principalTable: "soil",
                        principalColumn: "soil_id");
                });

            migrationBuilder.CreateTable(
                name: "plots",
                columns: table => new
                {
                    plot_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    soil_id = table.Column<Guid>(type: "uuid", nullable: true),
                    farm_id = table.Column<Guid>(type: "uuid", nullable: true),
                    plot_name = table.Column<string>(type: "text", nullable: true),
                    plot_area = table.Column<decimal>(type: "numeric", nullable: true),
                    plot_status = table.Column<string>(type: "text", nullable: true),
                    bed_created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("plots_pkey", x => x.plot_id);
                    table.ForeignKey(
                        name: "plots_farm_id_fkey",
                        column: x => x.farm_id,
                        principalTable: "farms",
                        principalColumn: "farm_id");
                    table.ForeignKey(
                        name: "plots_soil_id_fkey",
                        column: x => x.soil_id,
                        principalTable: "soil",
                        principalColumn: "soil_id");
                });

            migrationBuilder.CreateTable(
                name: "recommendation_tasks",
                columns: table => new
                {
                    recommendation_task_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_by_owner_id = table.Column<Guid>(type: "uuid", nullable: true),
                    assigned_to_worker_id = table.Column<Guid>(type: "uuid", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    task_scheduled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    task_status = table.Column<string>(type: "text", nullable: true),
                    task_created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("recommendation_tasks_pkey", x => x.recommendation_task_id);
                    table.ForeignKey(
                        name: "recommendation_tasks_assigned_to_worker_id_fkey",
                        column: x => x.assigned_to_worker_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "recommendation_tasks_created_by_owner_id_fkey",
                        column: x => x.created_by_owner_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "report",
                columns: table => new
                {
                    report_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    worker_id = table.Column<Guid>(type: "uuid", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    submit_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("report_pkey", x => x.report_id);
                    table.ForeignKey(
                        name: "report_worker_id_fkey",
                        column: x => x.worker_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "tasks",
                columns: table => new
                {
                    task_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    assigned_to_worker_id = table.Column<Guid>(type: "uuid", nullable: true),
                    season_id = table.Column<Guid>(type: "uuid", nullable: true),
                    task_title = table.Column<string>(type: "text", nullable: true),
                    task_scheduled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    task_status = table.Column<string>(type: "text", nullable: true),
                    task_notes = table.Column<string>(type: "text", nullable: true),
                    task_created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("tasks_pkey", x => x.task_id);
                    table.ForeignKey(
                        name: "tasks_assigned_to_worker_id_fkey",
                        column: x => x.assigned_to_worker_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "tasks_season_id_fkey",
                        column: x => x.season_id,
                        principalTable: "seasons",
                        principalColumn: "season_id");
                });

            migrationBuilder.CreateTable(
                name: "beds",
                columns: table => new
                {
                    bed_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    plot_id = table.Column<Guid>(type: "uuid", nullable: true),
                    bed_name = table.Column<string>(type: "text", nullable: true),
                    bed_area = table.Column<decimal>(type: "numeric", nullable: true),
                    bed_status = table.Column<string>(type: "text", nullable: true),
                    bed_created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    crop_quantities = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("beds_pkey", x => x.bed_id);
                    table.ForeignKey(
                        name: "beds_plot_id_fkey",
                        column: x => x.plot_id,
                        principalTable: "plots",
                        principalColumn: "plot_id");
                });

            migrationBuilder.CreateTable(
                name: "recommendation_task_detail",
                columns: table => new
                {
                    task_detail_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    task_id = table.Column<Guid>(type: "uuid", nullable: true),
                    worker_id = table.Column<Guid>(type: "uuid", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    quantity = table.Column<decimal>(type: "numeric", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true),
                    unit = table.Column<string>(type: "text", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("recommendation_task_detail_pkey", x => x.task_detail_id);
                    table.ForeignKey(
                        name: "recommendation_task_detail_task_id_fkey",
                        column: x => x.task_id,
                        principalTable: "recommendation_tasks",
                        principalColumn: "recommendation_task_id");
                    table.ForeignKey(
                        name: "recommendation_task_detail_worker_id_fkey",
                        column: x => x.worker_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "task_detail",
                columns: table => new
                {
                    task_detail_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    task_id = table.Column<Guid>(type: "uuid", nullable: true),
                    season_id = table.Column<Guid>(type: "uuid", nullable: true),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("task_detail_pkey", x => x.task_detail_id);
                    table.ForeignKey(
                        name: "task_detail_season_id_fkey",
                        column: x => x.season_id,
                        principalTable: "seasons",
                        principalColumn: "season_id");
                    table.ForeignKey(
                        name: "task_detail_task_id_fkey",
                        column: x => x.task_id,
                        principalTable: "tasks",
                        principalColumn: "task_id");
                });

            migrationBuilder.CreateTable(
                name: "seasons_detail",
                columns: table => new
                {
                    season_detail_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    season_id = table.Column<Guid>(type: "uuid", nullable: true),
                    bed_id = table.Column<Guid>(type: "uuid", nullable: true),
                    crop_id = table.Column<Guid>(type: "uuid", nullable: true),
                    season_expected_harvest_date = table.Column<DateOnly>(type: "date", nullable: true),
                    crop_quantity = table.Column<int>(type: "integer", nullable: true),
                    start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    total_harvest_yield = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("seasons_detail_pkey", x => x.season_detail_id);
                    table.ForeignKey(
                        name: "seasons_detail_bed_id_fkey",
                        column: x => x.bed_id,
                        principalTable: "beds",
                        principalColumn: "bed_id");
                    table.ForeignKey(
                        name: "seasons_detail_crop_id_fkey",
                        column: x => x.crop_id,
                        principalTable: "crops",
                        principalColumn: "crop_id");
                    table.ForeignKey(
                        name: "seasons_detail_season_id_fkey",
                        column: x => x.season_id,
                        principalTable: "seasons",
                        principalColumn: "season_id");
                });

            migrationBuilder.CreateTable(
                name: "worker_schedule",
                columns: table => new
                {
                    schedule_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    task_detail_id = table.Column<Guid>(type: "uuid", nullable: true),
                    worker_id = table.Column<Guid>(type: "uuid", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("worker_schedule_pkey", x => x.schedule_id);
                    table.ForeignKey(
                        name: "worker_schedule_task_detail_id_fkey",
                        column: x => x.task_detail_id,
                        principalTable: "task_detail",
                        principalColumn: "task_detail_id");
                    table.ForeignKey(
                        name: "worker_schedule_worker_id_fkey",
                        column: x => x.worker_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "photo",
                columns: table => new
                {
                    photo_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    season_detail_id = table.Column<Guid>(type: "uuid", nullable: true),
                    photo_date = table.Column<DateOnly>(type: "date", nullable: true),
                    photo_time = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    temperature_at_the_moment_take_photo = table.Column<decimal>(type: "numeric", nullable: true),
                    humidity_at_the_moment_take_photo = table.Column<decimal>(type: "numeric", nullable: true),
                    soil_moisture_at_the_moment_take_photo = table.Column<decimal>(type: "numeric", nullable: true),
                    rainfall_at_the_moment_take_photo = table.Column<decimal>(type: "numeric", nullable: true),
                    photo_source = table.Column<string>(type: "text", nullable: true),
                    uploaded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("photo_pkey", x => x.photo_id);
                    table.ForeignKey(
                        name: "photo_season_detail_id_fkey",
                        column: x => x.season_detail_id,
                        principalTable: "seasons_detail",
                        principalColumn: "season_detail_id");
                });

            migrationBuilder.CreateTable(
                name: "image_analyses",
                columns: table => new
                {
                    image_analysis_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    photo_id = table.Column<Guid>(type: "uuid", nullable: true),
                    analysis_type = table.Column<string>(type: "text", nullable: true),
                    analysis_status = table.Column<string>(type: "text", nullable: true),
                    ai_provider = table.Column<string>(type: "text", nullable: true),
                    ai_model = table.Column<string>(type: "text", nullable: true),
                    ai_model_version = table.Column<string>(type: "text", nullable: true),
                    ai_prompt = table.Column<string>(type: "text", nullable: true),
                    ai_request_id = table.Column<string>(type: "text", nullable: true),
                    ai_tokens = table.Column<int>(type: "integer", nullable: true),
                    ai_latency_ms = table.Column<int>(type: "integer", nullable: true),
                    ai_raw_response_json = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("image_analyses_pkey", x => x.image_analysis_id);
                    table.ForeignKey(
                        name: "image_analyses_photo_id_fkey",
                        column: x => x.photo_id,
                        principalTable: "photo",
                        principalColumn: "photo_id");
                });

            migrationBuilder.CreateTable(
                name: "image_analysis_result",
                columns: table => new
                {
                    image_analysis_result_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    image_analysis_id = table.Column<Guid>(type: "uuid", nullable: true),
                    ai_label = table.Column<string>(type: "text", nullable: true),
                    ai_confidence = table.Column<decimal>(type: "numeric", nullable: true),
                    ai_severity = table.Column<string>(type: "text", nullable: true),
                    bounding_box_json = table.Column<string>(type: "jsonb", nullable: true),
                    extra_data_json = table.Column<string>(type: "jsonb", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("image_analysis_result_pkey", x => x.image_analysis_result_id);
                    table.ForeignKey(
                        name: "image_analysis_result_image_analysis_id_fkey",
                        column: x => x.image_analysis_id,
                        principalTable: "image_analyses",
                        principalColumn: "image_analysis_id");
                });

            migrationBuilder.CreateTable(
                name: "pest_detections",
                columns: table => new
                {
                    pest_detection_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    season_id = table.Column<Guid>(type: "uuid", nullable: true),
                    specialist_id = table.Column<Guid>(type: "uuid", nullable: true),
                    image_analysis_result_id = table.Column<Guid>(type: "uuid", nullable: true),
                    general_label = table.Column<string>(type: "text", nullable: true),
                    general_severity = table.Column<string>(type: "text", nullable: true),
                    confidence_source = table.Column<string>(type: "text", nullable: true),
                    detection_status = table.Column<string>(type: "text", nullable: true),
                    review_notes = table.Column<string>(type: "text", nullable: true),
                    detected_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pest_detections_pkey", x => x.pest_detection_id);
                    table.ForeignKey(
                        name: "pest_detections_image_analysis_result_id_fkey",
                        column: x => x.image_analysis_result_id,
                        principalTable: "image_analysis_result",
                        principalColumn: "image_analysis_result_id");
                    table.ForeignKey(
                        name: "pest_detections_season_id_fkey",
                        column: x => x.season_id,
                        principalTable: "seasons",
                        principalColumn: "season_id");
                    table.ForeignKey(
                        name: "pest_detections_specialist_id_fkey",
                        column: x => x.specialist_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "notification",
                columns: table => new
                {
                    note_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    pest_detection_id = table.Column<Guid>(type: "uuid", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    note_type = table.Column<string>(type: "text", nullable: true),
                    note_title = table.Column<string>(type: "text", nullable: true),
                    note_message = table.Column<string>(type: "text", nullable: true),
                    note_status = table.Column<string>(type: "text", nullable: true),
                    note_created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("notification_pkey", x => x.note_id);
                    table.ForeignKey(
                        name: "notification_pest_detection_id_fkey",
                        column: x => x.pest_detection_id,
                        principalTable: "pest_detections",
                        principalColumn: "pest_detection_id");
                    table.ForeignKey(
                        name: "notification_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "recommendation",
                columns: table => new
                {
                    recommendation_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    season_id = table.Column<Guid>(type: "uuid", nullable: true),
                    pest_detection_id = table.Column<Guid>(type: "uuid", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("recommendation_pkey", x => x.recommendation_id);
                    table.ForeignKey(
                        name: "recommendation_pest_detection_id_fkey",
                        column: x => x.pest_detection_id,
                        principalTable: "pest_detections",
                        principalColumn: "pest_detection_id");
                    table.ForeignKey(
                        name: "recommendation_season_id_fkey",
                        column: x => x.season_id,
                        principalTable: "seasons",
                        principalColumn: "season_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_beds_plot_id",
                table: "beds",
                column: "plot_id");

            migrationBuilder.CreateIndex(
                name: "IX_crops_soil_id",
                table: "crops",
                column: "soil_id");

            migrationBuilder.CreateIndex(
                name: "IX_image_analyses_photo_id",
                table: "image_analyses",
                column: "photo_id");

            migrationBuilder.CreateIndex(
                name: "IX_image_analysis_result_image_analysis_id",
                table: "image_analysis_result",
                column: "image_analysis_id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_pest_detection_id",
                table: "notification",
                column: "pest_detection_id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_user_id",
                table: "notification",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_pest_detections_image_analysis_result_id",
                table: "pest_detections",
                column: "image_analysis_result_id");

            migrationBuilder.CreateIndex(
                name: "IX_pest_detections_season_id",
                table: "pest_detections",
                column: "season_id");

            migrationBuilder.CreateIndex(
                name: "IX_pest_detections_specialist_id",
                table: "pest_detections",
                column: "specialist_id");

            migrationBuilder.CreateIndex(
                name: "IX_photo_season_detail_id",
                table: "photo",
                column: "season_detail_id");

            migrationBuilder.CreateIndex(
                name: "IX_plots_farm_id",
                table: "plots",
                column: "farm_id");

            migrationBuilder.CreateIndex(
                name: "IX_plots_soil_id",
                table: "plots",
                column: "soil_id");

            migrationBuilder.CreateIndex(
                name: "IX_recommendation_pest_detection_id",
                table: "recommendation",
                column: "pest_detection_id");

            migrationBuilder.CreateIndex(
                name: "IX_recommendation_season_id",
                table: "recommendation",
                column: "season_id");

            migrationBuilder.CreateIndex(
                name: "IX_recommendation_task_detail_task_id",
                table: "recommendation_task_detail",
                column: "task_id");

            migrationBuilder.CreateIndex(
                name: "IX_recommendation_task_detail_worker_id",
                table: "recommendation_task_detail",
                column: "worker_id");

            migrationBuilder.CreateIndex(
                name: "IX_recommendation_tasks_assigned_to_worker_id",
                table: "recommendation_tasks",
                column: "assigned_to_worker_id");

            migrationBuilder.CreateIndex(
                name: "IX_recommendation_tasks_created_by_owner_id",
                table: "recommendation_tasks",
                column: "created_by_owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_report_worker_id",
                table: "report",
                column: "worker_id");

            migrationBuilder.CreateIndex(
                name: "IX_seasons_farm_id",
                table: "seasons",
                column: "farm_id");

            migrationBuilder.CreateIndex(
                name: "IX_seasons_detail_bed_id",
                table: "seasons_detail",
                column: "bed_id");

            migrationBuilder.CreateIndex(
                name: "IX_seasons_detail_crop_id",
                table: "seasons_detail",
                column: "crop_id");

            migrationBuilder.CreateIndex(
                name: "IX_seasons_detail_season_id",
                table: "seasons_detail",
                column: "season_id");

            migrationBuilder.CreateIndex(
                name: "IX_task_detail_season_id",
                table: "task_detail",
                column: "season_id");

            migrationBuilder.CreateIndex(
                name: "IX_task_detail_task_id",
                table: "task_detail",
                column: "task_id");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_assigned_to_worker_id",
                table: "tasks",
                column: "assigned_to_worker_id");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_season_id",
                table: "tasks",
                column: "season_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_role_id",
                table: "users",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "users_email_key",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_worker_schedule_task_detail_id",
                table: "worker_schedule",
                column: "task_detail_id");

            migrationBuilder.CreateIndex(
                name: "IX_worker_schedule_worker_id",
                table: "worker_schedule",
                column: "worker_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notification");

            migrationBuilder.DropTable(
                name: "recommendation");

            migrationBuilder.DropTable(
                name: "recommendation_task_detail");

            migrationBuilder.DropTable(
                name: "report");

            migrationBuilder.DropTable(
                name: "worker_schedule");

            migrationBuilder.DropTable(
                name: "pest_detections");

            migrationBuilder.DropTable(
                name: "recommendation_tasks");

            migrationBuilder.DropTable(
                name: "task_detail");

            migrationBuilder.DropTable(
                name: "image_analysis_result");

            migrationBuilder.DropTable(
                name: "tasks");

            migrationBuilder.DropTable(
                name: "image_analyses");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "photo");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "seasons_detail");

            migrationBuilder.DropTable(
                name: "beds");

            migrationBuilder.DropTable(
                name: "crops");

            migrationBuilder.DropTable(
                name: "seasons");

            migrationBuilder.DropTable(
                name: "plots");

            migrationBuilder.DropTable(
                name: "farms");

            migrationBuilder.DropTable(
                name: "soil");
        }
    }
}
