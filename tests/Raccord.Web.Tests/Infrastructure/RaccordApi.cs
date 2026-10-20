using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Raccord.Contracts.Common;
using Raccord.Contracts.Customers;
using Raccord.Contracts.Organization;
using Raccord.Contracts.Projects;
using Raccord.Infrastructure.Persistence;

namespace Raccord.Web.Tests.Infrastructure;

/// <summary>The Raccord web host running against its own migrated database and document folder.</summary>
public sealed class RaccordApi : WebApplicationFactory<Program>
{
    private RaccordApi(string connectionString, bool testingAuthBypass = true)
    {
        ConnectionString = connectionString;
        TestingAuthBypass = testingAuthBypass;
        DocumentRoot = Path.Combine(
            Path.GetTempPath(),
            "raccord-web-tests",
            Guid.NewGuid().ToString("N")
        );
    }

    public string ConnectionString { get; }

    private bool TestingAuthBypass { get; }

    public string DocumentRoot { get; }

    public static readonly AddressDto Madrid = new(
        "Calle Mayor 1",
        "28013",
        "Madrid",
        "Madrid",
        "ES"
    );

    public static readonly AddressDto Valencia = new(
        "Avenida del Puerto 20",
        "46023",
        "Valencia",
        "Valencia",
        "ES"
    );

    public static async Task<RaccordApi> StartAsync(PostgresContainer postgres)
    {
        var api = new RaccordApi(await postgres.CreateDatabaseAsync());
        await using var scope = api.Services.CreateAsyncScope();
        await scope
            .ServiceProvider.GetRequiredService<DatabaseMaintenance>()
            .MigrateAsync(CancellationToken.None);
        return api;
    }

    public static async Task<RaccordApi> StartWithoutAuthBypassAsync(PostgresContainer postgres)
    {
        var api = new RaccordApi(await postgres.CreateDatabaseAsync(), testingAuthBypass: false);
        await using var scope = api.Services.CreateAsyncScope();
        await scope
            .ServiceProvider.GetRequiredService<DatabaseMaintenance>()
            .MigrateAsync(CancellationToken.None);
        return api;
    }

    public static async Task<RaccordApi> StartAtMigrationAsync(
        PostgresContainer postgres,
        string migrationId
    )
    {
        var api = new RaccordApi(await postgres.CreateDatabaseAsync());
        await using var scope = api.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<RaccordDbContext>();
        await db.Database.GetService<IMigrator>().MigrateAsync(migrationId);
        return api;
    }

    /// <summary>Defines the issuer, a customer and a project with the given lines; returns the project.</summary>
    public static async Task<ProjectResponse> ArrangeProjectAsync(
        HttpClient client,
        string? customerTaxId = "A00000000",
        params AddBillableLineRequest[] lines
    )
    {
        (
            await client.PutAsJsonAsync(
                "/api/organization/issuer",
                new UpdateIssuerProfileRequest("Fotograma Rental S.L.", "B00000000", Madrid)
            )
        ).EnsureSuccessStatusCode();
        var customer = await (
            await client.PostAsJsonAsync(
                "/api/customers",
                new RegisterCustomerRequest(
                    "Productora Ejemplo S.A.",
                    customerTaxId,
                    Valencia,
                    null
                )
            )
        )
            .EnsureSuccessStatusCode()
            .Content.ReadFromJsonAsync<CustomerResponse>();
        var project = await (
            await client.PostAsJsonAsync(
                "/api/projects",
                new CreateProjectRequest(
                    "Spring campaign shoot",
                    customer!.Id,
                    new DateOnly(2026, 5, 4),
                    new DateOnly(2026, 5, 8)
                )
            )
        )
            .EnsureSuccessStatusCode()
            .Content.ReadFromJsonAsync<ProjectResponse>();
        foreach (var line in lines)
        {
            (
                await client.PostAsJsonAsync($"/api/projects/{project!.Id}/billable-lines", line)
            ).EnsureSuccessStatusCode();
        }

        return (await client.GetFromJsonAsync<ProjectResponse>($"/api/projects/{project!.Id}"))!;
    }

    public async Task<T?> QueryScalarAsync<T>(string sql)
    {
        await using var connection = new NpgsqlConnection(ConnectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(sql, connection);
        return (T?)await command.ExecuteScalarAsync();
    }

    public async Task ExecuteAsync(string sql)
    {
        await using var connection = new NpgsqlConnection(ConnectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting("Raccord:TestingBypassAuth", TestingAuthBypass ? "true" : "false");
        builder.UseSetting("ConnectionStrings:raccord", ConnectionString);
        builder.UseSetting("Storage:Provider", "FileSystem");
        builder.UseSetting("Storage:FileSystemRoot", DocumentRoot);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing && Directory.Exists(DocumentRoot))
        {
            Directory.Delete(DocumentRoot, recursive: true);
        }
    }
}
