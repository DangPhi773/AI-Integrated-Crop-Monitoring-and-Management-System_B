using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddSoilCropCompatibilityTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE public.tasks DROP CONSTRAINT IF EXISTS \"FK_tasks_seasons_SeasonId\";");
            migrationBuilder.Sql("ALTER TABLE public.tasks DROP CONSTRAINT IF EXISTS \"FK_tasks_users_UserId\";");
            migrationBuilder.Sql("ALTER TABLE public.tasks DROP CONSTRAINT IF EXISTS \"tasks_season_id_fkey\";");
            migrationBuilder.Sql("ALTER TABLE public.tasks DROP CONSTRAINT IF EXISTS \"tasks_assigned_to_worker_id_fkey\";");
            migrationBuilder.Sql("DROP INDEX IF EXISTS public.\"IX_tasks_SeasonId\";");
            migrationBuilder.Sql("DROP INDEX IF EXISTS public.\"IX_tasks_UserId\";");
            migrationBuilder.Sql("ALTER TABLE public.tasks DROP COLUMN IF EXISTS \"SeasonId\";");
            migrationBuilder.Sql("ALTER TABLE public.tasks DROP COLUMN IF EXISTS \"UserId\";");

            migrationBuilder.CreateTable(
                name: "soil_crop_compatibility",
                schema: "public",
                columns: table => new
                {
                    ComptId = table.Column<Guid>(type: "uuid", nullable: false),
                    SoilId = table.Column<Guid>(type: "uuid", nullable: false),
                    CropId = table.Column<Guid>(type: "uuid", nullable: false),
                    Compatibility = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_soil_crop_compatibility", x => x.ComptId);
                    table.ForeignKey(
                        name: "FK_soil_crop_compatibility_crops_CropId",
                        column: x => x.CropId,
                        principalSchema: "public",
                        principalTable: "crops",
                        principalColumn: "crop_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_soil_crop_compatibility_soil_SoilId",
                        column: x => x.SoilId,
                        principalSchema: "public",
                        principalTable: "soil",
                        principalColumn: "soil_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql(@"
                INSERT INTO public.soil_crop_compatibility ( ""ComptId"", ""SoilId"", ""CropId"", ""Compatibility"", ""Note"" )
                SELECT gen_random_uuid(), soil_id, crop_id, 'High', 'Migrated from old schema'
                FROM public.crops
                WHERE soil_id IS NOT NULL;
            ");

            migrationBuilder.Sql("ALTER TABLE public.crops DROP COLUMN IF EXISTS soil_id CASCADE;");

            migrationBuilder.CreateIndex(
                name: "IX_soil_crop_compatibility_CropId",
                schema: "public",
                table: "soil_crop_compatibility",
                column: "CropId");

            migrationBuilder.CreateIndex(
                name: "IX_soil_crop_compatibility_SoilId",
                schema: "public",
                table: "soil_crop_compatibility",
                column: "SoilId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_crops_soil_soil_id",
                schema: "public",
                table: "crops");

            migrationBuilder.DropTable(
                name: "soil_crop_compatibility",
                schema: "public");

            migrationBuilder.AddColumn<Guid>(
                name: "SeasonId",
                schema: "public",
                table: "tasks",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "public",
                table: "tasks",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tasks_SeasonId",
                schema: "public",
                table: "tasks",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_UserId",
                schema: "public",
                table: "tasks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "crops_soil_id_fkey",
                schema: "public",
                table: "crops",
                column: "soil_id",
                principalSchema: "public",
                principalTable: "soil",
                principalColumn: "soil_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_seasons_SeasonId",
                schema: "public",
                table: "tasks",
                column: "SeasonId",
                principalSchema: "public",
                principalTable: "seasons",
                principalColumn: "season_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_users_UserId",
                schema: "public",
                table: "tasks",
                column: "UserId",
                principalSchema: "public",
                principalTable: "users",
                principalColumn: "user_id");
        }
    }
}
