using Npgsql;
using Testcontainers.PostgreSql;

[assembly: AssemblyFixture(typeof(Raccord.Web.Tests.Infrastructure.PostgresContainer))]

namespace Raccord.Web.Tests.Infrastructure;

/// <summary>One PostgreSQL server for the test run; every test gets its own database.</summary>
public sealed class PostgresContainer : IAsyncLifetime
{
    private PostgreSqlContainer? container;
    private string connectionString = null!;

    public async ValueTask InitializeAsync()
    {
        connectionString = Environment.GetEnvironmentVariable("RACCORD_TEST_DATABASE")!;
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            await using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            return;
        }

        container = new PostgreSqlBuilder("postgres:17-alpine").Build();
        await container.StartAsync();
        connectionString = container.GetConnectionString();
    }

    public async ValueTask DisposeAsync()
    {
        if (container is not null)
            await container.DisposeAsync();
    }

    /// <summary>Creates an empty database and returns its connection string.</summary>
    public async Task<string> CreateDatabaseAsync()
    {
        var name = "raccord_" + Guid.NewGuid().ToString("N");
        await using (var connection = new NpgsqlConnection(connectionString))
        {
            await connection.OpenAsync();
            await using var command = new NpgsqlCommand($"CREATE DATABASE {name}", connection);
            await command.ExecuteNonQueryAsync();
        }

        return new NpgsqlConnectionStringBuilder(connectionString)
        {
            Database = name,
        }.ConnectionString;
    }
}
