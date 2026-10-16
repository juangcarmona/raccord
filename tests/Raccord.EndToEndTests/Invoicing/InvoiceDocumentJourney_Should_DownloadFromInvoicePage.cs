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

                if (PlaywrightArtifacts.VisualEvidenceEnabled)
                {
                    System.IO.Directory.CreateDirectory(PlaywrightArtifacts.EvidenceDirectory);
                    await MaskEmailTextAsync(page);
                    await page.ScreenshotAsync(
                        new PageScreenshotOptions
                        {
                            Path = Path.Combine(
                                PlaywrightArtifacts.EvidenceDirectory,
                                "invoice-publication.png"
                            ),
                            FullPage = true,
                            Mask =
                            [
                                page.Locator("[data-public-mask='email']"),
                                page.GetByText(
                                    $"Productora E2E {RunStamp}",
                                    new() { Exact = true }
                                ),
                                page.GetByText("Tax ID: B87654321", new() { Exact = true }),
                            ],
                            MaskColor = "#e5e7eb",
                        }
                    );
                }
            }
        );
    }

    private static async Task MaskEmailTextAsync(IPage page)
    {
        await page.Locator("body")
            .EvaluateAsync(
                "body => { const walker = document.createTreeWalker(body, NodeFilter.SHOW_TEXT); "
                    + "const nodes = []; let node; while ((node = walker.nextNode())) "
                    + "if (node.nodeValue.includes('admin@example.com')) nodes.push(node); "
                    + "for (const textNode of nodes) { const parts = textNode.nodeValue.split('admin@example.com'); "
                    + "const fragment = document.createDocumentFragment(); parts.forEach((part, index) => { "
                    + "if (part) fragment.append(document.createTextNode(part)); if (index < parts.length - 1) { "
                    + "const mask = document.createElement('span'); mask.dataset.publicMask = 'email'; "
                    + "mask.textContent = 'admin@example.com'; fragment.append(mask); } }); "
                    + "textNode.replaceWith(fragment); } }"
            );
    }
}
