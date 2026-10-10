using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BillableLinePositions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_billable_lines_project_id",
                table: "billable_lines"
            );

            migrationBuilder.AddColumn<int>(
                name: "position",
                table: "billable_lines",
                type: "integer",
                nullable: false,
                defaultValue: 0
            );

            // Existing lines get positions in creation order (UUIDv7 identifiers sort by time) before uniqueness applies.
            migrationBuilder.Sql(
                """
                UPDATE billable_lines AS line
                SET position = ordered.position
                FROM (SELECT id, row_number() OVER (PARTITION BY project_id ORDER BY id) AS position FROM billable_lines) AS ordered
                WHERE line.id = ordered.id;
                """
            );

            migrationBuilder.CreateIndex(
                name: "ix_billable_lines_project_id_position",
                table: "billable_lines",
                columns: new[] { "project_id", "position" },
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_billable_lines_project_id_position",
                table: "billable_lines"
            );

            migrationBuilder.DropColumn(name: "position", table: "billable_lines");

            migrationBuilder.CreateIndex(
                name: "ix_billable_lines_project_id",
                table: "billable_lines",
                column: "project_id"
            );
        }
    }
}
