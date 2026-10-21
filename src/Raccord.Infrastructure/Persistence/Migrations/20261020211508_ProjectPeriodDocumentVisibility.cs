using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProjectPeriodDocumentVisibility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "show_on_external_documents",
                schema: "projects",
                table: "project_time_periods",
                type: "boolean",
                nullable: false,
                defaultValue: false
            );

            migrationBuilder.AddColumn<bool>(
                name: "show_on_internal_documents",
                schema: "projects",
                table: "project_time_periods",
                type: "boolean",
                nullable: false,
                defaultValue: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "show_on_external_documents",
                schema: "projects",
                table: "project_time_periods"
            );

            migrationBuilder.DropColumn(
                name: "show_on_internal_documents",
                schema: "projects",
                table: "project_time_periods"
            );
        }
    }
}
