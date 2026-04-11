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

    public virtual DbSet<Notification> Notifications { get; set; }

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
    public virtual DbSet<SubTask> SubTasks { get; set; }

    public virtual DbSet<CropBedConfig> CropBedConfigs { get; set; }
    public virtual DbSet<Attachment> Attachments { get; set; }
    public virtual DbSet<ReportAssignment> ReportAssignments { get; set; }
    public virtual DbSet<DiagnosisResult> DiagnosisResults { get; set; }
    public virtual DbSet<IotSensor> IotSensors { get; set; }
    public virtual DbSet<ReportEnvironmentSnapshot> ReportEnvironmentSnapshots { get; set; }

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
            entity.Property(e => e.PlantingPattern).HasColumnName("planting_pattern");
            entity.Property(e => e.RowCount).HasColumnName("row_count");
            entity.Property(e => e.BedWidth).HasColumnName("bed_width");
            entity.Property(e => e.BedLength).HasColumnName("bed_length");

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
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.ReportId).HasColumnName("report_id");
            entity.Property(e => e.DiagnosisId).HasColumnName("diagnosis_id");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("notification_user_id_fkey");

            entity.HasOne(d => d.Report).WithMany()
                .HasForeignKey(d => d.ReportId)
                .HasConstraintName("notification_report_id_fkey");

            entity.HasOne(d => d.Diagnosis).WithMany()
                .HasForeignKey(d => d.DiagnosisId)
                .HasConstraintName("notification_diagnosis_id_fkey");
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
            entity.Property(e => e.PlotLength).HasColumnName("plot_length");
            entity.Property(e => e.PlotWidth).HasColumnName("plot_width");
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
            entity.Property(e => e.DiagnosisId).HasColumnName("diagnosis_id");
            entity.Property(e => e.SeasonId).HasColumnName("season_id");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Diagnosis).WithMany(p => p.Recommendations)
                .HasForeignKey(d => d.DiagnosisId)
                .HasConstraintName("recommendation_diagnosis_id_fkey");

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
            entity.Property(e => e.ReportNo).HasColumnName("report_no");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ReportType).HasColumnName("report_type");
            entity.Property(e => e.PlotId).HasColumnName("plot_id");
            entity.Property(e => e.BedId).HasColumnName("bed_id");
            entity.Property(e => e.SeasonId).HasColumnName("season_id");
            entity.Property(e => e.AiResultsJson).HasColumnName("ai_results_json");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.SubmitDate).HasColumnName("submit_date");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.Creator).WithMany(p => p.CreatedReports)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("report_created_by_fkey");

            entity.HasOne(d => d.Owner).WithMany(p => p.OwnedReports)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("report_owner_id_fkey");

            entity.HasOne(d => d.Plot).WithMany()
                .HasForeignKey(d => d.PlotId)
                .HasConstraintName("report_plot_id_fkey");

            entity.HasOne(d => d.Bed).WithMany()
                .HasForeignKey(d => d.BedId)
                .HasConstraintName("report_bed_id_fkey");

            entity.HasOne(d => d.Season).WithMany()
                .HasForeignKey(d => d.SeasonId)
                .HasConstraintName("report_season_id_fkey");
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
            entity.Property(e => e.DeviceCode).HasColumnName("device_code");
            entity.Property(e => e.LastActiveAt).HasColumnName("last_active_at").HasColumnType("timestamp with time zone");

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
            entity.Property(e => e.SensorId).HasColumnName("sensor_id");
            entity.Property(e => e.RawData).HasColumnName("raw_data");

            entity.HasOne(d => d.Device)
                  .WithMany(p => p.IotDatas)
                  .HasForeignKey(d => d.DeviceId)
                  .HasConstraintName("fk_iot_data_iot_devices");

            entity.HasOne(d => d.Season)
                  .WithMany()
                  .HasForeignKey(d => d.SeasonId)
                  .HasConstraintName("fk_iot_data_seasons");

            entity.HasOne(d => d.Sensor)
                  .WithMany(p => p.IotDatas)
                  .HasForeignKey(d => d.SensorId)
                  .HasConstraintName("iot_data_sensor_id_fkey");
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

        modelBuilder.Entity<CropGrowthTask>(entity =>
        {
            entity.ToTable("crop_growth_tasks");
            entity.HasKey(e => e.GrowthTaskId);
            entity.Property(e => e.GrowthTaskId)
                  .HasColumnName("GrowthTaskId") 
                  .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.StageId)
                  .HasColumnName("StageId"); 

            entity.Property(e => e.TaskName)
                  .HasColumnName("TaskName")
                  .IsRequired();

            entity.Property(e => e.CreatedAt)
                  .HasColumnName("CreatedAt") 
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.TaskDescription).HasColumnName("TaskDescription");
            entity.Property(e => e.Frequency).HasColumnName("Frequency");
            entity.Property(e => e.DurationMinutes).HasColumnName("DurationMinutes");
            entity.Property(e => e.RequiredTools).HasColumnName("RequiredTools");
            entity.Property(e => e.RequiredMaterials).HasColumnName("RequiredMaterials");
            entity.Property(e => e.QuantityPerUnit).HasColumnName("QuantityPerUnit");
            entity.Property(e => e.QuantityUnit).HasColumnName("QuantityUnit");
            entity.Property(e => e.Priority).HasColumnName("Priority");
            entity.Property(e => e.IsMandatory).HasColumnName("IsMandatory");
            entity.Property(e => e.Notes).HasColumnName("Notes");

            entity.HasOne(d => d.CropGrowthStage)
                  .WithMany(p => p.CropGrowthTasks)
                  .HasForeignKey(d => d.StageId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .HasConstraintName("FK_CropGrowthTasks_CropGrowthStages");
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

        modelBuilder.Entity<SubTask>(entity =>
        {
            entity.ToTable("sub_tasks");
            entity.HasKey(e => e.SubTaskId);
            entity.Property(e => e.SubTaskId)
                  .HasColumnName("sub_task_id");

            entity.Property(e => e.Title)
                  .IsRequired()
                  .HasMaxLength(200)
                  .HasColumnName("title");

            entity.Property(e => e.Description)
                  .HasColumnName("description");

            entity.Property(e => e.TaskDetailId)
                  .HasColumnName("task_detail_id");
            entity.HasOne(d => d.TaskDetail)
                  .WithMany(p => p.SubTasks)
                  .HasForeignKey(d => d.TaskDetailId)
                  .OnDelete(DeleteBehavior.Cascade); 
        });

        modelBuilder.Entity<CropBedConfig>(entity =>
        {
            entity.ToTable("crop_bed_config");
            entity.HasKey(e => e.ConfigId).HasName("crop_bed_config_pkey");

            entity.Property(e => e.ConfigId)
                  .HasDefaultValueSql("gen_random_uuid()")
                  .HasColumnName("config_id");
            entity.Property(e => e.CropId).HasColumnName("crop_id");
            entity.Property(e => e.PlantingPattern)
                  .IsRequired()
                  .HasMaxLength(20)
                  .HasColumnName("planting_pattern");
            entity.Property(e => e.RowSpacing).HasColumnName("row_spacing");
            entity.Property(e => e.PlantSpacing).HasColumnName("plant_spacing");
            entity.Property(e => e.RowsPerBed).HasColumnName("rows_per_bed");
            entity.Property(e => e.BedWidthMin).HasColumnName("bed_width_min");
            entity.Property(e => e.BedWidthMax).HasColumnName("bed_width_max");
            entity.Property(e => e.PathWidthMin).HasColumnName("path_width_min");
            entity.Property(e => e.PathWidthMax).HasColumnName("path_width_max");
            entity.Property(e => e.BedHeight).HasColumnName("bed_height");
            entity.Property(e => e.DensityPerHaMin).HasColumnName("density_per_ha_min");
            entity.Property(e => e.DensityPerHaMax).HasColumnName("density_per_ha_max");
            entity.Property(e => e.IsDefault).HasColumnName("is_default").HasDefaultValue(false);
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("now()")
                  .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                  .HasDefaultValueSql("now()")
                  .HasColumnName("updated_at");

            entity.HasIndex(e => new { e.CropId, e.PlantingPattern })
                  .IsUnique()
                  .HasDatabaseName("ux_crop_bed_config_crop_pattern");

            entity.HasIndex(e => e.CropId)
                  .IsUnique()
                  .HasFilter("is_default = true")
                  .HasDatabaseName("ux_crop_bed_config_default_per_crop");

            entity.HasOne(d => d.Crop)
                  .WithMany()
                  .HasForeignKey(d => d.CropId)
                  .HasConstraintName("crop_bed_config_crop_id_fkey")
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.ToTable("attachment");
            entity.HasKey(e => e.Id).HasName("attachment_pkey");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.ObjectType).HasColumnName("object_type");
            entity.Property(e => e.ObjectId).HasColumnName("object_id");
            entity.Property(e => e.AttachmentType).HasColumnName("attachment_type");
            entity.Property(e => e.FileName).HasColumnName("file_name");
            entity.Property(e => e.FileUrl).HasColumnName("file_url");
            entity.Property(e => e.CloudinaryPublicId).HasColumnName("cloudinary_public_id");
            entity.Property(e => e.CloudinarySecureUrl).HasColumnName("cloudinary_secure_url");
            entity.Property(e => e.FileExtension).HasColumnName("file_extension");
            entity.Property(e => e.MimeType).HasColumnName("mime_type");
            entity.Property(e => e.FileSize).HasColumnName("file_size");
            entity.Property(e => e.UploadedBy).HasColumnName("uploaded_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false).HasColumnName("is_deleted");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");

            entity.HasIndex(e => new { e.ObjectType, e.ObjectId })
                  .HasDatabaseName("idx_attachment_object");

            entity.HasOne(d => d.Uploader).WithMany()
                .HasForeignKey(d => d.UploadedBy)
                .HasConstraintName("attachment_uploaded_by_fkey");
        });

        modelBuilder.Entity<ReportAssignment>(entity =>
        {
            entity.ToTable("report_assignment");
            entity.HasKey(e => e.Id).HasName("report_assignment_pkey");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.ReportId).HasColumnName("report_id");
            entity.Property(e => e.AssignedBy).HasColumnName("assigned_by");
            entity.Property(e => e.AssignedTo).HasColumnName("assigned_to");
            entity.Property(e => e.AssignedAt).HasColumnName("assigned_at");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.Report).WithMany(p => p.ReportAssignments)
                .HasForeignKey(d => d.ReportId)
                .HasConstraintName("report_assignment_report_id_fkey");

            entity.HasOne(d => d.Assigner).WithMany()
                .HasForeignKey(d => d.AssignedBy)
                .HasConstraintName("report_assignment_assigned_by_fkey");

            entity.HasOne(d => d.Assignee).WithMany()
                .HasForeignKey(d => d.AssignedTo)
                .HasConstraintName("report_assignment_assigned_to_fkey");
        });

        modelBuilder.Entity<DiagnosisResult>(entity =>
        {
            entity.ToTable("diagnosis_result");
            entity.HasKey(e => e.Id).HasName("diagnosis_result_pkey");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.ReportId).HasColumnName("report_id");
            entity.Property(e => e.DiagnosedBy).HasColumnName("diagnosed_by");
            entity.Property(e => e.DiseaseName).HasColumnName("disease_name");
            entity.Property(e => e.Conclusion).HasColumnName("conclusion");
            entity.Property(e => e.RecommendedAction).HasColumnName("recommended_action");
            entity.Property(e => e.SeverityLevel).HasColumnName("severity_level");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.Report).WithMany(p => p.DiagnosisResults)
                .HasForeignKey(d => d.ReportId)
                .HasConstraintName("diagnosis_result_report_id_fkey");

            entity.HasOne(d => d.Diagnoser).WithMany()
                .HasForeignKey(d => d.DiagnosedBy)
                .HasConstraintName("diagnosis_result_diagnosed_by_fkey");
        });

        modelBuilder.Entity<IotSensor>(entity =>
        {
            entity.ToTable("iot_sensor");
            entity.HasKey(e => e.Id).HasName("iot_sensor_pkey");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.DeviceId).HasColumnName("device_id");
            entity.Property(e => e.SensorCode).HasColumnName("sensor_code");
            entity.Property(e => e.SensorName).HasColumnName("sensor_name");
            entity.Property(e => e.SensorType).HasColumnName("sensor_type");
            entity.Property(e => e.Unit).HasColumnName("unit");
            entity.Property(e => e.MinValue).HasColumnName("min_value");
            entity.Property(e => e.MaxValue).HasColumnName("max_value");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.Device).WithMany(p => p.IotSensors)
                .HasForeignKey(d => d.DeviceId)
                .HasConstraintName("iot_sensor_device_id_fkey");
        });

        modelBuilder.Entity<ReportEnvironmentSnapshot>(entity =>
        {
            entity.ToTable("report_environment_snapshot");
            entity.HasKey(e => e.Id).HasName("report_environment_snapshot_pkey");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.ReportId).HasColumnName("report_id");
            entity.Property(e => e.Temperature).HasColumnName("temperature");
            entity.Property(e => e.Humidity).HasColumnName("humidity");
            entity.Property(e => e.SoilMoisture).HasColumnName("soil_moisture");
            entity.Property(e => e.Rainfall).HasColumnName("rainfall");
            entity.Property(e => e.LightIntensity).HasColumnName("light_intensity");
            entity.Property(e => e.RecordedAt).HasColumnName("recorded_at");
            entity.Property(e => e.SourceDeviceId).HasColumnName("source_device_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");

            entity.HasOne(d => d.Report).WithMany(p => p.EnvironmentSnapshots)
                .HasForeignKey(d => d.ReportId)
                .HasConstraintName("report_env_snapshot_report_id_fkey");

            entity.HasOne(d => d.SourceDevice).WithMany()
                .HasForeignKey(d => d.SourceDeviceId)
                .HasConstraintName("report_env_snapshot_device_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
