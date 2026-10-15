using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CrewRecordsCurrent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "crew");

            migrationBuilder.CreateTable(
                name: "crew_members",
                schema: "crew",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    role = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    email = table.Column<string>(
                        type: "character varying(320)",
                        maxLength: 320,
                        nullable: false
                    ),
                    last_name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: true
                    ),
                    folder = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: true
                    ),
                    skills = table.Column<string>(type: "jsonb", nullable: false),
                    is_plannable = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_crew_members", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "crew_rates",
                schema: "crew",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(
                        type: "character varying(160)",
                        maxLength: 160,
                        nullable: false
                    ),
                    kind = table.Column<string>(
                        type: "character varying(16)",
                        maxLength: 16,
                        nullable: false
                    ),
                    bands = table.Column<string>(type: "jsonb", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_crew_rates", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "vehicles",
                schema: "crew",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    registration_number = table.Column<string>(
                        type: "character varying(32)",
                        maxLength: 32,
                        nullable: false
                    ),
                    seats = table.Column<int>(type: "integer", nullable: false),
                    folder = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: true
                    ),
                    tags = table.Column<string>(type: "jsonb", nullable: false),
                    is_shown_in_planning = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicles", x => x.id);
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_crew_members_email",
                schema: "crew",
                table: "crew_members",
                column: "email",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_crew_rates_name_kind",
                schema: "crew",
                table: "crew_rates",
                columns: new[] { "name", "kind" },
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_vehicles_registration_number",
                schema: "crew",
                table: "vehicles",
                column: "registration_number",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "crew_members", schema: "crew");

            migrationBuilder.DropTable(name: "crew_rates", schema: "crew");

            migrationBuilder.DropTable(name: "vehicles", schema: "crew");
        }
    }
}
