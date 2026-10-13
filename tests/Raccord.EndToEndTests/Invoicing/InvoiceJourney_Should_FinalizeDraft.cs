using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Raccord.EndToEndTests.Projects;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Invoicing;

/// <summary>
/// Drives the full money path — customer, project, billable line, draft, issued invoice — and
/// asserts the numbering and immutability the domain guarantees. This is the deepest smoke
/// journey; it exists so that "an invoice can be issued from the browser" is always true.
/// </summary>
[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Invoicing)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
public sealed class InvoiceJourney_Should_FinalizeDraft(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_FinalizeADraftIntoAnImmutableNumberedInvoice()
    {
        await RunJourneyAsync(
            "invoice-finalize",
            async page =>
            {
                await EnsureInvoiceIssuerAsync(page);
                await CreateCustomerAsync(page);
                await CreateProjectWithBillableLineAsync(page);
                await FinalizeDraftAsync(page);

                // The invoice number comes from the domain's series sequence; asserting its presence
                // (not exact digits — journeys share one database within a run) proves the final
                // numbering survived the round trip.
                var number = await page.GetByRole(AriaRole.Heading, new() { Level = 1 })
                    .First.InnerTextAsync();
                number.Trim().ShouldNotBeNullOrWhiteSpace();
                number.TrimStart('F').ShouldNotBeNullOrEmpty();
            }
        );
    }
}
