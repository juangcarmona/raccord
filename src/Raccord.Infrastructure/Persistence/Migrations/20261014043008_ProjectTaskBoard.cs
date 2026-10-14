using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProjectTaskBoard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "progress_percent",
                schema: "projects",
                table: "projects",
                type: "numeric(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m
            );

            migrationBuilder.CreateTable(
                name: "project_task_statuses",
                schema: "workspace",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    position = table.Column<int>(type: "integer", nullable: false),
                    completes_project = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_task_statuses", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "project_tasks",
                schema: "workspace",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: false
                    ),
                    description = table.Column<string>(
                        type: "character varying(2000)",
                        maxLength: 2000,
                        nullable: true
                    ),
                    status_id = table.Column<Guid>(type: "uuid", nullable: false),
                    position = table.Column<int>(type: "integer", nullable: false),
                    assignee_id = table.Column<Guid>(type: "uuid", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_tasks", x => x.id);
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_project_task_statuses_position",
                schema: "workspace",
                table: "project_task_statuses",
                column: "position",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_project_tasks_project_id_status_id_position",
                schema: "workspace",
                table: "project_tasks",
                columns: new[] { "project_id", "status_id", "position" }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "project_task_statuses", schema: "workspace");

            migrationBuilder.DropTable(name: "project_tasks", schema: "workspace");

            migrationBuilder.DropColumn(
                name: "progress_percent",
                schema: "projects",
                table: "projects"
            );
        }
    }
}
