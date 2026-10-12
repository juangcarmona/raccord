using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProjectsSchemaMove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "projects");

            migrationBuilder.RenameTable(
                name: "projects",
                newName: "projects",
                newSchema: "projects"
            );

            migrationBuilder.RenameTable(
                name: "billable_lines",
                newName: "billable_lines",
                newSchema: "projects"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(name: "projects", schema: "projects", newName: "projects");

            migrationBuilder.RenameTable(
                name: "billable_lines",
                schema: "projects",
                newName: "billable_lines"
            );
        }
    }
}
