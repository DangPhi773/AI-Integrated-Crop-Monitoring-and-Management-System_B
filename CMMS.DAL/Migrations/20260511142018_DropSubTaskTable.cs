using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DropSubTaskTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sub_tasks",
                schema: "public");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sub_tasks",
                schema: "public",
                columns: table => new
                {
                    sub_task_id = table.Column<Guid>(type: "uuid", nullable: false),
                    task_detail_id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sub_tasks", x => x.sub_task_id);
                    table.ForeignKey(
                        name: "FK_sub_tasks_task_detail_task_detail_id",
                        column: x => x.task_detail_id,
                        principalSchema: "public",
                        principalTable: "task_detail",
                        principalColumn: "task_detail_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_sub_tasks_task_detail_id",
                schema: "public",
                table: "sub_tasks",
                column: "task_detail_id");
        }
    }
}
