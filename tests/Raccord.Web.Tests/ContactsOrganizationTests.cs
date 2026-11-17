using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Raccord.Contracts.Contacts;
using Raccord.Infrastructure.Persistence;
using Raccord.Modules.Contacts;
using Raccord.Web.Tests.Infrastructure;

namespace Raccord.Web.Tests;

public sealed class ContactsOrganizationTests(PostgresContainer postgres) : IAsyncLifetime
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
    public async Task ContactsOrganization_Should_CreateTagAssignFolderAndFilterAnyOrAll()
    {
        var first = await CreateContactAsync("First contact");
        var second = await CreateContactAsync("Second contact");
        await AssertTagFiltersAsync(first, second);
        await AssertFolderAssignmentAndDeletionAsync(first);
    }

    private async Task AssertTagFiltersAsync(Guid first, Guid second)
    {
        var venueTag = await CreateTagAsync("venue");
        var customerTag = await CreateTagAsync("customer");
        (
            await client.PostAsJsonAsync("/api/contacts/tags", new { name = "venue" })
        ).StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);

        (
            await client.PutAsJsonAsync($"/api/contacts/{first}/tags/{venueTag}", new { })
        ).StatusCode.ShouldBe(HttpStatusCode.NoContent);
        (
            await client.PutAsJsonAsync($"/api/contacts/{first}/tags/{customerTag}", new { })
        ).StatusCode.ShouldBe(HttpStatusCode.NoContent);
        (
            await client.PutAsJsonAsync($"/api/contacts/{second}/tags/{venueTag}", new { })
        ).StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var any = await client.GetFromJsonAsync<ContactPageResponse>(
            $"/api/contacts?tagIds={venueTag}&tagIds={customerTag}&tagMatch=any"
        );
        any!.Items.Select(contact => contact.Id).ShouldBe([first, second]);

        var all = await client.GetFromJsonAsync<ContactPageResponse>(
            $"/api/contacts?tagIds={venueTag}&tagIds={customerTag}&tagMatch=all"
        );
        all!.Items.Select(contact => contact.Id).ShouldBe([first]);
    }

    private async Task AssertFolderAssignmentAndDeletionAsync(Guid contact)
    {
        var rootFolder = await CreateFolderAsync("Clients", null);
        var childFolder = await CreateFolderAsync("Priority", rootFolder);
        (
            await client.PutAsJsonAsync(
                $"/api/contacts/{contact}/folder",
                new { folderId = childFolder }
            )
        ).StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var inFolder = await client.GetFromJsonAsync<ContactPageResponse>(
            $"/api/contacts?folderId={childFolder}"
        );
        inFolder!.Items.Select(item => item.Id).ShouldBe([contact]);

        var otherFolder = await CreateFolderAsync("General", null);
        (
            await client.PutAsJsonAsync(
                $"/api/contacts/{contact}/folder",
                new { folderId = otherFolder }
            )
        ).StatusCode.ShouldBe(HttpStatusCode.NoContent);
        var movedContacts = await client.GetFromJsonAsync<ContactPageResponse>(
            $"/api/contacts?folderId={otherFolder}"
        );
        movedContacts!.Items.Select(item => item.Id).ShouldBe([contact]);
        var priorFolder = await client.GetFromJsonAsync<ContactPageResponse>(
            $"/api/contacts?folderId={childFolder}"
        );
        priorFolder!.Items.ShouldBeEmpty();

        (await client.DeleteAsync($"/api/contacts/folders/{rootFolder}")).StatusCode.ShouldBe(
            HttpStatusCode.UnprocessableEntity
        );
    }

    [Fact]
    public async Task ContactsOrganization_Should_RejectMovingFolderIntoDescendant()
    {
        var root = await CreateFolderAsync("Root", null);
        var child = await CreateFolderAsync("Child", root);

        var response = await client.PutAsJsonAsync(
            $"/api/contacts/folders/{root}",
            new
            {
                name = "Root",
                parentId = child,
                position = 0,
            }
        );

        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        (await client.GetFromJsonAsync<JsonElement[]>("/api/contacts/folders"))!.Length.ShouldBe(2);
    }

    [Fact]
    public async Task ContactsOrganization_Should_ReorderFoldersAndProtectAssignedTags()
    {
        var first = await CreateFolderAsync("First", null);
        var second = await CreateFolderAsync("Second", null);
        var response = await client.PutAsJsonAsync(
            $"/api/contacts/folders/{second}",
            new
            {
                name = "Second",
                parentId = (Guid?)null,
                position = 0,
            }
        );
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var folders = await client.GetFromJsonAsync<JsonElement[]>("/api/contacts/folders");
        folders!.Select(folder => folder.GetProperty("id").GetGuid()).ShouldBe([second, first]);

        var contact = await CreateContactAsync("Tagged contact");
        var tag = await CreateTagAsync("customer");
        (await client.PutAsync($"/api/contacts/{contact}/tags/{tag}", null)).StatusCode.ShouldBe(
            HttpStatusCode.NoContent
        );
        (await client.DeleteAsync($"/api/contacts/tags/{tag}")).StatusCode.ShouldBe(
            HttpStatusCode.UnprocessableEntity
        );
        (await client.DeleteAsync($"/api/contacts/{contact}/tags/{tag}")).StatusCode.ShouldBe(
            HttpStatusCode.NoContent
        );
        (await client.DeleteAsync($"/api/contacts/tags/{tag}")).StatusCode.ShouldBe(
            HttpStatusCode.NoContent
        );
    }

    [Fact]
    public async Task ContactsOrganization_Should_RejectUnknownTargetsAndInvalidFilterModes()
    {
        var contact = await CreateContactAsync("Existing contact");
        var tag = await CreateTagAsync("existing-tag");
        var missing = Guid.NewGuid();

        (
            await client.PutAsync($"/api/contacts/{contact}/tags/{missing}", null)
        ).StatusCode.ShouldBe(HttpStatusCode.NotFound);
        (await client.PutAsync($"/api/contacts/{missing}/tags/{tag}", null)).StatusCode.ShouldBe(
            HttpStatusCode.NotFound
        );
        (
            await client.PutAsJsonAsync(
                $"/api/contacts/{contact}/folder",
                new { folderId = missing }
            )
        ).StatusCode.ShouldBe(HttpStatusCode.NotFound);
        (await client.GetAsync($"/api/contacts/{missing}/organization")).StatusCode.ShouldBe(
            HttpStatusCode.NotFound
        );
        (await client.GetAsync("/api/contacts?tagMatch=some")).StatusCode.ShouldBe(
            HttpStatusCode.UnprocessableEntity
        );
    }

    [Fact]
    public async Task ContactsOrganization_Should_SerializeOpposingMovesBeforeReadingHierarchy()
    {
        var first = await CreateFolderAsync("First", null);
        var second = await CreateFolderAsync("Second", null);
        var responses = await RunConcurrentWritesAsync(
            () =>
                client.PutAsJsonAsync(
                    $"/api/contacts/folders/{first}",
                    new
                    {
                        name = "First",
                        parentId = second,
                        position = 0,
                    }
                ),
            () =>
                client.PutAsJsonAsync(
                    $"/api/contacts/folders/{second}",
                    new
                    {
                        name = "Second",
                        parentId = first,
                        position = 0,
                    }
                )
        );
        var statuses = responses.Select(response => response.StatusCode).ToArray();
        statuses.Count(status => status == HttpStatusCode.OK).ShouldBe(1);
        statuses.Count(status => status == HttpStatusCode.UnprocessableEntity).ShouldBe(1);
        var folders = (
            await client.GetFromJsonAsync<ContactFolderResponse[]>("/api/contacts/folders")
        )!;
        folders.Count(folder => folder.ParentId is null).ShouldBe(1);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task ContactsOrganization_Should_SerializeFolderDeletionWithFilingOrChildCreation(
        bool createChild
    )
    {
        var folder = await CreateFolderAsync("Parent", null);
        var contact = await CreateContactAsync("Filed contact");
        var responses = await RunConcurrentWritesAsync(
            () => client.DeleteAsync($"/api/contacts/folders/{folder}"),
            () =>
                createChild
                    ? client.PostAsJsonAsync(
                        "/api/contacts/folders",
                        new { name = "Child", parentId = folder }
                    )
                    : client.PutAsJsonAsync(
                        $"/api/contacts/{contact}/folder",
                        new { folderId = folder }
                    )
        );
        responses.Any(response => response.IsSuccessStatusCode).ShouldBeTrue();
        responses
            .All(response =>
                response.IsSuccessStatusCode
                || response.StatusCode
                    is HttpStatusCode.NotFound
                        or HttpStatusCode.UnprocessableEntity
            )
            .ShouldBeTrue();
        (
            await api.QueryScalarAsync<long>(
                "SELECT count(*) FROM contacts.customers c LEFT JOIN contacts.contact_folders f ON f.id = c.folder_id WHERE c.folder_id IS NOT NULL AND f.id IS NULL"
            )
        ).ShouldBe(0);
        (
            await api.QueryScalarAsync<long>(
                "SELECT count(*) FROM contacts.contact_folders c LEFT JOIN contacts.contact_folders p ON p.id = c.parent_id WHERE c.parent_id IS NOT NULL AND p.id IS NULL"
            )
        ).ShouldBe(0);
    }

    [Fact]
    public async Task ContactsOrganization_Should_SerializeTagAssignmentAndDeletion()
    {
        var contact = await CreateContactAsync("Tagged");
        var tag = await CreateTagAsync("concurrent");
        var responses = await RunConcurrentWritesAsync(
            () => client.DeleteAsync($"/api/contacts/tags/{tag}"),
            () => client.PutAsync($"/api/contacts/{contact}/tags/{tag}", null)
        );
        responses.Count(response => response.IsSuccessStatusCode).ShouldBe(1);
        responses
            .All(response =>
                response.IsSuccessStatusCode
                || response.StatusCode
                    is HttpStatusCode.NotFound
                        or HttpStatusCode.UnprocessableEntity
            )
            .ShouldBeTrue();
    }

    [Fact]
    public async Task ContactsOrganization_Should_SerializeReorderAndDuplicateTagCreation()
    {
        var first = await CreateFolderAsync("First", null);
        var second = await CreateFolderAsync("Second", null);
        var third = await CreateFolderAsync("Third", null);
        var reordered = await RunConcurrentWritesAsync(
            () =>
                client.PutAsJsonAsync(
                    $"/api/contacts/folders/{second}",
                    new
                    {
                        name = "Second",
                        parentId = (Guid?)null,
                        position = 0,
                    }
                ),
            () =>
                client.PutAsJsonAsync(
                    $"/api/contacts/folders/{third}",
                    new
                    {
                        name = "Third",
                        parentId = (Guid?)null,
                        position = 0,
                    }
                )
        );
        reordered.All(response => response.IsSuccessStatusCode).ShouldBeTrue();
        var folders = (
            await client.GetFromJsonAsync<ContactFolderResponse[]>("/api/contacts/folders")
        )!;
        folders.Select(folder => folder.Position).ShouldBe([0, 1, 2]);
        folders.Select(folder => folder.Id).ShouldContain(first);
        var created = await RunConcurrentWritesAsync(
            () => client.PostAsJsonAsync("/api/contacts/tags", new { name = "same-name" }),
            () => client.PostAsJsonAsync("/api/contacts/tags", new { name = "SAME-NAME" })
        );
        created.Count(response => response.StatusCode == HttpStatusCode.Created).ShouldBe(1);
        created
            .Count(response => response.StatusCode == HttpStatusCode.UnprocessableEntity)
            .ShouldBe(1);
    }

    [Fact]
    public async Task ContactsOrganization_Should_CombineRepeatedTagsFolderRoleAndStablePages()
    {
        var tag = await CreateTagAsync("shared");
        var folder = await CreateFolderAsync("Shared", null);
        var contacts = new[]
        {
            await CreateContactAsync("A"),
            await CreateContactAsync("B"),
            await CreateContactAsync("C"),
        };
        foreach (var contact in contacts)
        {
            (
                await client.PutAsync($"/api/contacts/{contact}/tags/{tag}", null)
            ).EnsureSuccessStatusCode();
            (
                await client.PutAsJsonAsync(
                    $"/api/contacts/{contact}/folder",
                    new { folderId = folder }
                )
            ).EnsureSuccessStatusCode();
        }
        var query =
            $"/api/contacts?folderId={folder}&tagIds={tag}&tagIds={tag}&tagMatch=all&role=Supplier&pageSize=2";
        var first = (await client.GetFromJsonAsync<ContactPageResponse>(query + "&page=1"))!;
        var second = (await client.GetFromJsonAsync<ContactPageResponse>(query + "&page=2"))!;
        first.Items.Concat(second.Items).Select(contact => contact.Id).ShouldBe(contacts);
        first.TotalCount.ShouldBe(3);
        (
            await client.GetFromJsonAsync<ContactPageResponse>(
                query.Replace("role=Supplier", "role=Customer", StringComparison.Ordinal)
            )
        )!.Items.ShouldBeEmpty();
    }

    [Fact]
    public async Task ContactsOrganization_Should_PreserveOtherInstallationWithIdenticalNames()
    {
        await using var other = await RaccordApi.StartAsync(postgres);
        using var otherClient = other.CreateClient();
        var otherTag = (
            await (
                await otherClient.PostAsJsonAsync("/api/contacts/tags", new { name = "shared" })
            ).Content.ReadFromJsonAsync<ContactTagResponse>()
        )!;
        var otherFolder = (
            await (
                await otherClient.PostAsJsonAsync(
                    "/api/contacts/folders",
                    new { name = "Shared", parentId = (Guid?)null }
                )
            ).Content.ReadFromJsonAsync<ContactFolderResponse>()
        )!;
        var contact = await CreateContactAsync("Local");
        var tag = await CreateTagAsync("shared");
        var folder = await CreateFolderAsync("Shared", null);
        (
            await client.PutAsync($"/api/contacts/{contact}/tags/{otherTag.Id}", null)
        ).StatusCode.ShouldBe(HttpStatusCode.NotFound);
        (
            await client.PutAsJsonAsync(
                $"/api/contacts/{contact}/folder",
                new { folderId = otherFolder.Id }
            )
        ).StatusCode.ShouldBe(HttpStatusCode.NotFound);
        (await client.DeleteAsync($"/api/contacts/tags/{otherTag.Id}")).StatusCode.ShouldBe(
            HttpStatusCode.NotFound
        );
        (await client.DeleteAsync($"/api/contacts/folders/{otherFolder.Id}")).StatusCode.ShouldBe(
            HttpStatusCode.NotFound
        );
        (
            await client.GetFromJsonAsync<ContactPageResponse>(
                $"/api/contacts?tagIds={otherTag.Id}&folderId={otherFolder.Id}"
            )
        )!.Items.ShouldBeEmpty();
        (
            await client.PutAsync($"/api/contacts/{contact}/tags/{tag}", null)
        ).EnsureSuccessStatusCode();
        (
            await client.PutAsJsonAsync(
                $"/api/contacts/{contact}/folder",
                new { folderId = folder }
            )
        ).EnsureSuccessStatusCode();
        (await otherClient.GetFromJsonAsync<ContactTagResponse[]>("/api/contacts/tags"))!
            .Single()
            .ShouldBe(otherTag);
        (await otherClient.GetFromJsonAsync<ContactFolderResponse[]>("/api/contacts/folders"))!
            .Single()
            .ShouldBe(otherFolder);
        (
            await other.QueryScalarAsync<long>(
                "SELECT count(*) FROM contacts.contact_tag_assignments"
            )
        ).ShouldBe(0);
        (await other.QueryScalarAsync<long>("SELECT count(*) FROM contacts.customers")).ShouldBe(0);
    }

    [Fact]
    public async Task ContactsOrganization_Should_PropagateCancellationAndInfrastructureErrors()
    {
        await using var scope = api.Services.CreateAsyncScope();
        var directory = scope.ServiceProvider.GetRequiredService<ContactOrganizationDirectory>();
        using var cancelled = new CancellationTokenSource();
        await cancelled.CancelAsync();
        await Should.ThrowAsync<OperationCanceledException>(() =>
            directory.CreateTagAsync(new CreateContactTagRequest("cancelled"), cancelled.Token)
        );
        await api.ExecuteAsync(
            "CREATE FUNCTION contacts.fail_tag() RETURNS trigger LANGUAGE plpgsql AS $$ BEGIN RAISE EXCEPTION 'synthetic infrastructure failure'; END; $$; CREATE TRIGGER fail_tag BEFORE INSERT ON contacts.contact_tags FOR EACH ROW EXECUTE FUNCTION contacts.fail_tag();"
        );
        (
            await client.PostAsJsonAsync("/api/contacts/tags", new { name = "failure" })
        ).StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
        (await api.QueryScalarAsync<long>("SELECT count(*) FROM contacts.contact_tags")).ShouldBe(
            0
        );
    }

    [Fact]
    public async Task ContactsOrganization_Should_PreserveContactsAndImportsAcrossMigrationRollbackAndUpgrade()
    {
        var contact = await CreateContactAsync("Existing before upgrade");
        var folder = await CreateFolderAsync("Temporary organization", null);
        (
            await client.PutAsJsonAsync(
                $"/api/contacts/{contact}/folder",
                new { folderId = folder }
            )
        ).EnsureSuccessStatusCode();
        var bytes = Encoding.UTF8.GetBytes("Company,Imported before upgrade,Supplier\n");
        var mapping = new ContactImportMapping(
            false,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["type"] = 0,
                ["legalName"] = 1,
                ["roles"] = 2,
            }
        );
        using var upload = new MultipartFormDataContent();
        upload.Add(new ByteArrayContent(bytes), "file", "contacts.csv");
        upload.Add(
            new StringContent(JsonSerializer.Serialize(mapping, JsonSerializerOptions.Web)),
            "mapping"
        );
        upload.Add(
            new StringContent(Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant()),
            "sha256"
        );
        (await client.PostAsync("/api/contacts/import", upload)).EnsureSuccessStatusCode();
        await using var scope = api.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<RaccordDbContext>();
        var migrator = db.Database.GetService<IMigrator>();
        await migrator.MigrateAsync("20261021130515_VatDefaultConcurrency");
        (await api.QueryScalarAsync<long>("SELECT count(*) FROM contacts.customers")).ShouldBe(2);
        (
            await api.QueryScalarAsync<string>(
                "SELECT legal_name FROM contacts.customers WHERE legal_name = 'Imported before upgrade'"
            )
        ).ShouldBe("Imported before upgrade");
        await migrator.MigrateAsync();
        (
            await api.QueryScalarAsync<long>(
                "SELECT count(*) FROM contacts.customers WHERE folder_id IS NULL"
            )
        ).ShouldBe(2);
        (
            await client.GetFromJsonAsync<ContactResponse>($"/api/contacts/{contact}")
        )!.LegalName.ShouldBe("Existing before upgrade");
        (
            await client.GetFromJsonAsync<ContactFolderResponse[]>("/api/contacts/folders")
        )!.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("two words")]
    [InlineData(" leading")]
    [InlineData("trailing ")]
    [InlineData("tab\tname")]
    [InlineData("")]
    public async Task ContactsOrganization_Should_RejectWhitespaceInTagNames(string name)
    {
        (await client.PostAsJsonAsync("/api/contacts/tags", new { name })).StatusCode.ShouldBe(
            HttpStatusCode.UnprocessableEntity
        );
        (
            await client.GetFromJsonAsync<ContactTagResponse[]>("/api/contacts/tags")
        )!.ShouldBeEmpty();
    }

    [Fact]
    public async Task ContactsOrganization_Should_RenameUnfileAndDeleteEmptyFolder()
    {
        var contact = await CreateContactAsync("Unfiled contact");
        var folder = await CreateFolderAsync("Original", null);
        (
            await client.PutAsJsonAsync(
                $"/api/contacts/{contact}/folder",
                new { folderId = folder }
            )
        ).EnsureSuccessStatusCode();
        (
            await client.PutAsJsonAsync(
                $"/api/contacts/folders/{folder}",
                new
                {
                    name = "Renamed",
                    parentId = (Guid?)null,
                    position = 0,
                }
            )
        ).EnsureSuccessStatusCode();
        (await client.GetFromJsonAsync<ContactFolderResponse[]>("/api/contacts/folders"))!
            .Single()
            .Name.ShouldBe("Renamed");
        (
            await client.PutAsJsonAsync(
                $"/api/contacts/{contact}/folder",
                new { folderId = (Guid?)null }
            )
        ).EnsureSuccessStatusCode();
        (
            await client.GetFromJsonAsync<ContactOrganizationResponse>(
                $"/api/contacts/{contact}/organization"
            )
        )!.FolderId.ShouldBeNull();
        (await client.DeleteAsync($"/api/contacts/folders/{folder}")).StatusCode.ShouldBe(
            HttpStatusCode.NoContent
        );
        (
            await client.GetFromJsonAsync<ContactResponse>($"/api/contacts/{contact}")
        )!.LegalName.ShouldBe("Unfiled contact");
    }

    private async Task<HttpResponseMessage[]> RunConcurrentWritesAsync(
        Func<Task<HttpResponseMessage>> first,
        Func<Task<HttpResponseMessage>> second
    )
    {
        await using var connection = new NpgsqlConnection(api.ConnectionString);
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();
        await using (
            var command = new NpgsqlCommand(
                "SELECT pg_advisory_xact_lock(302, 1)",
                connection,
                transaction
            )
        )
        {
            await command.ExecuteNonQueryAsync();
        }
        var requests = new[] { first(), second() };
        try
        {
            await AssertWaitingWritersAsync();
            requests.All(request => !request.IsCompleted).ShouldBeTrue();
        }
        finally
        {
            await transaction.RollbackAsync();
            await Task.WhenAll(requests);
        }
        return await Task.WhenAll(requests);
    }

    private async Task AssertWaitingWritersAsync()
    {
        var waiting = 0L;
        for (var attempt = 0; attempt < 100 && waiting < 2; attempt++)
        {
            waiting = await api.QueryScalarAsync<long>(
                "SELECT count(*) FROM pg_stat_activity WHERE datname = current_database() AND wait_event_type = 'Lock' AND query LIKE '%pg_advisory_xact_lock(302, 1)%'"
            );
            if (waiting < 2)
                await Task.Delay(20);
        }
        waiting.ShouldBe(2);
    }

    private async Task<Guid> CreateContactAsync(string name)
    {
        var response = await client.PostAsJsonAsync(
            "/api/contacts",
            new SaveContactRequest("Company", name, null, null, null, null, ["Supplier"])
        );
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return body.RootElement.GetProperty("id").GetGuid();
    }

    private async Task<Guid> CreateTagAsync(string name)
    {
        var response = await client.PostAsJsonAsync("/api/contacts/tags", new { name });
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return body.RootElement.GetProperty("id").GetGuid();
    }

    private async Task<Guid> CreateFolderAsync(string name, Guid? parentId)
    {
        var response = await client.PostAsJsonAsync(
            "/api/contacts/folders",
            new { name, parentId }
        );
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return body.RootElement.GetProperty("id").GetGuid();
    }
}
