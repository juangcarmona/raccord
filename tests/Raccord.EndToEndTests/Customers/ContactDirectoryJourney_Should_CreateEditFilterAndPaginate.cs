using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Xunit;

namespace Raccord.EndToEndTests.Customers;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Contacts)]
public sealed class ContactDirectoryJourney_Should_CreateEditFilterAndPaginate(
    DistributedAppFixture app
) : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_CreateCompanyAndVenue_EditAndBrowseByRoleAcrossPages()
    {
        await RunJourneyAsync(
            "contacts-directory-create-filter-paginate",
            async page =>
            {
                var companyName = $"Northlight E2E {RunStamp}";
                await page.GotoAsync("/contacts/new");
                await page.GetByLabel("Contact type").SelectOptionAsync("Company");
                await page.GetByLabel("Legal name").FillAsync(companyName);
                await page.GetByRole(AriaRole.Checkbox, new() { Name = "Customer" }).CheckAsync();
                await page.GetByRole(AriaRole.Checkbox, new() { Name = "Supplier" }).CheckAsync();
                await page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
                await page.WaitForURLAsync(url =>
                    url.EndsWith("/contacts", StringComparison.Ordinal)
                );
                var supplierListLoaded = page.WaitForResponseAsync(response =>
                    response.Url.Contains("/api/contacts?", StringComparison.Ordinal)
                    && response.Url.Contains("role=Supplier", StringComparison.Ordinal)
                    && response.Status == 200
                );
                await page.GetByLabel("Filter by role").SelectOptionAsync("Supplier");
                await supplierListLoaded;
                var companyRow = page.GetByRole(AriaRole.Row)
                    .Filter(new() { HasText = companyName });
                await companyRow.WaitForAsync();

                await companyRow
                    .GetByRole(AriaRole.Link, new() { Name = companyName })
                    .ClickAsync();
                await page.GetByLabel("Legal name").FillAsync($"{companyName} Edited");
                await page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
                await page.WaitForURLAsync(url =>
                    url.EndsWith("/contacts", StringComparison.Ordinal)
                );
                await page.GetByLabel("Filter by role").SelectOptionAsync("Supplier");
                await page.GetByText($"{companyName} Edited").WaitForAsync();

                await SeedVenueContactsAsync(page);
                var venueName = $"Marina Venue E2E {RunStamp}";
                await page.GetByRole(AriaRole.Link, new() { Name = "New contact" }).ClickAsync();
                await page.GetByLabel("Contact type").SelectOptionAsync("PrivatePerson");
                await page.GetByLabel("Legal name").FillAsync(venueName);
                await page.GetByRole(AriaRole.Checkbox, new() { Name = "Venue" }).CheckAsync();
                await page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
                await page.WaitForURLAsync(url =>
                    url.EndsWith("/contacts", StringComparison.Ordinal)
                );

                await AssertProjectCustomerOptionsAsync(page, companyName, venueName);
                await AssertStableVenuePaginationAsync(page, companyName, venueName);

                await page.GetByLabel("Filter by role").SelectOptionAsync("Supplier");
                var supplierNames = await ReadAllContactNamesAcrossPagesAsync(page);
                supplierNames.ShouldContain($"{companyName} Edited");
            }
        );
    }

    private static async Task AssertProjectCustomerOptionsAsync(
        IPage page,
        string companyName,
        string venueName
    )
    {
        await page.GotoAsync("/projects/new");
        var customerField = page.GetByLabel("Customer");
        await customerField
            .Locator("option")
            .Filter(new() { HasText = $"{companyName} Edited" })
            .WaitForAsync(new() { State = WaitForSelectorState.Attached });
        var options = await customerField.Locator("option").AllTextContentsAsync();
        options.ShouldContain($"{companyName} Edited");
        options.ShouldNotContain(venueName);
    }

    private static async Task AssertStableVenuePaginationAsync(
        IPage page,
        string companyName,
        string venueName
    )
    {
        await page.GotoAsync("/contacts");
        await page.GetByLabel("Filter by role").SelectOptionAsync("Venue");
        await page.Locator("tbody tr").Nth(9).WaitForAsync();
        var firstPageVenueNames = await page.Locator("tbody tr th a").AllTextContentsAsync();
        firstPageVenueNames.Count.ShouldBe(10);

        await page.GetByRole(AriaRole.Button, new() { Name = "Next" }).ClickAsync();
        await page.GetByText("Page 2 of", new() { Exact = false }).WaitForAsync();
        var secondPageVenueNames = await page.Locator("tbody tr th a").AllTextContentsAsync();
        firstPageVenueNames.ShouldNotContain(secondPageVenueNames[0]);

        await page.GetByRole(AriaRole.Button, new() { Name = "Previous" }).ClickAsync();
        await page.GetByText("Page 1 of", new() { Exact = false }).WaitForAsync();
        var returnedPageVenueNames = await page.Locator("tbody tr th a").AllTextContentsAsync();
        returnedPageVenueNames.ShouldBe(firstPageVenueNames);

        var allVenueNames = await ReadAllContactNamesAcrossPagesAsync(page);
        allVenueNames.ShouldContain(venueName);
        allVenueNames.ShouldNotContain(companyName);
        allVenueNames.Count.ShouldBe(allVenueNames.Distinct(StringComparer.Ordinal).Count());
    }

    private static async Task<List<string>> ReadAllContactNamesAcrossPagesAsync(IPage page)
    {
        var names = new List<string>();
        var pageNumber = 1;
        while (true)
        {
            await page.Locator("tbody tr").First.WaitForAsync();
            names.AddRange(await page.Locator("tbody tr th a").AllTextContentsAsync());
            var next = page.GetByRole(AriaRole.Button, new() { Name = "Next" });
            if (await next.IsDisabledAsync())
            {
                break;
            }

            await next.ClickAsync();
            pageNumber++;
            await page.GetByText($"Page {pageNumber} of", new() { Exact = false }).WaitForAsync();
        }

        return names;
    }

    private static async Task SeedVenueContactsAsync(IPage page)
    {
        var result = await page.EvaluateAsync<int>(
            """
            async () => {
                for (let index = 1; index <= 10; index++) {
                    const response = await fetch('/api/contacts', {
                        method: 'POST',
                        headers: { 'content-type': 'application/json' },
                        body: JSON.stringify({
                            type: 'PrivatePerson',
                            legalName: `Venue Page ${index} ${Date.now()}`,
                            contactNumber: null,
                            taxId: null,
                            address: null,
                            email: null,
                            roles: ['Venue']
                        })
                    });
                    if (!response.ok) return response.status;
                }
                return 201;
            }
            """
        );
        result.ShouldBe(201);
    }
}
