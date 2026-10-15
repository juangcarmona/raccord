using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CrewAvailabilityCurrent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "crew_availability",
                schema: "crew",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    crew_member_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    kind = table.Column<string>(
                        type: "character varying(20)",
                        maxLength: 20,
                        nullable: false
                    ),
                    remark = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_crew_availability", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "crew_invitations",
                schema: "crew",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    function_id = table.Column<Guid>(type: "uuid", nullable: false),
                    crew_member_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(
                        type: "character varying(20)",
                        maxLength: 20,
                        nullable: false
                    ),
                    remark = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_crew_invitations", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "crew_jobs",
                schema: "crew",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    function_id = table.Column<Guid>(type: "uuid", nullable: false),
                    brief = table.Column<string>(
                        type: "character varying(1000)",
                        maxLength: 1000,
                        nullable: true
                    ),
                    posted_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_crew_jobs", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "crew_signups",
                schema: "crew",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    job_id = table.Column<Guid>(type: "uuid", nullable: false),
                    crew_member_id = table.Column<Guid>(type: "uuid", nullable: false),
                    signed_up_at = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_crew_signups", x => x.id);
                    table.ForeignKey(
                        name: "fk_crew_signups_crew_jobs_job_id",
                        column: x => x.job_id,
                        principalSchema: "crew",
                        principalTable: "crew_jobs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_crew_invitations_function_id_crew_member_id",
                schema: "crew",
                table: "crew_invitations",
                columns: new[] { "function_id", "crew_member_id" },
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_crew_signups_job_id_crew_member_id",
                schema: "crew",
                table: "crew_signups",
                columns: new[] { "job_id", "crew_member_id" },
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "crew_availability", schema: "crew");

            migrationBuilder.DropTable(name: "crew_invitations", schema: "crew");

            migrationBuilder.DropTable(name: "crew_signups", schema: "crew");

            migrationBuilder.DropTable(name: "crew_jobs", schema: "crew");
        }
    }
}
