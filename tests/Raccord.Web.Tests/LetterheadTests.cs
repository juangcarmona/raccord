using System.Net.Http.Headers;
using System.Net.Http.Json;
using Raccord.Contracts.Documents;
using Raccord.Web.Tests.Infrastructure;

namespace Raccord.Web.Tests;

public sealed class LetterheadTests(PostgresContainer postgres) : IAsyncLifetime
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
    public async Task Letterhead_Should_Store_Pdf_Metadata_And_Select_Default()
    {
        using var form = new MultipartFormDataContent();
        form.Add(new StringContent("A4 stationery"), "name");
        form.Add(new StringContent("Invoice,Quote"), "documentTypes");
        form.Add(new StringContent("210"), "pageWidthMillimetres");
        form.Add(new StringContent("297"), "pageHeightMillimetres");
        form.Add(new StringContent("12"), "marginTopMillimetres");
        form.Add(new StringContent("10"), "marginRightMillimetres");
        form.Add(new StringContent("12"), "marginBottomMillimetres");
        form.Add(new StringContent("10"), "marginLeftMillimetres");
        using var pdf = new ByteArrayContent("%PDF-1.7"u8.ToArray());
        pdf.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
        form.Add(pdf, "file", "letterhead.pdf");

        var created = await (await client.PostAsync("/api/documents/letterheads", form))
            .EnsureSuccessStatusCode()
            .Content.ReadFromJsonAsync<LetterheadSnapshot>();
        created.ShouldNotBeNull();
        created!.IsDefault.ShouldBeTrue();
        created.StorageKey.ShouldStartWith("letterheads/");

        var listed = await client.GetFromJsonAsync<LetterheadSnapshot[]>(
            "/api/documents/letterheads"
        );
        listed.ShouldNotBeNull();
        listed!.ShouldContain(item => item.Id == created.Id);
    }
}
