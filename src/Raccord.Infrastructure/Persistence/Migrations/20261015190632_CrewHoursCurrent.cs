using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CrewHoursCurrent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "crew_hours",
                schema: "crew",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    crew_member_id = table.Column<Guid>(type: "uuid", nullable: false),
                    work_date = table.Column<DateOnly>(type: "date", nullable: false),
                    hours = table.Column<decimal>(
                        type: "numeric(8,2)",
                        precision: 8,
                        scale: 2,
                        nullable: false
                    ),
                    source = table.Column<string>(
                        type: "character varying(30)",
                        maxLength: 30,
                        nullable: false
                    ),
                    function_id = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("pk_crew_hours", x => x.id);
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_crew_hours_crew_member_id_work_date",
                schema: "crew",
                table: "crew_hours",
                columns: new[] { "crew_member_id", "work_date" }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "crew_hours", schema: "crew");
        }
    }
}
