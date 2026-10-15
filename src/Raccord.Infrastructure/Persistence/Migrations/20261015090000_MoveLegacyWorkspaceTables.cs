using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Raccord.Infrastructure.Persistence;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations;

/// <summary>Moves legacy workspace-owned tables into the workspace schema.</summary>
[DbContext(typeof(RaccordDbContext))]
[Migration("20261015090000_MoveLegacyWorkspaceTables")]
public partial class MoveLegacyWorkspaceTables : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(name: "workspace");

        migrationBuilder.RenameTable(
            name: "invoice_series",
            newName: "invoice_series",
            newSchema: "workspace"
        );
        migrationBuilder.RenameTable(
            name: "issued_invoices",
            newName: "issued_invoices",
            newSchema: "workspace"
        );
        migrationBuilder.RenameTable(
            name: "issuer_profiles",
            newName: "issuer_profiles",
            newSchema: "workspace"
        );
        migrationBuilder.RenameTable(name: "projects", newName: "projects", newSchema: "workspace");
        migrationBuilder.RenameTable(
            name: "invoice_number_sequences",
            newName: "invoice_number_sequences",
            newSchema: "workspace"
        );
        migrationBuilder.RenameTable(
            name: "billable_lines",
            newName: "billable_lines",
            newSchema: "workspace"
        );
        migrationBuilder.RenameTable(
            name: "invoice_drafts",
            newName: "invoice_drafts",
            newSchema: "workspace"
        );
        migrationBuilder.RenameTable(
            name: "invoice_draft_lines",
            newName: "invoice_draft_lines",
            newSchema: "workspace"
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameTable(
            name: "invoice_draft_lines",
            schema: "workspace",
            newName: "invoice_draft_lines"
        );
        migrationBuilder.RenameTable(
            name: "invoice_drafts",
            schema: "workspace",
            newName: "invoice_drafts"
        );
        migrationBuilder.RenameTable(
            name: "billable_lines",
            schema: "workspace",
            newName: "billable_lines"
        );
        migrationBuilder.RenameTable(
            name: "invoice_number_sequences",
            schema: "workspace",
            newName: "invoice_number_sequences"
        );
        migrationBuilder.RenameTable(name: "projects", schema: "workspace", newName: "projects");
        migrationBuilder.RenameTable(
            name: "issuer_profiles",
            schema: "workspace",
            newName: "issuer_profiles"
        );
        migrationBuilder.RenameTable(
            name: "issued_invoices",
            schema: "workspace",
            newName: "issued_invoices"
        );
        migrationBuilder.RenameTable(
            name: "invoice_series",
            schema: "workspace",
            newName: "invoice_series"
        );
    }
}
