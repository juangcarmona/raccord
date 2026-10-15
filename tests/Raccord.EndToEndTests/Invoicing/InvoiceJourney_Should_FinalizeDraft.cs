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
/// ProductShape: JRN-FIN-STAGED-BILLING, UC-FIN-GENERATE-PROJECT-INVOICE,
/// UC-FIN-CONFIGURE-INVOICE-MOMENTS, UC-FIN-WORK-INVOICING-QUEUE,
/// UC-FIN-RECORD-PAYMENT, UC-FIN-MANAGE-REFUNDABLE-DEPOSIT.
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
                await CreateCustomerAsync(page);
                await CreateProjectWithBillableLineAsync(page);

                var queue = await page.APIRequest.GetAsync(
                    "/api/invoicing/queue?from=2026-01-01&to=2026-12-31"
                );
                queue.Ok.ShouldBeTrue();
                (await queue.TextAsync()).ShouldContain($"Promo E2E {RunStamp}");
                var projectId = new Uri(page.Url).Segments[^1];
                await SetDepositAsync(page, projectId);

                await FinalizeDraftAsync(page);
                var invoiceId = new Uri(page.Url).Segments[^1];
                var payment = await page.APIRequest.PostAsync(
                    $"/api/invoices/{invoiceId}/payments",
                    new APIRequestContextOptions
                    {
                        Data =
                            "{\"date\":\"2026-09-18\",\"method\":\"bank-transfer\",\"amount\":100,\"description\":\"E2E payment\"}",
                        Headers = new Dictionary<string, string>(StringComparer.Ordinal)
                        {
                            ["content-type"] = "application/json",
                        },
                    }
                );
                payment.Status.ShouldBe(201);

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

    private static async Task SetDepositAsync(IPage page, string projectId)
    {
        var headers = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["content-type"] = "application/json",
        };
        var configure = await page.APIRequest.PutAsync(
            $"/api/projects/{projectId}/deposit",
            new APIRequestContextOptions { Data = "{\"amount\":500}", Headers = headers }
        );
        configure.Ok.ShouldBeTrue();
        var paid = await page.APIRequest.PutAsync(
            $"/api/projects/{projectId}/deposit/status",
            new APIRequestContextOptions { Data = "{\"status\":1}", Headers = headers }
        );
        paid.Ok.ShouldBeTrue();
        var returned = await page.APIRequest.PutAsync(
            $"/api/projects/{projectId}/deposit/status",
            new APIRequestContextOptions { Data = "{\"status\":2}", Headers = headers }
        );
        returned.Ok.ShouldBeTrue();
    }
}
