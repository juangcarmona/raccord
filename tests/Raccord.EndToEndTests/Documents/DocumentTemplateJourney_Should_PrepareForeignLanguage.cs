using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Documents;

/// JRN-DOC-SERVE-FOREIGN-CUSTOMER / UC-DOC-PREPARE-FOREIGN-LANGUAGE-TEMPLATE:
/// Given a customer language, when an administrator saves and selects a localized template,
/// then the matching language variant is listed and previewed without replacing another locale.
/// ProductShape: BC-DOCUMENTS, UC-DOC-CONFIGURE-TEMPLATE-SETTINGS, UC-DOC-SAVE-TEXT-TEMPLATE.
[Collection(E2ECollection.Name)]
[Trait(E2ETraits.EvidenceMode, E2ETraits.EvidenceModes.Api)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Documents)]
public sealed class DocumentTemplateJourney_Should_PrepareForeignLanguage(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_Save_And_Select_French_Template()
    {
        await RunJourneyAsync(
            "document-template-foreign-language",
            async page =>
            {
                var response = await page.Context.APIRequest.PostAsync(
                    "/api/documents/templates",
                    new()
                    {
                        DataObject = new
                        {
                            name = "E2E French invoice",
                            documentType = "invoice",
                            language = "fr-FR",
                            letterhead = "Studio Raccord",
                            bodyTemplate = "Facture {{number}}",
                            footer = "Merci",
                            fontFamily = "Inter",
                            accentColor = "#1d4ed8",
                            isDefault = true,
                        },
                    }
                );
                response.Status.ShouldBe(201);
                var id = (await response.JsonAsync())!.Value.GetProperty("id").GetGuid();
                var localized = await page.Context.APIRequest.GetAsync(
                    "/api/documents/templates?language=fr-FR"
                );
                localized.Status.ShouldBe(200);
                (await localized.TextAsync()).ShouldContain("E2E French invoice");
                var preview = await page.Context.APIRequest.GetAsync(
                    $"/api/documents/templates/{id}/preview"
                );
                preview.Status.ShouldBe(200);
                (await preview.TextAsync()).ShouldContain("Facture");
            }
        );
    }
}
