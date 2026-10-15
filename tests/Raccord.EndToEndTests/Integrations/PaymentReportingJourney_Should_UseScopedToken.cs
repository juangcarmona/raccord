using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Xunit;

namespace Raccord.EndToEndTests.Integrations;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Invoicing)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Full)]
public sealed class PaymentReportingJourney_Should_UseScopedToken(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    private static readonly string[] PaymentScopes = ["invoices.read", "payments.write"];

    [Fact]
    public async Task Journey_Should_ReadInvoicesWithPaymentScope()
    {
        await RunJourneyAsync(
            "payment-reporting",
            async page =>
            {
                var issue = await page.APIRequest.PostAsync(
                    "/api/integrations/api-tokens",
                    new()
                    {
                        DataObject = new
                        {
                            name = $"payment-reporting-{RunStamp}",
                            scopes = PaymentScopes,
                        },
                    }
                );
                issue.Status.ShouldBe((int)HttpStatusCode.Created);
                var token = (await issue.JsonAsync<TokenResponse>())!.Token!;

                var invoices = await page.APIRequest.GetAsync(
                    "/api/integrations/invoices",
                    new()
                    {
                        Headers = new Dictionary<string, string>(StringComparer.Ordinal)
                        {
                            ["X-Api-Key"] = token,
                        },
                    }
                );
                invoices.Status.ShouldBe((int)HttpStatusCode.OK);
                (await invoices.JsonAsync<JsonElement>()).ValueKind.ShouldBe(JsonValueKind.Array);

                await page.SetContentAsync(
                    "<h1>Payment reporting</h1><p>Scoped invoice read access is active.</p>"
                );
                await page.ScreenshotAsync(
                    new()
                    {
                        Path = $"playwright-artifacts/payment-reporting-{RunStamp}.png",
                        FullPage = true,
                    }
                );
            }
        );
    }

    private sealed record TokenResponse([property: JsonPropertyName("token")] string? Token);
}
