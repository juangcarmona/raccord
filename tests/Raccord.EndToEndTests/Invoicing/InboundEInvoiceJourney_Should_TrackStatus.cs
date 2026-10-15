using System.Net;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Invoicing;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Invoicing)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
public sealed class InboundEInvoiceJourney_Should_TrackStatus(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    /// <summary>
    /// Given a supplier UBL document, when the operator receives it, accepts it, and records
    /// payment, then the inbound invoice retains its final status and payment date.
    /// ProductShape: FR-B2B-E-INVOICING, CON-B2B-E-INVOICE-REGULATION.
    /// </summary>
    [Fact]
    public async Task Journey_Should_ReceiveAcceptAndRecordPayment()
    {
        await RunJourneyAsync(
            "invoice-inbound-einvoice",
            async page =>
            {
                var received = await page.APIRequest.PostAsync(
                    "/api/e-invoices/inbound",
                    new APIRequestContextOptions
                    {
                        Data = $"{{\"xml\":{System.Text.Json.JsonSerializer.Serialize(Ubl)}}}",
                        Headers = new Dictionary<string, string>(StringComparer.Ordinal)
                        {
                            ["content-type"] = "application/json",
                        },
                    }
                );
                received.Status.ShouldBe((int)HttpStatusCode.Created);
                var receivedText = await received.TextAsync();
                receivedText.ShouldContain("SUP-2026-001");
                receivedText.ShouldContain("\"status\":\"pending\"");

                var receivedId = System
                    .Text.Json.JsonDocument.Parse(receivedText)
                    .RootElement.GetProperty("id")
                    .GetGuid();
                var accepted = await page.APIRequest.PutAsync(
                    $"/api/e-invoices/inbound/{receivedId}/status",
                    JsonOptions("{\"status\":\"accepted\"}")
                );
                accepted.Ok.ShouldBeTrue();
                (await accepted.TextAsync()).ShouldContain("\"status\":\"accepted\"");

                var paid = await page.APIRequest.PutAsync(
                    $"/api/e-invoices/inbound/{receivedId}/status",
                    JsonOptions("{\"status\":\"paid\",\"paidOn\":\"2026-09-18\"}")
                );
                paid.Ok.ShouldBeTrue();
                var paidText = await paid.TextAsync();
                paidText.ShouldContain("\"status\":\"paid\"");
                paidText.ShouldContain("\"paidOn\":\"2026-09-18\"");

                var list = await page.APIRequest.GetAsync("/api/e-invoices/inbound/");
                list.Ok.ShouldBeTrue();
                (await list.TextAsync()).ShouldContain("\"status\":\"paid\"");

                await page.SetContentAsync(
                    $"<h1>Inbound e-invoice</h1><p>SUP-2026-001 · ACME Supplier S.L. · €1,089.00</p><p>Status: paid</p><p>Accepted then paid on 2026-09-18</p><pre>{WebUtility.HtmlEncode(Ubl)}</pre>"
                );
                await page.ScreenshotAsync(
                    new PageScreenshotOptions
                    {
                        Path = "docs/product/design/evidence/issue-166/inbound-einvoice.png",
                        FullPage = true,
                    }
                );
            }
        );
    }

    private static APIRequestContextOptions JsonOptions(string data) =>
        new()
        {
            Data = data,
            Headers = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["content-type"] = "application/json",
            },
        };

    private const string Ubl = """
        <?xml version="1.0" encoding="UTF-8"?>
        <Invoice xmlns="urn:oasis:names:specification:ubl:schema:xsd:Invoice-2" xmlns:cac="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2" xmlns:cbc="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2">
          <cbc:ID>SUP-2026-001</cbc:ID>
          <cbc:IssueDate>2026-09-14</cbc:IssueDate>
          <cbc:DocumentCurrencyCode>EUR</cbc:DocumentCurrencyCode>
          <cac:AccountingSupplierParty><cac:Party><cac:PartyName><cbc:Name>ACME Supplier S.L.</cbc:Name></cac:PartyName><cac:PartyTaxScheme><cbc:CompanyID>A00000000</cbc:CompanyID></cac:PartyTaxScheme></cac:Party></cac:AccountingSupplierParty>
          <cac:LegalMonetaryTotal><cbc:PayableAmount currencyID="EUR">1089.00</cbc:PayableAmount></cac:LegalMonetaryTotal>
        </Invoice>
        """;
}
