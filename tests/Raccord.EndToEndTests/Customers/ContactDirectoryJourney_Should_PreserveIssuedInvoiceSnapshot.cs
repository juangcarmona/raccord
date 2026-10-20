using System.Text.Json;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Xunit;

namespace Raccord.EndToEndTests.Customers;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Contacts)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Invoicing)]
public sealed class ContactDirectoryJourney_Should_PreserveIssuedInvoiceSnapshot(
    DistributedAppFixture app
) : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_KeepRecipientDetailsAfterEditingItsContact()
    {
        await RunJourneyAsync(
            "contacts-issued-invoice-snapshot",
            async page =>
            {
                var originalName = $"Productora E2E {RunStamp}";
                await CreateCustomerAsync(page);
                var customerId = await FindCustomerIdAsync(page, originalName);
                await CreateProjectWithBillableLineAsync(page);
                await FinalizeDraftAsync(page);

                var invoiceUrl = page.Url;
                var recipient = page.GetByRole(AriaRole.Region, new() { Name = "Recipient" });
                await recipient.GetByText(originalName).WaitForAsync();

                await page.GotoAsync($"/contacts/{customerId}");
                await page.GetByLabel("Legal name").FillAsync($"{originalName} Updated");
                await page.GetByLabel("Street and number").FillAsync("Calle Nueva 99");
                await page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
                await page.WaitForURLAsync(url =>
                    url.EndsWith("/contacts", StringComparison.Ordinal)
                );

                await page.GotoAsync(invoiceUrl);
                var reloadedRecipient = page.GetByRole(
                    AriaRole.Region,
                    new() { Name = "Recipient" }
                );
                await reloadedRecipient.GetByText(originalName).WaitForAsync();
                await reloadedRecipient.GetByText("Calle de pruebas 1").WaitForAsync();
                (
                    await reloadedRecipient.GetByText($"{originalName} Updated").CountAsync()
                ).ShouldBe(0);

                await CreateProjectWithBillableLineAsync(page, $"{originalName} Updated");
                await FinalizeDraftAsync(page);
                var futureRecipient = page.GetByRole(AriaRole.Region, new() { Name = "Recipient" });
                await futureRecipient.GetByText($"{originalName} Updated").WaitForAsync();
                await futureRecipient.GetByText("Calle Nueva 99").WaitForAsync();
            }
        );
    }

    private static async Task<string> FindCustomerIdAsync(IPage page, string legalName)
    {
        var response = await page.APIRequest.GetAsync("/api/customers");
        response.Ok.ShouldBeTrue();
        using var customers = JsonDocument.Parse(await response.TextAsync());
        return customers
            .RootElement.EnumerateArray()
            .Single(customer =>
                string.Equals(
                    customer.GetProperty("legalName").GetString(),
                    legalName,
                    StringComparison.Ordinal
                )
            )
            .GetProperty("id")
            .GetString()!;
    }
}
