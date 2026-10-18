using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Documents;

/// JRN-DOC-SET-UP-BRANDED-DOCUMENTS / UC-DOC-ADD-LETTERHEAD / UC-DOC-EDIT-LETTERHEAD:
/// Given document branding settings, when an administrator saves a reusable template and previews it,
/// then the persisted letterhead, styling and safe preview are available for the selected document type.
/// ProductShape: BC-DOCUMENTS, UC-DOC-CONFIGURE-TEMPLATE-SETTINGS, UC-DOC-SAVE-TEXT-TEMPLATE,
/// UC-DOC-STYLE-TEMPLATE, UC-DOC-INSERT-IMAGE.
[Collection(E2ECollection.Name)]
[Trait(E2ETraits.EvidenceMode, E2ETraits.EvidenceModes.Api)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Documents)]
public sealed class DocumentTemplateJourney_Should_ConfigureBranding(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_Save_And_Preview_Branded_Template()
    {
        await RunJourneyAsync(
            "document-template-branding",
            async page =>
            {
                var response = await page.Context.APIRequest.PostAsync(
                    "/api/documents/templates",
                    new()
                    {
                        DataObject = new
                        {
                            name = "E2E invoice",
                            documentType = "invoice",
                            letterhead = "E2E Studio",
                            bodyTemplate = "Invoice {{number}}",
                            footer = "E2E footer",
                            fontFamily = "Inter",
                            accentColor = "#0f766e",
                            logoStorageKey = "branding/e2e.svg",
                            isDefault = true,
                        },
                    }
                );
                response.Status.ShouldBe(201);
                var id = (await response.JsonAsync())!.Value.GetProperty("id").GetGuid();
                var preview = await page.Context.APIRequest.GetAsync(
                    $"/api/documents/templates/{id}/preview"
                );
                preview.Status.ShouldBe(200);
                var body = await preview.TextAsync();
                body.ShouldContain("E2E Studio");
                body.ShouldContain("#0f766e");
                body.ShouldNotContain("<script>");
            }
        );
    }
}
