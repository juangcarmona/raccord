using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DocumentTemplateLanguages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_document_templates_document_type_name",
                schema: "workspace",
                table: "document_templates"
            );

            migrationBuilder.AddColumn<string>(
                name: "language",
                schema: "workspace",
                table: "document_templates",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "en"
            );

            migrationBuilder.CreateIndex(
                name: "ix_document_templates_document_type_language_name",
                schema: "workspace",
                table: "document_templates",
                columns: new[] { "document_type", "language", "name" },
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_document_templates_document_type_language_name",
                schema: "workspace",
                table: "document_templates"
            );

            migrationBuilder.DropColumn(
                name: "language",
                schema: "workspace",
                table: "document_templates"
            );

            migrationBuilder.CreateIndex(
                name: "ix_document_templates_document_type_name",
                schema: "workspace",
                table: "document_templates",
                columns: new[] { "document_type", "name" },
                unique: true
            );
        }
    }
}
