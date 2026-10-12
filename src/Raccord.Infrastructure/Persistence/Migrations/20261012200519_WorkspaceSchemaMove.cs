using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class WorkspaceSchemaMove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_invoice_number_sequences_invoice_series_series_code",
                table: "invoice_number_sequences"
            );

            migrationBuilder.EnsureSchema(name: "workspace");

            migrationBuilder.RenameTable(
                name: "issuer_profiles",
                newName: "issuer_profiles",
                newSchema: "workspace"
            );

            migrationBuilder.RenameTable(
                name: "invoice_series",
                newName: "invoice_series",
                newSchema: "workspace"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "issuer_profiles",
                schema: "workspace",
                newName: "issuer_profiles"
            );

            migrationBuilder.RenameTable(
                name: "invoice_series",
                schema: "workspace",
                newName: "invoice_series"
            );

            migrationBuilder.AddForeignKey(
                name: "fk_invoice_number_sequences_invoice_series_series_code",
                table: "invoice_number_sequences",
                column: "series_code",
                principalTable: "invoice_series",
                principalColumn: "code",
                onDelete: ReferentialAction.Restrict
            );
        }
    }
}
