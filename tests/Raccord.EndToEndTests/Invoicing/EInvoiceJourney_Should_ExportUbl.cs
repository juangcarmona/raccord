using System.Net;
using System.Text;
using System.Xml.Linq;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Invoicing;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Invoicing)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Compliance)]
public sealed class EInvoiceJourney_Should_ExportUbl(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    /// <summary>
    /// Given an issued invoice with validated parties and lines, when the operator exports the
    /// supported structured format, then a UBL 2.1 document preserves the auditable totals.
    /// ProductShape: FR-B2B-E-INVOICING, UC-FIN-CONFIGURE-E-INVOICING, UC-FIN-EXPORT-E-INVOICE.
    /// </summary>
    [Fact]
    public async Task Journey_Should_ExportAnAuditableUblInvoice()
    {
        await RunJourneyAsync(
            "invoice-einvoice-ubl",
            async page =>
            {
                await CreateCustomerAsync(page);
                await CreateProjectWithBillableLineAsync(page);
                await FinalizeDraftAsync(page, DateOnly.FromDateTime(DateTime.UtcNow));

                var invoiceId = new Uri(page.Url).Segments[^1];
                var response = await page.APIRequest.GetAsync(
                    $"/api/invoices/{invoiceId}/e-invoice?format=ubl"
                );
                response.Ok.ShouldBeTrue();
                response.Headers["content-type"].ShouldStartWith("application/xml");
                response.Headers["content-disposition"].ShouldContain(".xml");

                var xml = await response.BodyAsync();
                var document = XDocument.Parse(Encoding.UTF8.GetString(xml));
                document.Root!.Name.LocalName.ShouldBe("Invoice");
                document.Root.Name.NamespaceName.ShouldContain("Invoice-2");
                document
                    .Descendants()
                    .Where(item =>
                        string.Equals(item.Name.LocalName, "CompanyID", StringComparison.Ordinal)
                    )
                    .Select(item => item.Value)
                    .ShouldBe(["B00000000", "B87654321"]);
                document
                    .Descendants()
                    .Count(item =>
                        string.Equals(item.Name.LocalName, "InvoiceLine", StringComparison.Ordinal)
                    )
                    .ShouldBe(1);
                document
                    .Descendants()
                    .Single(item =>
                        string.Equals(
                            item.Name.LocalName,
                            "PayableAmount",
                            StringComparison.Ordinal
                        )
                    )
                    .Value.ShouldBe("1089.00");

                await page.SetContentAsync(
                    $"<h1>UBL e-invoice</h1><pre>{WebUtility.HtmlEncode(Encoding.UTF8.GetString(xml))}</pre>"
                );
                await page.ScreenshotAsync(
                    new PageScreenshotOptions
                    {
                        Path = "docs/product/design/evidence/issue-165/ubl-einvoice.png",
                        FullPage = true,
                    }
                );
            }
        );
    }
}
