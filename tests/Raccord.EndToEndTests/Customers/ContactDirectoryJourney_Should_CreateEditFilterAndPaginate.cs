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
                await page.GetByLabel("Filter by role").SelectOptionAsync("Supplier");
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
                await page.GetByRole(AriaRole.Link, new() { Name = $"{companyName} Edited" })
                    .WaitForAsync();
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
        var companyRow = page.GetByRole(AriaRole.Row).Filter(new() { HasText = companyName });
        await companyRow.WaitForAsync();
        await page.GetByLabel("Filter by role").SelectOptionAsync("Venue");
        await companyRow.WaitForAsync(new() { State = WaitForSelectorState.Detached });
        await page.Locator("tbody tr").Nth(9).WaitForAsync();
        var firstPageVenueNames = await page.Locator("tbody tr a").AllTextContentsAsync();
        firstPageVenueNames.Count.ShouldBe(10);
        firstPageVenueNames.ShouldNotContain(venueName);

        await page.GetByRole(AriaRole.Button, new() { Name = "Next" }).ClickAsync();
        await page.GetByText("Page 2 of 2").WaitForAsync();
        var secondPageVenueNames = await page.Locator("tbody tr a").AllTextContentsAsync();
        secondPageVenueNames.ShouldBe([venueName]);
        firstPageVenueNames.ShouldNotContain(secondPageVenueNames[0]);

        await page.GetByRole(AriaRole.Button, new() { Name = "Previous" }).ClickAsync();
        await page.GetByText("Page 1 of 2").WaitForAsync();
        var returnedPageVenueNames = await page.Locator("tbody tr a").AllTextContentsAsync();
        returnedPageVenueNames.ShouldBe(firstPageVenueNames);
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
