using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Raccord.EndToEndTests.Projects;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Invoicing;

/// <summary>
/// Product journey JRN-BILL-FINISHED-PROJECT / UC-OBTAIN-INVOICE-DOCUMENT:
/// Given an invoiceable finished project, when the billing administrator opens the issued invoice
/// and chooses its document action, then a non-empty PDF is downloaded.
/// </summary>
[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Invoicing)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
public sealed class InvoiceDocumentJourney_Should_DownloadFromInvoicePage(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_DownloadTheIssuedInvoiceDocumentFromTheUi()
    {
        await RunJourneyAsync(
            "invoice-document-download",
            async page =>
            {
                await CreateCustomerAsync(page);
                await CreateProjectWithBillableLineAsync(page);
                await FinalizeDraftAsync(page);

                var download = await page.RunAndWaitForDownloadAsync(async () =>
                    await page.GetByRole(AriaRole.Link, new() { Name = "Download PDF" })
                        .ClickAsync()
                );

                download.SuggestedFilename.ShouldEndWith(".pdf");
                var path = await download.PathAsync();
                path.ShouldNotBeNullOrWhiteSpace();
                new FileInfo(path!).Length.ShouldBeGreaterThan(0);
            }
        );
    }
}
