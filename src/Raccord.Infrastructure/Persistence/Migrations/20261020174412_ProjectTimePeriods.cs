using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProjectTimePeriods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "project_time_periods",
                schema: "projects",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(
                        type: "character varying(120)",
                        maxLength: 120,
                        nullable: false
                    ),
                    position = table.Column<int>(type: "integer", nullable: false),
                    is_default_planning = table.Column<bool>(type: "boolean", nullable: false),
                    is_default_usage = table.Column<bool>(type: "boolean", nullable: false),
                    start = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    end = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_time_periods", x => x.id);
                    table.CheckConstraint(
                        "CK_project_time_periods_complete_range",
                        "(\"start\" IS NULL AND \"end\" IS NULL) OR (\"start\" IS NOT NULL AND \"end\" IS NOT NULL AND \"end\" > \"start\")"
                    );
                    table.ForeignKey(
                        name: "fk_project_time_periods_projects_project_id",
                        column: x => x.project_id,
                        principalSchema: "projects",
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.Sql(
                """
                INSERT INTO projects.project_time_periods
                    (id, name, position, is_default_planning, is_default_usage, start, "end", project_id)
                SELECT gen_random_uuid(), 'Planning', 0, true, false,
                    NULL::timestamp with time zone, NULL::timestamp with time zone, id
                FROM projects.projects
                UNION ALL
                SELECT gen_random_uuid(), 'Usage', 1, false, true,
                    NULL::timestamp with time zone, NULL::timestamp with time zone, id
                FROM projects.projects;
                """
            );

            migrationBuilder.CreateIndex(
                name: "ix_project_time_periods_project_id_is_default_planning",
                schema: "projects",
                table: "project_time_periods",
                columns: new[] { "project_id", "is_default_planning" },
                unique: true,
                filter: "is_default_planning"
            );

            migrationBuilder.CreateIndex(
                name: "ix_project_time_periods_project_id_is_default_usage",
                schema: "projects",
                table: "project_time_periods",
                columns: new[] { "project_id", "is_default_usage" },
                unique: true,
                filter: "is_default_usage"
            );

            migrationBuilder.CreateIndex(
                name: "ix_project_time_periods_project_id_position",
                schema: "projects",
                table: "project_time_periods",
                columns: new[] { "project_id", "position" },
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "project_time_periods", schema: "projects");
        }
    }
}
