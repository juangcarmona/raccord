using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Raccord.Infrastructure.Persistence;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations;

/// <summary>Moves any remaining workspace-owned tables from the legacy public schema.</summary>
[DbContext(typeof(RaccordDbContext))]
[Migration("20261015091000_MoveRemainingWorkspaceTables")]
public partial class MoveRemainingWorkspaceTables : Migration
{
    private const string WorkspaceTables = """
        DO $$
        DECLARE table_name text;
        BEGIN
            FOREACH table_name IN ARRAY ARRAY[
                'api_access_tokens', 'billable_lines', 'contact_persons',
                'financial_settings', 'invoice_draft_lines', 'invoice_drafts',
                'invoice_number_sequences', 'invoice_payments', 'invoice_reminders',
                'invoice_series', 'issued_invoices', 'issuer_profiles', 'packing_lines',
                'planned_equipment_lines', 'project_templates', 'projects', 'quote_lines',
                'quote_number_sequences', 'quotes'
            ] LOOP
                IF to_regclass('public.' || table_name) IS NOT NULL THEN
                    EXECUTE format('ALTER TABLE public.%I SET SCHEMA workspace', table_name);
                END IF;
            END LOOP;
        END $$;
        """;

    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) =>
        migrationBuilder.Sql(WorkspaceTables);

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) =>
        migrationBuilder.Sql(
            WorkspaceTables
                .Replace("public.' || table_name", "workspace.' || table_name")
                .Replace("public.%I SET SCHEMA workspace", "workspace.%I SET SCHEMA public")
        );
}
