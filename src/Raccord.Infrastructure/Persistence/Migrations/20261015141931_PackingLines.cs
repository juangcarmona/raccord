using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PackingLines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "packing_lines",
                schema: "projects",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    equipment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    is_packed = table.Column<bool>(type: "boolean", nullable: false),
                    is_booked_out = table.Column<bool>(type: "boolean", nullable: false),
                    is_received = table.Column<bool>(type: "boolean", nullable: false),
                    packed_by = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: true
                    ),
                    packed_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    booked_out_by = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: true
                    ),
                    booked_out_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    received_by = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: true
                    ),
                    received_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_packing_lines", x => x.id);
                    table.ForeignKey(
                        name: "fk_packing_lines_projects_project_id",
                        column: x => x.project_id,
                        principalSchema: "projects",
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_packing_lines_project_id",
                schema: "projects",
                table: "packing_lines",
                column: "project_id"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "packing_lines", schema: "projects");
        }
    }
}
