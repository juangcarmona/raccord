using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Raccord.Infrastructure.Persistence;
using Raccord.Web.Tests.Infrastructure;

namespace Raccord.Web.Tests;

public sealed class ContactsMigrationTests(PostgresContainer postgres)
{
    private const string PreviousMigration = "20261015190632_CrewHoursCurrent";

#pragma warning disable MA0051 // Upgrade fixture setup and all preserved-reference assertions form one migration proof.
    [Fact]
    public async Task ContactsMigration_Should_PreserveExistingCustomerIdsAndContinueNumberSeries()
    {
        using var api = await RaccordApi.StartAtMigrationAsync(postgres, PreviousMigration);
        var contactId = Guid.Parse("0199f500-0000-7000-8000-000000000001");
        var projectId = Guid.Parse("0199f500-0000-7000-8000-000000000002");
        var invoiceId = Guid.Parse("0199f500-0000-7000-8000-000000000003");
        var draftId = Guid.Parse("0199f500-0000-7000-8000-000000000004");
        await api.ExecuteAsync(
            $"INSERT INTO contacts.customers (id, legal_name) VALUES ('{contactId}', 'Legacy Studio S.A.')"
        );
        await api.ExecuteAsync(
            $"INSERT INTO projects.projects (id, customer_id, name, start_date, end_date, progress_percent, deposit_status) VALUES ('{projectId}', '{contactId}', 'Legacy project', DATE '2026-10-01', DATE '2026-10-02', 0, 'None')"
        );
        await api.ExecuteAsync(
            $"INSERT INTO invoicing.issued_invoices (id, series_code, year, sequence, number, issue_date, issued_at, draft_id, project_id, customer_id, recipient_name, recipient_tax_id, total, currency, representation, content_hash) VALUES ('{invoiceId}', 'F2026', 2026, 1, 'F2026-00001', DATE '2026-10-20', TIMESTAMPTZ '2026-10-20 10:00:00+00', '{draftId}', '{projectId}', '{contactId}', 'Legacy Studio at issue', 'A12345678', 0, 'EUR', '{{\"issue\":\"legacy\"}}', repeat('0', 64))"
        );

        await using (var scope = api.Services.CreateAsyncScope())
        {
            await scope
                .ServiceProvider.GetRequiredService<DatabaseMaintenance>()
                .MigrateAsync(CancellationToken.None);
        }

        (
            await api.QueryScalarAsync<Guid>(
                $"SELECT id FROM contacts.customers WHERE id = '{contactId}'"
            )
        ).ShouldBe(contactId);
        (
            await api.QueryScalarAsync<Guid>(
                $"SELECT customer_id FROM invoicing.issued_invoices WHERE id = '{invoiceId}'"
            )
        ).ShouldBe(contactId);
        (
            await api.QueryScalarAsync<Guid>(
                $"SELECT project_id FROM invoicing.issued_invoices WHERE id = '{invoiceId}'"
            )
        ).ShouldBe(projectId);
        (
            await api.QueryScalarAsync<string>(
                $"SELECT recipient_name FROM invoicing.issued_invoices WHERE id = '{invoiceId}'"
            )
        ).ShouldBe("Legacy Studio at issue");
        (
            await api.QueryScalarAsync<string>(
                $"SELECT type FROM contacts.customers WHERE id = '{contactId}'"
            )
        ).ShouldBe("Company");
        (
            await api.QueryScalarAsync<string>(
                $"SELECT role FROM contacts.contact_roles WHERE contact_id = '{contactId}'"
            )
        ).ShouldBe("Customer");
        (
            await api.QueryScalarAsync<string>(
                $"SELECT contact_number FROM contacts.customers WHERE id = '{contactId}'"
            )
        ).ShouldBe("10001");
        (
            await api.QueryScalarAsync<Guid>(
                $"SELECT customer_id FROM projects.projects WHERE id = '{projectId}'"
            )
        ).ShouldBe(contactId);
        (
            await api.QueryScalarAsync<long>("SELECT nextval('contacts.contact_number_seq')")
        ).ShouldBe(10002);
    }
#pragma warning restore MA0051
}
