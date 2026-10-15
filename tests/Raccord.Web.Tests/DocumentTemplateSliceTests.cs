using System.Net;
using System.Net.Http.Json;
using Raccord.Contracts.Documents;
using Raccord.Web.Tests.Infrastructure;
using Shouldly;

namespace Raccord.Web.Tests;

public sealed class DocumentTemplateSliceTests(PostgresContainer postgres) : IAsyncLifetime
{
    private RaccordApi api = null!;
    private HttpClient client = null!;

    public async ValueTask InitializeAsync()
    {
        api = await RaccordApi.StartAsync(postgres);
        client = api.CreateClient();
    }

    public async ValueTask DisposeAsync()
    {
        client.Dispose();
        await api.DisposeAsync();
    }

    [Fact]
    public async Task Templates_Should_Save_Branding_And_Render_A_Safe_Preview()
    {
        var created = await client.PostAsJsonAsync(
            "/api/documents/templates",
            new CreateDocumentTemplateRequest(
                "Branded invoice",
                "invoice",
                "Raccord Studio",
                "Invoice {{number}}",
                "Thank you",
                "Inter",
                "#123456",
                "branding/logo.svg",
                true,
                "en"
            )
        );
        created.StatusCode.ShouldBe(HttpStatusCode.Created);
        var template = (await created.Content.ReadFromJsonAsync<DocumentTemplateResponse>())!;
        template.IsDefault.ShouldBeTrue();

        var preview = await client.GetFromJsonAsync<DocumentTemplatePreviewResponse>(
            $"/api/documents/templates/{template.Id}/preview"
        );
        preview!.Html.ShouldContain("Raccord Studio");
        preview.Html.ShouldContain("Invoice {{number}}");
        preview.Html.ShouldNotContain("<script>");

        var listed = await client.GetFromJsonAsync<List<DocumentTemplateResponse>>(
            "/api/documents/templates"
        );
        listed!.ShouldContain(item => item.Id == template.Id && item.DocumentType == "invoice");
    }

    [Fact]
    public async Task Templates_Should_Isolate_Defaults_By_Language()
    {
        var english = await client.PostAsJsonAsync(
            "/api/documents/templates",
            new CreateDocumentTemplateRequest(
                "English invoice",
                "invoice",
                "Raccord Studio",
                "Invoice {{number}}",
                "Thank you",
                "Inter",
                "#123456",
                null,
                true
            )
        );
        english.StatusCode.ShouldBe(HttpStatusCode.Created);

        var request = new CreateDocumentTemplateRequest(
            "French invoice",
            "invoice",
            "Studio Raccord",
            "Facture {{number}}",
            "Merci",
            "Inter",
            "#1d4ed8",
            null,
            true,
            "fr-FR"
        );
        var created = await client.PostAsJsonAsync("/api/documents/templates", request);
        created.StatusCode.ShouldBe(HttpStatusCode.Created);
        var template = (await created.Content.ReadFromJsonAsync<DocumentTemplateResponse>())!;
        template.Language.ShouldBe("fr-fr");
        template.IsDefault.ShouldBeTrue();

        var french = await client.GetFromJsonAsync<List<DocumentTemplateResponse>>(
            "/api/documents/templates?language=FR-fr"
        );
        french!.ShouldContain(item => item.Id == template.Id && item.IsDefault);

        var english = await client.GetFromJsonAsync<List<DocumentTemplateResponse>>(
            "/api/documents/templates?language=en"
        );
        english!.ShouldContain(item => item.DocumentType == "invoice" && item.IsDefault);
    }
}
