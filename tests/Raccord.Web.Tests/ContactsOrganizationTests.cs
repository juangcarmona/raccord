using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Raccord.Contracts.Contacts;
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
