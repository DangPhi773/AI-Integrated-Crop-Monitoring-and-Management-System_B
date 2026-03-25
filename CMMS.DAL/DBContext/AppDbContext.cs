using CMMS.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;
using System.Collections.Generic;
using Task = CMMS.DAL.Entities.Task;

namespace CMMS.DAL.DBContext;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bed> Beds { get; set; }

    public virtual DbSet<Crop> Crops { get; set; }

    public virtual DbSet<Farm> Farms { get; set; }

    public virtual DbSet<ImageAnalysis> ImageAnalyses { get; set; }

    public virtual DbSet<ImageAnalysisResult> ImageAnalysisResults { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<PestDetection> PestDetections { get; set; }

    public virtual DbSet<Photo> Photos { get; set; }

    public virtual DbSet<Plot> Plots { get; set; }

    public virtual DbSet<Recommendation> Recommendations { get; set; }

    public virtual DbSet<RecommendationTask> RecommendationTasks { get; set; }

    public virtual DbSet<RecommendationTaskDetail> RecommendationTaskDetails { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Season> Seasons { get; set; }

    public virtual DbSet<SeasonsDetail> SeasonsDetails { get; set; }

    public virtual DbSet<Soil> Soils { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TaskDetail> TaskDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WorkerSchedule> WorkerSchedules { get; set; }
    public virtual DbSet<IotDevice> IotDevices { get; set; } = null!;
    public virtual DbSet<IotData> IotDatas { get; set; } = null!;
    public virtual DbSet<SoilCropCompatibility> SoilCropCompatibilities { get; set; }
    public virtual DbSet<CropGrowthStage> CropGrowthStages { get; set; }
    public virtual DbSet<CropGrowthTask> CropGrowthTasks { get; set; }
    public virtual DbSet<GrowthTracking> GrowthTrackings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Name=DefaultConnectionStringDB");
        }
    }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseNpgsql("Host=db.dldvqllcbicgdkiokoeu.supabase.co;Port=6543;Database=postgres;Username=postgres;Password=aICMms9K0Rk5OGKH;SSL Mode=Require;Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("public");
        //modelBuilder
        //    .HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
        //    .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
        //    .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
        //    .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn", "phone" })
        //    .HasPostgresEnum("auth", "oauth_authorization_status", new[] { "pending", "approved", "denied", "expired" })
        //    .HasPostgresEnum("auth", "oauth_client_type", new[] { "public", "confidential" })
        //    .HasPostgresEnum("auth", "oauth_registration_type", new[] { "dynamic", "manual" })
        //    .HasPostgresEnum("auth", "oauth_response_type", new[] { "code" })
        //    .HasPostgresEnum("auth", "one_time_token_type", new[] { "confirmation_token", "reauthentication_token", "recovery_token", "email_change_token_new", "email_change_token_current", "phone_change_token" })
        //    .HasPostgresEnum("realtime", "action", new[] { "INSERT", "UPDATE", "DELETE", "TRUNCATE", "ERROR" })
        //    .HasPostgresEnum("realtime", "equality_op", new[] { "eq", "neq", "lt", "lte", "gt", "gte", "in" })
        //    .HasPostgresEnum("storage", "buckettype", new[] { "STANDARD", "ANALYTICS", "VECTOR" })
        //    .HasPostgresExtension("extensions", "pg_stat_statements")
        //    .HasPostgresExtension("extensions", "pgcrypto")
        //    .HasPostgresExtension("extensions", "uuid-ossp")
        //    .HasPostgresExtension("graphql", "pg_graphql")
        //    .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<Bed>(entity =>
        {
            entity.HasKey(e => e.BedId).HasName("beds_pkey");

            entity.ToTable("beds");

            entity.Property(e => e.BedId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("bed_id");
            entity.Property(e => e.BedArea).HasColumnName("bed_area");
            entity.Property(e => e.BedCreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("bed_created_at");
            entity.Property(e => e.BedName).HasColumnName("bed_name");
            entity.Property(e => e.BedStatus).HasColumnName("bed_status");
            entity.Property(e => e.CropQuantities).HasColumnName("crop_quantities");
            entity.Property(e => e.PlotId).HasColumnName("plot_id");

            entity.HasOne(d => d.Plot).WithMany(p => p.Beds)
                .HasForeignKey(d => d.PlotId)
                .HasConstraintName("beds_plot_id_fkey");
        });

        modelBuilder.Entity<Crop>(entity =>
        {
            entity.HasKey(e => e.CropId).HasName("crops_pkey");

            entity.ToTable("crops");

            entity.Property(e => e.CropId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("crop_id");
            entity.Property(e => e.CropDefaultGrowthDays).HasColumnName("crop_default_growth_days");
            entity.Property(e => e.CropName).HasColumnName("crop_name");
            entity.Property(e => e.PlantSpacing).HasColumnName("plant_spacing");
            entity.Property(e => e.CropQuantities).HasColumnName("crop_quantities");
            entity.Property(e => e.CropScientificName).HasColumnName("crop_scientific_name");
            entity.Property(e => e.CropStatus).HasColumnName("crop_status");
            entity.Property(e => e.SoilId).HasColumnName("soil_id");

            //entity.HasOne(d => d.Soil).WithMany(p => p.Crops)
            //    .HasForeignKey(d => d.SoilId)
            //    .HasConstraintName("crops_soil_id_fkey");
        });

        modelBuilder.Entity<Farm>(entity =>
        {
            entity.HasKey(e => e.FarmId).HasName("farms_pkey");

            entity.ToTable("farms");

            entity.Property(e => e.FarmId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("farm_id");
            entity.Property(e => e.FarmArea).HasColumnName("farm_area");
            entity.Property(e => e.FarmCreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("farm_created_at");
            entity.Property(e => e.FarmLocation).HasColumnName("farm_location");
            entity.Property(e => e.FarmName).HasColumnName("farm_name");
            entity.Property(e => e.FarmStatus).HasColumnName("farm_status");
        });

        modelBuilder.Entity<ImageAnalysis>(entity =>
        {
            entity.HasKey(e => e.ImageAnalysisId).HasName("image_analyses_pkey");

            entity.ToTable("image_analyses");

            entity.Property(e => e.ImageAnalysisId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("image_analysis_id");
            entity.Property(e => e.AiLatencyMs).HasColumnName("ai_latency_ms");
            entity.Property(e => e.AiModel).HasColumnName("ai_model");
            entity.Property(e => e.AiModelVersion).HasColumnName("ai_model_version");
            entity.Property(e => e.AiPrompt).HasColumnName("ai_prompt");
            entity.Property(e => e.AiProvider).HasColumnName("ai_provider");
            entity.Property(e => e.AiRawResponseJson)
                .HasColumnType("jsonb")
                .HasColumnName("ai_raw_response_json");
            entity.Property(e => e.AiRequestId).HasColumnName("ai_request_id");
            entity.Property(e => e.AiTokens).HasColumnName("ai_tokens");
            entity.Property(e => e.AnalysisStatus).HasColumnName("analysis_status");
            entity.Property(e => e.AnalysisType).HasColumnName("analysis_type");
            entity.Property(e => e.PhotoId).HasColumnName("photo_id");

            entity.HasOne(d => d.Photo).WithMany(p => p.ImageAnalyses)
                .HasForeignKey(d => d.PhotoId)
                .HasConstraintName("image_analyses_photo_id_fkey");
        });

        modelBuilder.Entity<ImageAnalysisResult>(entity =>
        {
            entity.HasKey(e => e.ImageAnalysisResultId).HasName("image_analysis_result_pkey");

            entity.ToTable("image_analysis_result");

            entity.Property(e => e.ImageAnalysisResultId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("image_analysis_result_id");
            entity.Property(e => e.AiConfidence).HasColumnName("ai_confidence");
            entity.Property(e => e.AiLabel).HasColumnName("ai_label");
            entity.Property(e => e.AiSeverity).HasColumnName("ai_severity");
            entity.Property(e => e.BoundingBoxJson)
                .HasColumnType("jsonb")
                .HasColumnName("bounding_box_json");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.ExtraDataJson)
                .HasColumnType("jsonb")
                .HasColumnName("extra_data_json");
            entity.Property(e => e.ImageAnalysisId).HasColumnName("image_analysis_id");

            entity.HasOne(d => d.ImageAnalysis).WithMany(p => p.ImageAnalysisResults)
                .HasForeignKey(d => d.ImageAnalysisId)
                .HasConstraintName("image_analysis_result_image_analysis_id_fkey");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NoteId).HasName("notification_pkey");

            entity.ToTable("notification");

            entity.Property(e => e.NoteId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("note_id");
            entity.Property(e => e.NoteCreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("note_created_at");
            entity.Property(e => e.NoteMessage).HasColumnName("note_message");
            entity.Property(e => e.NoteStatus).HasColumnName("note_status");
            entity.Property(e => e.NoteTitle).HasColumnName("note_title");
            entity.Property(e => e.NoteType).HasColumnName("note_type");
            entity.Property(e => e.PestDetectionId).HasColumnName("pest_detection_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.PestDetection).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.PestDetectionId)
                .HasConstraintName("notification_pest_detection_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("notification_user_id_fkey");
        });

        modelBuilder.Entity<PestDetection>(entity =>
        {
            entity.HasKey(e => e.PestDetectionId).HasName("pest_detections_pkey");

            entity.ToTable("pest_detections");

            entity.Property(e => e.PestDetectionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("pest_detection_id");
            entity.Property(e => e.ConfidenceSource).HasColumnName("confidence_source");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DetectedAt).HasColumnName("detected_at");
            entity.Property(e => e.DetectionStatus).HasColumnName("detection_status");
            entity.Property(e => e.GeneralLabel).HasColumnName("general_label");
            entity.Property(e => e.GeneralSeverity).HasColumnName("general_severity");
            entity.Property(e => e.ImageAnalysisResultId).HasColumnName("image_analysis_result_id");
            entity.Property(e => e.ReviewNotes).HasColumnName("review_notes");
            entity.Property(e => e.SeasonId).HasColumnName("season_id");
            entity.Property(e => e.SpecialistId).HasColumnName("specialist_id");

            entity.HasOne(d => d.ImageAnalysisResult).WithMany(p => p.PestDetections)
                .HasForeignKey(d => d.ImageAnalysisResultId)
                .HasConstraintName("pest_detections_image_analysis_result_id_fkey");

            entity.HasOne(d => d.Season).WithMany(p => p.PestDetections)
                .HasForeignKey(d => d.SeasonId)
                .HasConstraintName("pest_detections_season_id_fkey");

            entity.HasOne(d => d.Specialist).WithMany(p => p.PestDetections)
                .HasForeignKey(d => d.SpecialistId)
                .HasConstraintName("pest_detections_specialist_id_fkey");
        });

        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasKey(e => e.PhotoId).HasName("photo_pkey");

            entity.ToTable("photo");

            entity.Property(e => e.PhotoId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("photo_id");
            entity.Property(e => e.HumidityAtTheMomentTakePhoto).HasColumnName("humidity_at_the_moment_take_photo");
            entity.Property(e => e.PhotoDate).HasColumnName("photo_date");
            entity.Property(e => e.PhotoSource).HasColumnName("photo_source");
            entity.Property(e => e.PhotoTime).HasColumnName("photo_time");
            entity.Property(e => e.RainfallAtTheMomentTakePhoto).HasColumnName("rainfall_at_the_moment_take_photo");
            entity.Property(e => e.SeasonDetailId).HasColumnName("season_detail_id");
            entity.Property(e => e.SoilMoistureAtTheMomentTakePhoto).HasColumnName("soil_moisture_at_the_moment_take_photo");
            entity.Property(e => e.TemperatureAtTheMomentTakePhoto).HasColumnName("temperature_at_the_moment_take_photo");
            entity.Property(e => e.UploadedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("uploaded_at");

            entity.HasOne(d => d.SeasonDetail).WithMany(p => p.Photos)
                .HasForeignKey(d => d.SeasonDetailId)
                .HasConstraintName("photo_season_detail_id_fkey");
        });

        modelBuilder.Entity<Plot>(entity =>
        {
            entity.HasKey(e => e.PlotId).HasName("plots_pkey");

            entity.ToTable("plots");

            entity.Property(e => e.PlotId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("plot_id");
            entity.Property(e => e.BedCreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("bed_created_at");
            entity.Property(e => e.FarmId).HasColumnName("farm_id");
            entity.Property(e => e.PlotArea).HasColumnName("plot_area");
            entity.Property(e => e.PlotName).HasColumnName("plot_name");
            entity.Property(e => e.PlotStatus).HasColumnName("plot_status");
            entity.Property(e => e.SoilId).HasColumnName("soil_id");

            entity.HasOne(d => d.Farm).WithMany(p => p.Plots)
                .HasForeignKey(d => d.FarmId)
                .HasConstraintName("plots_farm_id_fkey");

            entity.HasOne(d => d.Soil).WithMany(p => p.Plots)
                .HasForeignKey(d => d.SoilId)
                .HasConstraintName("plots_soil_id_fkey");
        });

        modelBuilder.Entity<Recommendation>(entity =>
        {
            entity.HasKey(e => e.RecommendationId).HasName("recommendation_pkey");

            entity.ToTable("recommendation");

            entity.Property(e => e.RecommendationId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("recommendation_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.PestDetectionId).HasColumnName("pest_detection_id");
            entity.Property(e => e.SeasonId).HasColumnName("season_id");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.PestDetection).WithMany(p => p.Recommendations)
                .HasForeignKey(d => d.PestDetectionId)
                .HasConstraintName("recommendation_pest_detection_id_fkey");

            entity.HasOne(d => d.Season).WithMany(p => p.Recommendations)
                .HasForeignKey(d => d.SeasonId)
                .HasConstraintName("recommendation_season_id_fkey");
        });

        modelBuilder.Entity<RecommendationTask>(entity =>
        {
            entity.HasKey(e => e.RecommendationTaskId).HasName("recommendation_tasks_pkey");

            entity.ToTable("recommendation_tasks");

            entity.Property(e => e.RecommendationTaskId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("recommendation_task_id");
            entity.Property(e => e.AssignedToWorkerId).HasColumnName("assigned_to_worker_id");
            entity.Property(e => e.CreatedByOwnerId).HasColumnName("created_by_owner_id");
            entity.Property(e => e.TaskCreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("task_created_at");
            entity.Property(e => e.TaskScheduledAt).HasColumnName("task_scheduled_at");
            entity.Property(e => e.TaskStatus).HasColumnName("task_status");
            entity.Property(e => e.Title).HasColumnName("title");

            entity.HasOne(d => d.AssignedToWorker).WithMany(p => p.RecommendationTaskAssignedToWorkers)
                .HasForeignKey(d => d.AssignedToWorkerId)
                .HasConstraintName("recommendation_tasks_assigned_to_worker_id_fkey");

            entity.HasOne(d => d.CreatedByOwner).WithMany(p => p.RecommendationTaskCreatedByOwners)
                .HasForeignKey(d => d.CreatedByOwnerId)
                .HasConstraintName("recommendation_tasks_created_by_owner_id_fkey");
        });

        modelBuilder.Entity<RecommendationTaskDetail>(entity =>
        {
            entity.HasKey(e => e.TaskDetailId).HasName("recommendation_task_detail_pkey");

            entity.ToTable("recommendation_task_detail");

            entity.Property(e => e.TaskDetailId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("task_detail_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Unit).HasColumnName("unit");
            entity.Property(e => e.WorkerId).HasColumnName("worker_id");

            entity.HasOne(d => d.Task).WithMany(p => p.RecommendationTaskDetails)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("recommendation_task_detail_task_id_fkey");

            entity.HasOne(d => d.Worker).WithMany(p => p.RecommendationTaskDetails)
                .HasForeignKey(d => d.WorkerId)
                .HasConstraintName("recommendation_task_detail_worker_id_fkey");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("report_pkey");

            entity.ToTable("report");

            entity.Property(e => e.ReportId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("report_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.SubmitDate).HasColumnName("submit_date");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.WorkerId).HasColumnName("worker_id");

            entity.HasOne(d => d.Worker).WithMany(p => p.Reports)
                .HasForeignKey(d => d.WorkerId)
                .HasConstraintName("report_worker_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("role_pkey");

            entity.ToTable("role");

            entity.Property(e => e.RoleId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("role_id");
            entity.Property(e => e.RoleName).HasColumnName("role_name");
        });

        modelBuilder.Entity<Season>(entity =>
        {
            entity.HasKey(e => e.SeasonId).HasName("seasons_pkey");

            entity.ToTable("seasons");

            entity.Property(e => e.SeasonId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("season_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.FarmId).HasColumnName("farm_id");
            entity.Property(e => e.SeasonCreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("season_created_at");
            entity.Property(e => e.SeasonEndDate).HasColumnName("season_end_date");
            entity.Property(e => e.SeasonName).HasColumnName("season_name");
            entity.Property(e => e.SeasonNotes).HasColumnName("season_notes");
            entity.Property(e => e.SeasonStartDate).HasColumnName("season_start_date");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Farm).WithMany(p => p.Seasons)
                .HasForeignKey(d => d.FarmId)
                .HasConstraintName("seasons_farm_id_fkey");
        });

        modelBuilder.Entity<SeasonsDetail>(entity =>
        {
            entity.HasKey(e => e.SeasonDetailId).HasName("seasons_detail_pkey");

            entity.ToTable("seasons_detail");

            entity.Property(e => e.SeasonDetailId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("season_detail_id");
            entity.Property(e => e.BedId).HasColumnName("bed_id");
            entity.Property(e => e.CropId).HasColumnName("crop_id");
            entity.Property(e => e.CropQuantity).HasColumnName("crop_quantity");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.SeasonExpectedHarvestDate).HasColumnName("season_expected_harvest_date");
            entity.Property(e => e.SeasonId).HasColumnName("season_id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.TotalHarvestYield).HasColumnName("total_harvest_yield");

            entity.HasOne(d => d.Bed).WithMany(p => p.SeasonsDetails)
                .HasForeignKey(d => d.BedId)
                .HasConstraintName("seasons_detail_bed_id_fkey");

            entity.HasOne(d => d.Crop).WithMany(p => p.SeasonsDetails)
                .HasForeignKey(d => d.CropId)
                .HasConstraintName("seasons_detail_crop_id_fkey");

            entity.HasOne(d => d.Season).WithMany(p => p.SeasonsDetails)
                .HasForeignKey(d => d.SeasonId)
                .HasConstraintName("seasons_detail_season_id_fkey");
        });

        modelBuilder.Entity<Soil>(entity =>
        {
            entity.HasKey(e => e.SoilId).HasName("soil_pkey");

            entity.ToTable("soil");

            entity.Property(e => e.SoilId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("soil_id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.ScienceName).HasColumnName("science_name");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("tasks_pkey");

            entity.ToTable("tasks");

            entity.Property(e => e.TaskId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("task_id");

            entity.Property(e => e.TaskTitle).HasColumnName("task_title");
            entity.Property(e => e.TaskScheduledAt).HasColumnName("task_scheduled_at");
            entity.Property(e => e.TaskStatus).HasColumnName("task_status");
            entity.Property(e => e.TaskNotes).HasColumnName("task_notes");

            entity.Property(e => e.TaskCreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("task_created_at");

        });

        modelBuilder.Entity<TaskDetail>(entity =>
        {
            entity.HasKey(e => e.TaskDetailId).HasName("task_detail_pkey");

            entity.ToTable("task_detail");

            entity.Property(e => e.TaskDetailId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("task_detail_id");

            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.SeasonId).HasColumnName("season_id");

            entity.Property(e => e.AssignedToWorkerIds)
                .HasColumnName("assigned_to_worker_ids")
                .HasColumnType("uuid[]");

            entity.Property(e => e.PlotIds)
                .HasColumnName("plot_ids")
                .HasColumnType("uuid[]");

            entity.Property(e => e.BedIds)
                .HasColumnName("bed_ids")
                .HasColumnType("uuid[]");

            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.Notes).HasColumnName("notes");

            entity.HasOne(d => d.Task)
                .WithMany(p => p.TaskDetails)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("task_detail_task_id_fkey");

            entity.HasOne(d => d.Season)
                .WithMany(p => p.TaskDetails)
                .HasForeignKey(d => d.SeasonId)
                .HasConstraintName("task_detail_season_id_fkey");

        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Fullname).HasColumnName("fullname");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("users_role_id_fkey");
        });

        modelBuilder.Entity<WorkerSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("worker_schedule_pkey");

            entity.ToTable("worker_schedule");

            entity.Property(e => e.ScheduleId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("schedule_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TaskDetailId).HasColumnName("task_detail_id");
            entity.Property(e => e.WorkerId).HasColumnName("worker_id");

            entity.HasOne(d => d.TaskDetail).WithMany(p => p.WorkerSchedules)
                .HasForeignKey(d => d.TaskDetailId)
                .HasConstraintName("worker_schedule_task_detail_id_fkey");

            entity.HasOne(d => d.Worker).WithMany(p => p.WorkerSchedules)
                .HasForeignKey(d => d.WorkerId)
                .HasConstraintName("worker_schedule_worker_id_fkey");
        });

        modelBuilder.Entity<IotDevice>(entity =>
        {
            entity.ToTable("iot_devices");

            entity.HasKey(e => e.DeviceId);
            entity.Property(e => e.DeviceId).HasColumnName("device_id");
            entity.Property(e => e.BedId).HasColumnName("bed_id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.InstallationDate).HasColumnName("installation_date").HasColumnType("date");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");

            entity.HasOne(d => d.Bed)
                  .WithMany(p => p.IotDevices)
                  .HasForeignKey(d => d.BedId)
                  .HasConstraintName("fk_iot_devices_beds");
        });

        modelBuilder.Entity<IotData>(entity =>
        {
            entity.ToTable("iot_data");

            entity.HasKey(e => e.SensorDataId); 
            entity.Property(e => e.SensorDataId).HasColumnName("sensor_data_id");

            entity.Property(e => e.DeviceId).HasColumnName("device_id");
            entity.Property(e => e.SeasonId).HasColumnName("season_id");

            entity.Property(e => e.RecordedAt).HasColumnName("recorded_at").HasColumnType("timestamp with time zone");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.Value).HasColumnName("value");
            entity.Property(e => e.Unit).HasColumnName("unit");
            entity.Property(e => e.IsAlert).HasColumnName("is_alert");
            entity.Property(e => e.Min).HasColumnName("min");
            entity.Property(e => e.Max).HasColumnName("max");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");

            entity.HasOne(d => d.Device)
                  .WithMany(p => p.IotDatas)
                  .HasForeignKey(d => d.DeviceId)
                  .HasConstraintName("fk_iot_data_iot_devices");

            entity.HasOne(d => d.Season)
                  .WithMany()
                  .HasForeignKey(d => d.SeasonId)
                  .HasConstraintName("fk_iot_data_seasons");
        });

        modelBuilder.Entity<SoilCropCompatibility>(entity =>
        {
            entity.ToTable("soil_crop_compatibility"); 
            entity.HasKey(e => e.ComptId);

            entity.HasOne(d => d.Soil)
                .WithMany(p => p.SoilCropCompatibilities)
                .HasForeignKey(d => d.SoilId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Crop)
                .WithMany(p => p.SoilCropCompatibilities)
                .HasForeignKey(d => d.CropId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CropGrowthStage>(entity => {
            entity.ToTable("crop_growth_stages");
            entity.HasKey(e => e.StageId);
            entity.Property(e => e.StageId).HasColumnName("stage_id");

            entity.HasOne(d => d.Crop)
                  .WithMany(p => p.CropGrowthStages)
                  .HasForeignKey(d => d.CropId);
        });

        modelBuilder.Entity<CropGrowthTask>(entity => {
            entity.ToTable("crop_growth_tasks");
            entity.HasKey(e => e.GrowthTaskId);

            entity.HasOne(d => d.CropGrowthStage)
                  .WithMany(p => p.CropGrowthTasks)
                  .HasForeignKey(d => d.StageId);
        });

        modelBuilder.Entity<GrowthTracking>(entity =>
        {
            entity.ToTable("growth_tracking");
            entity.HasKey(e => e.TrackingId);
            entity.Property(e => e.TrackingId).HasColumnName("tracking_id");

            entity.HasOne(d => d.SeasonDetail)
                  .WithMany(p => p.GrowthTrackings)
                  .HasForeignKey(d => d.SeasonDetailId)
                  .OnDelete(DeleteBehavior.Cascade); 

            entity.HasOne(d => d.CropGrowthStage)
                  .WithMany(p => p.GrowthTrackings)
                  .HasForeignKey(d => d.StageId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
