using System.Text.Json;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Customers;

/// <summary>
/// Given contacts in the active deployment, when an administrator organizes them with tags and folders, then any/all tag filters and folder filters return the matching contacts and assigned folders cannot be deleted (SB-CON-ORGANIZE-AND-FILTER-CONTACTS).
/// </summary>
[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Contacts)]
public sealed class ContactOrganizationJourney_Should_ManageTagsFoldersAndFilters(
    DistributedAppFixture app
) : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_ManageTagsFoldersAndFilterAnyOrAll()
    {
        await RunJourneyAsync(
            "contacts-tags-folders-and-filters",
            page => RunOrganizationFlowAsync(page, RunStamp)
        );
    }

    private static async Task RunOrganizationFlowAsync(IPage page, string runStamp)
    {
        var sharedContact = $"Northlight Tagged {runStamp}";
        var partialContact = $"Northlight Partially Tagged {runStamp}";
        var sharedContactId = await CreateCompanyContactAsync(page, sharedContact);
        var partialContactId = await CreateCompanyContactAsync(page, partialContact);

        var tagA = $"production-{runStamp}";
        var tagB = $"venue-{runStamp}";
        var folder = $"Locations {runStamp}";
        await page.GotoAsync($"/contacts/{sharedContactId}/organization");
        await CreateTagAsync(page, tagA);
        await CreateTagAsync(page, tagB);
        await CreateFolderAsync(page, folder);
        await AssignTagAsync(page, tagA);
        await AssignTagAsync(page, tagB);
        var folderReload = page.WaitForResponseAsync(IsOrganizationReload);
        await page.GetByLabel("Assign folder")
            .SelectOptionAsync(new SelectOptionValue { Label = folder });
        await folderReload;

        await page.GotoAsync($"/contacts/{partialContactId}/organization");
        await AssignTagAsync(page, tagA);

        await page.GotoAsync("/contacts");
        await FilterByTagsAsync(page, tagA, tagB, "Any");
        var anyMatches = await ReadVisibleContactNamesAsync(page);
        anyMatches.ShouldContain(sharedContact);
        anyMatches.ShouldContain(partialContact);

        var allFilterResponse = WaitForDirectoryResponseAsync(
            page,
            uri => uri.Query.Contains("tagMatch=all", StringComparison.Ordinal)
        );
        await page.GetByLabel("Tag matching").SelectOptionAsync("all");
        await allFilterResponse;
        var allMatches = await ReadVisibleContactNamesAsync(page);
        allMatches.ShouldContain(sharedContact);
        allMatches.ShouldNotContain(partialContact);

        var folderFilterResponse = WaitForDirectoryResponseAsync(
            page,
            uri => uri.Query.Contains("folderId=", StringComparison.Ordinal)
        );
        await page.GetByLabel("Filter by folder")
            .SelectOptionAsync(new SelectOptionValue { Label = folder });
        await folderFilterResponse;
        var folderMatches = await ReadVisibleContactNamesAsync(page);
        folderMatches.ShouldBe([sharedContact]);

        await RefuseNonemptyFolderDeletionAsync(page, folder);
    }

    private static async Task RefuseNonemptyFolderDeletionAsync(IPage page, string folder)
    {
        await page.GetByRole(AriaRole.Link, new() { Name = "Organize" }).ClickAsync();
        var deleteResponseTask = page.WaitForResponseAsync(response =>
            response.Url.Contains("/api/contacts/folders/", StringComparison.Ordinal)
            && string.Equals(response.Request.Method, "DELETE", StringComparison.Ordinal)
        );
        await page.GetByRole(AriaRole.Button, new() { Name = $"Delete folder {folder}" })
            .ClickAsync();
        var deleteResponse = await deleteResponseTask;
        deleteResponse.Status.ShouldBe(422);
        await page.GetByRole(AriaRole.Alert).WaitForAsync();
    }

    private static async Task<Guid> CreateCompanyContactAsync(IPage page, string name)
    {
        await page.GotoAsync("/contacts/new");
        await page.GetByLabel("Contact type").SelectOptionAsync("Company");
        await page.GetByLabel("Legal name").FillAsync(name);
        await page.GetByRole(AriaRole.Checkbox, new() { Name = "Customer" }).CheckAsync();
        var createResponseTask = page.WaitForResponseAsync(response =>
            response.Url.EndsWith("/api/contacts", StringComparison.Ordinal)
            && string.Equals(response.Request.Method, "POST", StringComparison.Ordinal)
        );
        await page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
        var response = await createResponseTask;
        response.Status.ShouldBe(201);
        using var content = JsonDocument.Parse(await response.TextAsync());
        var contactId = content.RootElement.GetProperty("id").GetGuid();
        await page.WaitForURLAsync(url => url.EndsWith("/contacts", StringComparison.Ordinal));
        return contactId;
    }

    private static async Task CreateTagAsync(IPage page, string name)
    {
        await page.GetByLabel("New tag").FillAsync(name);
        await page.GetByRole(AriaRole.Button, new() { Name = "Create tag" }).ClickAsync();
        await page.GetByLabel(name, new() { Exact = true }).WaitForAsync();
    }

    private static async Task CreateFolderAsync(IPage page, string name)
    {
        await page.GetByLabel("Folder name").FillAsync(name);
        await page.GetByRole(AriaRole.Button, new() { Name = "Create folder" }).ClickAsync();
        await page.GetByLabel("Assign folder")
            .Locator("option")
            .Filter(new() { HasText = name })
            .WaitForAsync(new() { State = WaitForSelectorState.Attached });
    }

    private static async Task AssignTagAsync(IPage page, string name)
    {
        var reload = page.WaitForResponseAsync(IsOrganizationReload);
        await page.GetByLabel(name, new() { Exact = true }).ClickAsync();
        await reload;
    }

    private static bool IsOrganizationReload(IResponse response) =>
        response.Url.Contains("/api/contacts/", StringComparison.Ordinal)
        && response.Url.EndsWith("/organization", StringComparison.Ordinal)
        && string.Equals(response.Request.Method, "GET", StringComparison.Ordinal);

    private static async Task FilterByTagsAsync(IPage page, string tagA, string tagB, string match)
    {
        await page.GetByLabel(tagA, new() { Exact = true }).CheckAsync();
        var filterResponse = WaitForDirectoryResponseAsync(
            page,
            uri =>
                uri.Query.Split("tagIds=", StringSplitOptions.None).Length >= 3
                && uri.Query.Contains(
                    $"tagMatch={match.ToLowerInvariant()}",
                    StringComparison.Ordinal
                )
        );
        await page.GetByLabel(tagB, new() { Exact = true }).CheckAsync();
        await filterResponse;
    }

    private static async Task WaitForDirectoryResponseAsync(IPage page, Func<Uri, bool> matches)
    {
        var response = await page.WaitForResponseAsync(candidate =>
            candidate.Url.Contains("/api/contacts?", StringComparison.Ordinal)
            && Uri.TryCreate(candidate.Url, UriKind.Absolute, out var uri)
            && matches(uri)
        );
        response.Ok.ShouldBeTrue();
        await page.Locator("tbody tr").First.WaitForAsync();
    }

    private static async Task<IReadOnlyList<string>> ReadVisibleContactNamesAsync(IPage page) =>
        await page.Locator("tbody tr th a").AllTextContentsAsync();
}
