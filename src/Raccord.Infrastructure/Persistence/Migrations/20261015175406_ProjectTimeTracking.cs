using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProjectTimeTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "project_time_entries",
                schema: "projects",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    task_id = table.Column<Guid>(type: "uuid", nullable: true),
                    service = table.Column<string>(
                        type: "character varying(300)",
                        maxLength: 300,
                        nullable: false
                    ),
                    planned_hours = table.Column<decimal>(
                        type: "numeric(10,2)",
                        precision: 10,
                        scale: 2,
                        nullable: false
                    ),
                    actual_hours = table.Column<decimal>(
                        type: "numeric(10,2)",
                        precision: 10,
                        scale: 2,
                        nullable: false
                    ),
                    actor = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: false
                    ),
                    recorded_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_time_entries", x => x.id);
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_project_time_entries_project_id_recorded_at",
                schema: "projects",
                table: "project_time_entries",
                columns: new[] { "project_id", "recorded_at" }
            );

            migrationBuilder.CreateIndex(
                name: "ix_project_time_entries_task_id",
                schema: "projects",
                table: "project_time_entries",
                column: "task_id"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "project_time_entries", schema: "projects");
        }
    }
}
