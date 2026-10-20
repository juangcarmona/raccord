using System.Net.Http.Json;
using Npgsql;
using Raccord.Contracts.Contacts;
using Raccord.Web.Tests.Infrastructure;

namespace Raccord.Web.Tests;

public sealed class ContactPersonPersistenceTests(PostgresContainer postgres) : IAsyncLifetime
{
    private RaccordApi api = null!;
    private HttpClient client = null!;

    public async ValueTask InitializeAsync()
    {
        api = await RaccordApi.StartAsync(postgres);
        client = api.CreateClient();
    }

    public async ValueTask DisposeAsync()
    {
        client.Dispose();
        await api.DisposeAsync();
    }

    [Fact]
    public async Task Contact_persons_Should_BeStoredUnderTheirParentContact()
    {
        var contactId = await CreateContactAsync();
        var personId = Guid.CreateVersion7();

        await InsertPersonAsync(personId, contactId, "Alex Rivera", isDefault: true);

        var savedContactId = await QueryScalarAsync<Guid>(
            "SELECT contact_id FROM contacts.contact_persons WHERE id = @personId",
            ("personId", personId)
        );

        savedContactId.ShouldBe(contactId);
    }

    [Fact]
    public async Task Contact_persons_Should_RejectDuplicateDefaultAndAdministrativeDesignations()
    {
        var contactId = await CreateContactAsync();
        var defaultPersonId = Guid.CreateVersion7();
        var adminPersonId = Guid.CreateVersion7();
        await InsertPersonAsync(defaultPersonId, contactId, "Alex Rivera", isDefault: true);
        await InsertPersonAsync(adminPersonId, contactId, "Bea Costa", isAdministrative: true);

        var duplicateDefault = await UpdateDesignationAsync(adminPersonId, "is_default", true);
        var duplicateAdmin = await UpdateDesignationAsync(
            defaultPersonId,
            "is_administrative",
            true
        );

        duplicateDefault.SqlState.ShouldBe(PostgresErrorCodes.UniqueViolation);
        duplicateAdmin.SqlState.ShouldBe(PostgresErrorCodes.UniqueViolation);
    }

    [Fact]
    public async Task Failed_contact_person_update_Should_RollBackThePreviousDefault()
    {
        var contactId = await CreateContactAsync();
        var formerDefaultId = Guid.CreateVersion7();
        var newPersonId = Guid.CreateVersion7();
        await InsertPersonAsync(
            formerDefaultId,
            contactId,
            "Alex Rivera",
            isDefault: true,
            isAdministrative: true
        );
        await InsertPersonAsync(newPersonId, contactId, "Bea Costa");

        await using var connection = new NpgsqlConnection(api.ConnectionString);
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();
        await ExecuteAsync(
            connection,
            transaction,
            "UPDATE contacts.contact_persons SET is_default = false WHERE id = @personId",
            ("personId", formerDefaultId)
        );
        var error = await Should.ThrowAsync<PostgresException>(() =>
            ExecuteAsync(
                connection,
                transaction,
                "UPDATE contacts.contact_persons SET is_default = true, is_administrative = true WHERE id = @personId",
                ("personId", newPersonId)
            )
        );
        await transaction.RollbackAsync();

        error.SqlState.ShouldBe(PostgresErrorCodes.UniqueViolation);
        var previousDefault = await QueryScalarAsync<bool>(
            "SELECT is_default FROM contacts.contact_persons WHERE id = @personId",
            ("personId", formerDefaultId)
        );
        previousDefault.ShouldBeTrue();
    }

    private async Task<Guid> CreateContactAsync()
    {
        var response = await client.PostAsJsonAsync(
            "/api/contacts",
            new SaveContactRequest(
                "Company",
                "Contact person test company",
                null,
                null,
                null,
                null,
                ["Customer"]
            )
        );
        response.EnsureSuccessStatusCode();
        var contact = await response.Content.ReadFromJsonAsync<ContactResponse>();
        return contact!.Id;
    }

    private async Task InsertPersonAsync(
        Guid id,
        Guid contactId,
        string name,
        bool isDefault = false,
        bool isAdministrative = false
    ) =>
        await ExecuteAsync(
            "INSERT INTO contacts.contact_persons (id, contact_id, name, is_default, is_administrative) VALUES (@id, @contactId, @name, @isDefault, @isAdministrative)",
            ("id", id),
            ("contactId", contactId),
            ("name", name),
            ("isDefault", isDefault),
            ("isAdministrative", isAdministrative)
        );

    private async Task<PostgresException> UpdateDesignationAsync(
        Guid id,
        string column,
        bool value
    ) =>
        await Should.ThrowAsync<PostgresException>(() =>
            ExecuteAsync(
                $"UPDATE contacts.contact_persons SET {column} = @value WHERE id = @id",
                ("value", value),
                ("id", id)
            )
        );

    private async Task<T?> QueryScalarAsync<T>(
        string sql,
        params (string Name, object Value)[] args
    )
    {
        await using var connection = new NpgsqlConnection(api.ConnectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(sql, connection);
        AddParameters(command, args);
        return (T?)await command.ExecuteScalarAsync();
    }

    private async Task ExecuteAsync(string sql, params (string Name, object Value)[] args)
    {
        await using var connection = new NpgsqlConnection(api.ConnectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(sql, connection);
        AddParameters(command, args);
        await command.ExecuteNonQueryAsync();
    }

    private static async Task ExecuteAsync(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        string sql,
        params (string Name, object Value)[] args
    )
    {
        await using var command = new NpgsqlCommand(sql, connection, transaction);
        AddParameters(command, args);
        await command.ExecuteNonQueryAsync();
    }

    private static void AddParameters(
        NpgsqlCommand command,
        IEnumerable<(string Name, object Value)> args
    )
    {
        foreach (var (name, value) in args)
        {
            command.Parameters.AddWithValue(name, value);
        }
    }
}
