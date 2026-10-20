using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProjectExternalReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "external_reference",
                schema: "projects",
                table: "projects",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "external_reference",
                schema: "projects",
                table: "projects"
            );
        }
    }
}
