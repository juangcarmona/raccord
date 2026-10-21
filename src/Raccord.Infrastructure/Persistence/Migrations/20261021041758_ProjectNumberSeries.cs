using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProjectNumberSeries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "CREATE SEQUENCE projects.project_number_seq AS bigint START WITH 10001 INCREMENT BY 1"
            );
            migrationBuilder.AddColumn<string>(
                name: "project_number",
                schema: "projects",
                table: "projects",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true
            );

            migrationBuilder.Sql(
                """
                WITH numbered_projects AS (
                    SELECT id, (10000 + row_number() OVER (ORDER BY id))::text AS project_number
                    FROM projects.projects
                )
                UPDATE projects.projects AS project
                SET project_number = numbered_projects.project_number
                FROM numbered_projects
                WHERE project.id = numbered_projects.id;
                """
            );
            migrationBuilder.AlterColumn<string>(
                name: "project_number",
                schema: "projects",
                table: "projects",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true
            );
            migrationBuilder.Sql(
                "SELECT setval('projects.project_number_seq', 10000 + (SELECT count(*) FROM projects.projects), true)"
            );

            migrationBuilder.CreateIndex(
                name: "ix_projects_project_number",
                schema: "projects",
                table: "projects",
                column: "project_number",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_projects_project_number",
                schema: "projects",
                table: "projects"
            );

            migrationBuilder.DropColumn(
                name: "project_number",
                schema: "projects",
                table: "projects"
            );
            migrationBuilder.Sql("DROP SEQUENCE projects.project_number_seq");
        }
    }
}
