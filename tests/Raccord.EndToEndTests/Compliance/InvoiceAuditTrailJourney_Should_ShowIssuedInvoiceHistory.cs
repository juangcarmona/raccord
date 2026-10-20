using System.Globalization;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Compliance;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Compliance)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Invoicing)]
[Trait(E2ETraits.Rule, "BR-AUDITED-ACTIONS")]
public sealed class InvoiceAuditTrailJourney_Should_ShowIssuedInvoiceHistory(
    DistributedAppFixture app
) : BrowserJourneyBase(app)
{
    /// <summary>
    /// Given a newly issued synthetic invoice, when the administrator opens its audit trail, then
    /// the browser shows each issued-invoice action with its actor and recorded time.
    /// ProductShape: UC-REVIEW-AUDIT-TRAIL, BR-AUDITED-ACTIONS, FR-AUDIT-001.
    /// </summary>
    [Fact]
    public async Task Journey_Should_ShowWhoIssuedTheInvoiceAndWhen()
    {
        await RunJourneyAsync(
            "invoice-audit-trail",
            async page =>
            {
                await CreateCustomerAsync(page);
                await CreateProjectWithBillableLineAsync(page);
                await FinalizeDraftAsync(page);

                await page.GetByRole(AriaRole.Heading, new() { Name = "Audit trail" })
                    .WaitForAsync();

                var events = page.Locator("ol.audit li");
                await events.Nth(1).WaitForAsync();
                (await events.CountAsync()).ShouldBe(2);

                var issued = events.Nth(0);
                (await issued.InnerTextAsync()).ShouldContain("Invoice issued");
                (await issued.InnerTextAsync()).ShouldContain("admin@example.com");
                var issuedAt = await issued.Locator("time").GetAttributeAsync("datetime");
                DateTimeOffset
                    .TryParse(issuedAt, CultureInfo.InvariantCulture, out _)
                    .ShouldBeTrue();

                var archived = events.Nth(1);
                (await archived.InnerTextAsync()).ShouldContain("PDF rendered and archived");
                (await archived.InnerTextAsync()).ShouldContain("admin@example.com");
                var archivedAt = await archived.Locator("time").GetAttributeAsync("datetime");
                DateTimeOffset
                    .TryParse(archivedAt, CultureInfo.InvariantCulture, out _)
                    .ShouldBeTrue();
                DateTimeOffset
                    .Parse(archivedAt!, CultureInfo.InvariantCulture)
                    .ShouldBeGreaterThanOrEqualTo(
                        DateTimeOffset.Parse(issuedAt!, CultureInfo.InvariantCulture)
                    );
            }
        );
    }
}
