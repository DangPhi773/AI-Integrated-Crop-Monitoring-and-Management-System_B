using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddGrowthTrackingWithSeasonDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "crop_growth_stages",
                schema: "public",
                columns: table => new
                {
                    stage_id = table.Column<Guid>(type: "uuid", nullable: false),
                    CropId = table.Column<Guid>(type: "uuid", nullable: false),
                    StageName = table.Column<string>(type: "text", nullable: false),
                    StageDescription = table.Column<string>(type: "text", nullable: true),
                    TemperatureMin = table.Column<double>(type: "double precision", nullable: true),
                    HumidityMin = table.Column<double>(type: "double precision", nullable: true),
                    SoilMoistureMin = table.Column<double>(type: "double precision", nullable: true),
                    GrowthIndicators = table.Column<string>(type: "text", nullable: true),
                    CommonDiseases = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_crop_growth_stages", x => x.stage_id);
                    table.ForeignKey(
                        name: "FK_crop_growth_stages_crops_CropId",
                        column: x => x.CropId,
                        principalSchema: "public",
                        principalTable: "crops",
                        principalColumn: "crop_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "crop_growth_tasks",
                schema: "public",
                columns: table => new
                {
                    GrowthTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    StageId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskName = table.Column<string>(type: "text", nullable: false),
                    TaskDescription = table.Column<string>(type: "text", nullable: true),
                    Frequency = table.Column<string>(type: "text", nullable: true),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: true),
                    RequiredTools = table.Column<string>(type: "text", nullable: true),
                    RequiredMaterials = table.Column<string>(type: "text", nullable: true),
                    QuantityPerUnit = table.Column<double>(type: "double precision", nullable: true),
                    QuantityUnit = table.Column<string>(type: "text", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    IsMandatory = table.Column<bool>(type: "boolean", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_crop_growth_tasks", x => x.GrowthTaskId);
                    table.ForeignKey(
                        name: "FK_crop_growth_tasks_crop_growth_stages_StageId",
                        column: x => x.StageId,
                        principalSchema: "public",
                        principalTable: "crop_growth_stages",
                        principalColumn: "stage_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "growth_tracking",
                schema: "public",
                columns: table => new
                {
                    tracking_id = table.Column<Guid>(type: "uuid", nullable: false),
                    SeasonDetailId = table.Column<Guid>(type: "uuid", nullable: false),
                    StageId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TrackingStatus = table.Column<string>(type: "text", nullable: false),
                    HealthStatus = table.Column<string>(type: "text", nullable: true),
                    ActualHeight = table.Column<double>(type: "double precision", nullable: true),
                    ActualYield = table.Column<double>(type: "double precision", nullable: true),
                    DelayDays = table.Column<int>(type: "integer", nullable: true),
                    DelayReason = table.Column<string>(type: "text", nullable: true),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LastObservedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_growth_tracking", x => x.tracking_id);
                    table.ForeignKey(
                        name: "FK_growth_tracking_crop_growth_stages_StageId",
                        column: x => x.StageId,
                        principalSchema: "public",
                        principalTable: "crop_growth_stages",
                        principalColumn: "stage_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_growth_tracking_seasons_detail_SeasonDetailId",
                        column: x => x.SeasonDetailId,
                        principalSchema: "public",
                        principalTable: "seasons_detail",
                        principalColumn: "season_detail_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_crop_growth_stages_CropId",
                schema: "public",
                table: "crop_growth_stages",
                column: "CropId");

            migrationBuilder.CreateIndex(
                name: "IX_crop_growth_tasks_StageId",
                schema: "public",
                table: "crop_growth_tasks",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_growth_tracking_SeasonDetailId",
                schema: "public",
                table: "growth_tracking",
                column: "SeasonDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_growth_tracking_StageId",
                schema: "public",
                table: "growth_tracking",
                column: "StageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "crop_growth_tasks",
                schema: "public");

            migrationBuilder.DropTable(
                name: "growth_tracking",
                schema: "public");

            migrationBuilder.DropTable(
                name: "crop_growth_stages",
                schema: "public");
        }
    }
}
