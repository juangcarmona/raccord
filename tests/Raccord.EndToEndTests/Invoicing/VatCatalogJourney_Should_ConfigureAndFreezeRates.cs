using System.Text.Json;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Invoicing;

/// <summary>Configures a VAT rate, applies it to project work and proves the saved rate stays frozen.</summary>
[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Full)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Invoicing)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Projects)]
[Trait(E2ETraits.EvidenceMode, E2ETraits.EvidenceModes.Browser)]
public sealed class VatCatalogJourney_Should_ConfigureAndFreezeRates(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_KeepTheRateUsedBySavedProjectWork()
    {
        await RunJourneyAsync(
            "vat-catalog-project-rate",
            async page =>
            {
                var (vatClassName, vatClassId, schemeName, schemeId) = await ConfigureCatalogAsync(
                    page
                );
                var (vatSchemeId, appliedVatClassId) = await CreateProjectLineAsync(
                    page,
                    vatClassName,
                    schemeName
                );
                vatSchemeId.ShouldBe(schemeId);
                appliedVatClassId.ShouldBe(vatClassId);
                await VerifyUsedRateIsFrozenAsync(page, vatClassId, schemeName, schemeId);
            }
        );
    }

    private async Task<(
        string VatClassName,
        string VatClassId,
        string SchemeName,
        string SchemeId
    )> ConfigureCatalogAsync(IPage page)
    {
        await page.GotoAsync("/settings/vat");
        await page.GetByRole(AriaRole.Heading, new() { Name = "VAT classes and schemes" })
            .WaitForAsync();
        await page.GetByText("General: 21%").WaitForAsync();
        await page.GetByRole(AriaRole.Link, new() { Name = "Official source" }).WaitForAsync();
        await page.GetByText("Initial reference for ES, checked on 2026-10-21:").WaitForAsync();

        var vatClassName = $"Synthetic {RunStamp}";
        await page.GetByLabel("Class name").FillAsync(vatClassName);
        await page.GetByRole(AriaRole.Button, new() { Name = "Add class" }).ClickAsync();
        await page.Locator("section[aria-labelledby='vat-classes-heading']")
            .GetByText(vatClassName, new() { Exact = true })
            .WaitForAsync();
        var vatClassId = await GetVatClassIdAsync(page, vatClassName);

        var schemeName = $"Synthetic rate {RunStamp}";
        await page.GetByLabel("Scheme name").FillAsync(schemeName);
        await page.GetByLabel("Class for scheme").SelectOptionAsync(vatClassId);
        await page.GetByLabel("Rate (%)", new() { Exact = true }).FillAsync("12.5");
        await page.GetByRole(AriaRole.Button, new() { Name = "Add scheme" }).ClickAsync();
        var schemeRow = page.Locator("li").Filter(new() { HasText = schemeName });
        await schemeRow.GetByText($"{vatClassName}: 12.5%").WaitForAsync();
        var scheme = await GetSchemeIdAsync(page, schemeName);
        await schemeRow.GetByRole(AriaRole.Button, new() { Name = "Use as default" }).ClickAsync();
        await page.GetByRole(AriaRole.Status).GetByText("Changes saved.").WaitForAsync();
        await schemeRow.GetByText("Default for new lines").WaitForAsync();
        return (vatClassName, vatClassId, schemeName, scheme);
    }

    private async Task<(string VatSchemeId, string VatClassId)> CreateProjectLineAsync(
        IPage page,
        string vatClassName,
        string schemeName
    )
    {
        await CreateCustomerAsync(page);
        await CreateProjectWithBillableLineAsync(page, vatClassName, schemeName);

        var projectId = new Uri(page.Url).Segments[^1];
        var projectResponse = await page.APIRequest.GetAsync($"/api/projects/{projectId}");
        projectResponse.Status.ShouldBe(200);
        using var project = JsonDocument.Parse(await projectResponse.TextAsync());
        var billableLine = project
            .RootElement.GetProperty("billableLines")
            .EnumerateArray()
            .Single(line =>
                string.Equals(
                    line.GetProperty("description").GetString(),
                    "Camera kit with lenses",
                    StringComparison.Ordinal
                )
            );
        var vatClassId =
            billableLine.GetProperty("vatClassId").GetString()
            ?? throw new InvalidOperationException("The project line had no VAT class identity.");
        billableLine.GetProperty("vatRate").GetDecimal().ShouldBe(12.5m);
        return (
            billableLine.GetProperty("vatSchemeId").GetString()
                ?? throw new InvalidOperationException(
                    "The project line had no VAT scheme identity."
                ),
            vatClassId
        );
    }

    private static async Task VerifyUsedRateIsFrozenAsync(
        IPage page,
        string vatClassId,
        string schemeName,
        string schemeId
    )
    {
        var rejected = await page.APIRequest.PutAsync(
            $"/api/invoicing/vat-schemes/{schemeId}",
            new APIRequestContextOptions
            {
                Data = JsonSerializer.Serialize(
                    new
                    {
                        name = schemeName,
                        fixedPercentage = (decimal?)null,
                        classRates = new[] { new { vatClassId, percentage = 13m } },
                    }
                ),
                Headers = new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    ["content-type"] = "application/json",
                },
            }
        );
        rejected.Status.ShouldBe(422);
        await page.GotoAsync("/settings/vat");
        var schemeRow = page.Locator("li").Filter(new() { HasText = schemeName });
        await schemeRow
            .GetByText(
                "The rate is locked because it has been used. Create another scheme for new work."
            )
            .WaitForAsync();

        var auditResponse = await page.APIRequest.GetAsync(
            $"/api/audit-events?entityId={schemeId}"
        );
        auditResponse.Status.ShouldBe(200);
        using var audit = JsonDocument.Parse(await auditResponse.TextAsync());
        var actions = audit
            .RootElement.EnumerateArray()
            .Select(item => item.GetProperty("action").GetString())
            .ToArray();
        actions.ShouldContain("vat-scheme.created");
        actions.ShouldContain("vat-scheme.default-set");
        actions.ShouldNotContain("vat-scheme.updated");
    }

    private static async Task<string> GetSchemeIdAsync(IPage page, string schemeName)
    {
        var response = await page.APIRequest.GetAsync("/api/invoicing/vat-schemes");
        response.Status.ShouldBe(200);
        using var schemes = JsonDocument.Parse(await response.TextAsync());
        return schemes
                .RootElement.EnumerateArray()
                .Single(scheme =>
                    string.Equals(
                        scheme.GetProperty("name").GetString(),
                        schemeName,
                        StringComparison.Ordinal
                    )
                )
                .GetProperty("id")
                .GetString()
            ?? throw new InvalidOperationException("The created VAT scheme had no identity.");
    }

    private static async Task<string> GetVatClassIdAsync(IPage page, string vatClassName)
    {
        var response = await page.APIRequest.GetAsync("/api/invoicing/vat-classes");
        response.Status.ShouldBe(200);
        using var classes = JsonDocument.Parse(await response.TextAsync());
        return classes
                .RootElement.EnumerateArray()
                .Single(vatClass =>
                    string.Equals(
                        vatClass.GetProperty("name").GetString(),
                        vatClassName,
                        StringComparison.Ordinal
                    )
                )
                .GetProperty("id")
                .GetString()
            ?? throw new InvalidOperationException("The created VAT class had no identity.");
    }
}
