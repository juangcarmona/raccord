using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProjectCoordination : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "project_type_id",
                schema: "projects",
                table: "projects",
                type: "uuid",
                nullable: true
            );

            migrationBuilder.AddColumn<Guid>(
                name: "team_id",
                schema: "projects",
                table: "projects",
                type: "uuid",
                nullable: true
            );

            migrationBuilder.CreateTable(
                name: "project_requests",
                schema: "workspace",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: false
                    ),
                    requester_name = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: false
                    ),
                    requester_email = table.Column<string>(
                        type: "character varying(320)",
                        maxLength: 320,
                        nullable: false
                    ),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: true),
                    description = table.Column<string>(
                        type: "character varying(2000)",
                        maxLength: 2000,
                        nullable: true
                    ),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    project_type_id = table.Column<Guid>(type: "uuid", nullable: true),
                    team_id = table.Column<Guid>(type: "uuid", nullable: true),
                    status = table.Column<string>(
                        type: "character varying(20)",
                        maxLength: 20,
                        nullable: false
                    ),
                    project_id = table.Column<Guid>(type: "uuid", nullable: true),
                    decline_reason = table.Column<string>(
                        type: "character varying(1000)",
                        maxLength: 1000,
                        nullable: true
                    ),
                    received_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    resolved_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_requests", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "project_teams",
                schema: "workspace",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    description = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_teams", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "project_types",
                schema: "workspace",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    description = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_types", x => x.id);
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_project_requests_status",
                schema: "workspace",
                table: "project_requests",
                column: "status"
            );

            migrationBuilder.CreateIndex(
                name: "ix_project_teams_name",
                schema: "workspace",
                table: "project_teams",
                column: "name",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_project_types_name",
                schema: "workspace",
                table: "project_types",
                column: "name",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "project_requests", schema: "workspace");

            migrationBuilder.DropTable(name: "project_teams", schema: "workspace");

            migrationBuilder.DropTable(name: "project_types", schema: "workspace");

            migrationBuilder.DropColumn(
                name: "project_type_id",
                schema: "projects",
                table: "projects"
            );

            migrationBuilder.DropColumn(name: "team_id", schema: "projects", table: "projects");
        }
    }
}
