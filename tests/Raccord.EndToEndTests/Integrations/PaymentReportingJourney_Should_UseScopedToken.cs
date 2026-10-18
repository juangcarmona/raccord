using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Xunit;

namespace Raccord.EndToEndTests.Integrations;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.EvidenceMode, E2ETraits.EvidenceModes.Api)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Full)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Invoicing)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Workspace)]
public sealed class PaymentReportingJourney_Should_UseScopedToken(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    private static readonly string[] PaymentScopes = ["invoices.read", "payments.write"];

    /// <summary>
    /// Given an administrator issues an integration token with invoice and payment scopes, when
    /// an external client reads invoices, then the scoped invoice collection is available.
    /// ProductShape: UC-INT-REPORT-PAYMENT-THROUGH-API.
    /// </summary>
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
            }
        );
    }

    private sealed record TokenResponse([property: JsonPropertyName("token")] string? Token);
}
