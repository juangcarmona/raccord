using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Xunit;

namespace Raccord.EndToEndTests.Customers;

/// <summary>
/// Registers a customer through the real form and asserts it appears in the list, invoiceable —
/// the customer leg of the smoke set, and the setup leg every other journey reuses.
/// </summary>
[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Contacts)]
public sealed class CustomerJourney_Should_RegisterAndAppear(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_RegisterACustomerAndListItAsInvoiceable()
    {
        await RunJourneyAsync(
            "customer-register",
            async page =>
            {
                await CreateCustomerAsync(page);

                // The badge flips to "Ready to invoice" only when tax id and address satisfy the
                // invoiceability rule — the actual assertion of this journey.
                await page.GetByRole(AriaRole.Row)
                    .Filter(new() { HasText = $"Productora E2E {RunStamp}" })
                    .GetByText("Ready to invoice")
                    .WaitForAsync();
            }
        );
    }
}
