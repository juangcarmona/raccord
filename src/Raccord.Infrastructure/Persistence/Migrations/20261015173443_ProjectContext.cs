using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProjectContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "project_context_entries",
                schema: "projects",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    kind = table.Column<string>(
                        type: "character varying(30)",
                        maxLength: 30,
                        nullable: false
                    ),
                    name = table.Column<string>(
                        type: "character varying(300)",
                        maxLength: 300,
                        nullable: false
                    ),
                    content = table.Column<string>(type: "text", nullable: true),
                    folder = table.Column<string>(
                        type: "character varying(300)",
                        maxLength: 300,
                        nullable: true
                    ),
                    key = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: true
                    ),
                    value = table.Column<string>(type: "text", nullable: true),
                    target_type = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: true
                    ),
                    target_id = table.Column<Guid>(type: "uuid", nullable: true),
                    actor = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: false
                    ),
                    created_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_context_entries", x => x.id);
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_project_context_entries_project_id_key",
                schema: "projects",
                table: "project_context_entries",
                columns: new[] { "project_id", "key" }
            );

            migrationBuilder.CreateIndex(
                name: "ix_project_context_entries_project_id_kind_created_at",
                schema: "projects",
                table: "project_context_entries",
                columns: new[] { "project_id", "kind", "created_at" }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "project_context_entries", schema: "projects");
        }
    }
}
