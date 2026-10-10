using Npgsql;
using Testcontainers.PostgreSql;

[assembly: AssemblyFixture(typeof(Raccord.Web.Tests.Infrastructure.PostgresContainer))]

namespace Raccord.Web.Tests.Infrastructure;

/// <summary>One PostgreSQL server for the test run; every test gets its own database.</summary>
public sealed class PostgresContainer : IAsyncLifetime
{
    private readonly PostgreSqlContainer container = new PostgreSqlBuilder("postgres:17-alpine").Build();

    public async ValueTask InitializeAsync() => await container.StartAsync();

    public async ValueTask DisposeAsync() => await container.DisposeAsync();

    /// <summary>Creates an empty database and returns its connection string.</summary>
    public async Task<string> CreateDatabaseAsync()
    {
        var name = "raccord_" + Guid.NewGuid().ToString("N");
        await using (var connection = new NpgsqlConnection(container.GetConnectionString()))
        {
            await connection.OpenAsync();
            await using var command = new NpgsqlCommand($"CREATE DATABASE {name}", connection);
            await command.ExecuteNonQueryAsync();
        }

        return new NpgsqlConnectionStringBuilder(container.GetConnectionString()) { Database = name }.ConnectionString;
    }
}
