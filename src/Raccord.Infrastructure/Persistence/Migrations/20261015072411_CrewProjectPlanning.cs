using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CrewProjectPlanning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "project_functions",
                schema: "crew",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: false
                    ),
                    kind = table.Column<string>(
                        type: "character varying(20)",
                        maxLength: 20,
                        nullable: false
                    ),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    required_resources = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_functions", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "project_crew_assignments",
                schema: "crew",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_function_row_id = table.Column<Guid>(type: "uuid", nullable: false),
                    crew_member_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rate_id = table.Column<Guid>(type: "uuid", nullable: true),
                    include_travel = table.Column<bool>(type: "boolean", nullable: false),
                    is_project_leader = table.Column<bool>(type: "boolean", nullable: false),
                    remark = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_crew_assignments", x => x.id);
                    table.ForeignKey(
                        name: "fk_project_crew_assignments_project_functions_project_function~",
                        column: x => x.project_function_row_id,
                        principalSchema: "crew",
                        principalTable: "project_functions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "project_shifts",
                schema: "crew",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_function_row_id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    start_time = table.Column<TimeOnly>(
                        type: "time without time zone",
                        nullable: false
                    ),
                    end_time = table.Column<TimeOnly>(
                        type: "time without time zone",
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_shifts", x => x.id);
                    table.ForeignKey(
                        name: "fk_project_shifts_project_functions_project_function_row_id",
                        column: x => x.project_function_row_id,
                        principalSchema: "crew",
                        principalTable: "project_functions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "project_vehicle_assignments",
                schema: "crew",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_function_row_id = table.Column<Guid>(type: "uuid", nullable: false),
                    vehicle_id = table.Column<Guid>(type: "uuid", nullable: false),
                    remark = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_vehicle_assignments", x => x.id);
                    table.ForeignKey(
                        name: "fk_project_vehicle_assignments_project_functions_project_funct~",
                        column: x => x.project_function_row_id,
                        principalSchema: "crew",
                        principalTable: "project_functions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_project_crew_assignments_project_function_row_id_crew_membe~",
                schema: "crew",
                table: "project_crew_assignments",
                columns: new[] { "project_function_row_id", "crew_member_id" },
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_project_functions_project_id_name",
                schema: "crew",
                table: "project_functions",
                columns: new[] { "project_id", "name" },
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_project_shifts_project_function_row_id_date",
                schema: "crew",
                table: "project_shifts",
                columns: new[] { "project_function_row_id", "date" }
            );

            migrationBuilder.CreateIndex(
                name: "ix_project_vehicle_assignments_project_function_row_id_vehicle~",
                schema: "crew",
                table: "project_vehicle_assignments",
                columns: new[] { "project_function_row_id", "vehicle_id" },
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "project_crew_assignments", schema: "crew");

            migrationBuilder.DropTable(name: "project_shifts", schema: "crew");

            migrationBuilder.DropTable(name: "project_vehicle_assignments", schema: "crew");

            migrationBuilder.DropTable(name: "project_functions", schema: "crew");
        }
    }
}
