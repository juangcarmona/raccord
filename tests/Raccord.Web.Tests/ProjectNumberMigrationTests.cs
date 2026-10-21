using System.Globalization;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Raccord.Contracts.Customers;
using Raccord.Web.Tests.Infrastructure;

namespace Raccord.Web.Tests;

public sealed class ProjectNumberMigrationTests(PostgresContainer postgres)
{
    [Fact]
    public async Task Migration_Should_BackfillByProjectIdAndInitializeNextNumber()
    {
        await using var api = await RaccordApi.StartAtMigrationAsync(
            postgres,
            "20261020235421_ProjectContactPerson"
        );
        using var client = api.CreateClient();
        var customer = await (
            await client.PostAsJsonAsync(
                "/api/customers",
                new RegisterCustomerRequest(
                    "Legacy project customer",
                    "A12345678",
                    RaccordApi.Madrid,
                    null
                )
            )
        )
            .EnsureSuccessStatusCode()
            .Content.ReadFromJsonAsync<CustomerResponse>();

        var ids = new[]
        {
            Guid.Parse("019b0000-0000-7000-8000-000000000003"),
            Guid.Parse("019b0000-0000-7000-8000-000000000001"),
            Guid.Parse("019b0000-0000-7000-8000-000000000002"),
        };
        await InsertLegacyProjectsAsync(api.ConnectionString, ids, customer!.Id);
        await ApplyPendingMigrationsAsync(api);
        await AssertBackfillAsync(api.ConnectionString, ids);
    }

    private static async Task InsertLegacyProjectsAsync(
        string connectionString,
        IReadOnlyList<Guid> ids,
        Guid customerId
    )
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        foreach (var id in ids)
        {
            await using var insert = new NpgsqlCommand(
                """
                INSERT INTO projects.projects (id, name, customer_id, start_date, end_date)
                VALUES (@id, 'Legacy project', @customer_id, DATE '2026-05-04', DATE '2026-05-08')
                """,
                connection
            );
            insert.Parameters.AddWithValue("id", id);
            insert.Parameters.AddWithValue("customer_id", customerId);
            await insert.ExecuteNonQueryAsync();
        }
    }

    private static async Task ApplyPendingMigrationsAsync(RaccordApi api)
    {
        await using var scope = api.Services.CreateAsyncScope();
        await scope
            .ServiceProvider.GetRequiredService<Raccord.Infrastructure.Persistence.DatabaseMaintenance>()
            .MigrateAsync(CancellationToken.None);
    }

    private static async Task AssertBackfillAsync(string connectionString, IReadOnlyList<Guid> ids)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        foreach (var (id, number) in ids.Order().Select((id, index) => (id, index + 10001)))
        {
            await using var select = new NpgsqlCommand(
                "SELECT project_number FROM projects.projects WHERE id = @id",
                connection
            );
            select.Parameters.AddWithValue("id", id);
            (await select.ExecuteScalarAsync()).ShouldBe(
                number.ToString(CultureInfo.InvariantCulture)
            );
        }

        await using var next = new NpgsqlCommand(
            "SELECT nextval('projects.project_number_seq')",
            connection
        );
        (await next.ExecuteScalarAsync()).ShouldBe(10004L);
    }
}
