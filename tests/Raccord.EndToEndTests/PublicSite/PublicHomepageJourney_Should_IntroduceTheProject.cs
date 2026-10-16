using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.PublicSite;

/// <summary>
/// Checks the Spanish public homepage as a visitor, including its stated first-slice workflow,
/// maturity boundaries, public documentation links and responsive keyboard navigation.
/// ProductShape: ACT-OPERATIONS-COORDINATOR, ACT-BILLING-ADMINISTRATOR,
/// JRN-BILL-FINISHED-PROJECT, JRN-SET-UP-INVOICING.
/// </summary>
[Trait(E2ETraits.Area, E2ETraits.Areas.Invoicing)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
public sealed class PublicHomepageJourney_Should_IntroduceTheProject
{
    [Fact]
    public async Task Journey_Should_ExplainPurposeMaturityAndSafeAdoption()
    {
        const string artifactName = "public-homepage";
        var (playwright, browser) = await LaunchBrowserAsync();
        using (playwright)
        await using (browser)
        {
            await RunHomepageJourneyAsync(browser, artifactName);
        }
    }

    private static async Task<(IPlaywright Playwright, IBrowser Browser)> LaunchBrowserAsync()
    {
        var installExitCode = await Task.Run(() =>
            Microsoft.Playwright.Program.Main(["install", "chromium"])
        );
        installExitCode.ShouldBe(0);
        var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(
            new BrowserTypeLaunchOptions { Headless = true }
        );
        return (playwright, browser);
    }

    private static async Task RunHomepageJourneyAsync(IBrowser browser, string artifactName)
    {
        var evidenceDirectory = PlaywrightArtifacts.EvidenceDirectory;
        var shouldCaptureEvidence = PlaywrightArtifacts.VisualEvidenceEnabled;
        System.IO.Directory.CreateDirectory(evidenceDirectory);
        var context = await browser.NewContextAsync(
            new BrowserNewContextOptions
            {
                Locale = "es-ES",
                RecordVideoDir = shouldCaptureEvidence ? evidenceDirectory : null,
            }
        );
        var passed = false;
        var page = await context.NewPageAsync();

        try
        {
            await context.Tracing.StartAsync(
                new TracingStartOptions
                {
                    Screenshots = true,
                    Snapshots = true,
                    Sources = true,
                }
            );
            await OpenHomepageAsync(page);
            await AssertHomepageContentAsync(page);
            await AssertMobileKeyboardNavigationAsync(page);
            if (shouldCaptureEvidence)
            {
                await page.ScreenshotAsync(
                    new()
                    {
                        Path = Path.Combine(evidenceDirectory, "public-homepage-mobile.png"),
                        FullPage = true,
                    }
                );
            }
            await AssertDesktopLayoutAsync(page);
            if (shouldCaptureEvidence)
            {
                await PlaywrightArtifacts.CaptureSuccessAsync(page, artifactName);
            }
            await PlaywrightArtifacts.SaveTraceAsync(context, artifactName);
            passed = true;
        }
        catch
        {
            await PlaywrightArtifacts.CaptureAsync(context, page, artifactName);
            throw;
        }
        finally
        {
            await context.CloseAsync();
            if (shouldCaptureEvidence)
            {
                await PlaywrightArtifacts.SaveVideoAsync(page, artifactName);
            }
            PlaywrightArtifacts.WriteJourneyResult(artifactName, passed ? "passed" : "failed");
        }
    }

    private static async Task OpenHomepageAsync(IPage page)
    {
        var homepagePath = Path.Combine(AppContext.BaseDirectory, "public-site", "index.html");
        await page.GotoAsync(new Uri(homepagePath).AbsoluteUri);
    }

    private static async Task AssertHomepageContentAsync(IPage page)
    {
        (await page.TitleAsync()).ShouldBe("Raccord — Operaciones y facturación audiovisual");
        (await page.Locator("html").GetAttributeAsync("lang")).ShouldBe("es");
        (
            await page.GetByRole(AriaRole.Heading, new() { Level = 1 }).InnerTextAsync()
        ).ShouldContain("Del trabajo de producción a la factura");
        var content = await page.GetByRole(AriaRole.Main).InnerTextAsync();
        content.ShouldContain("productoras audiovisuales");
        content.ShouldContain("empresas de alquiler de equipos");
        content.ShouldContain("Ya puedes llevar un proyecto hasta su factura");
        content.ShouldContain("Registra un cliente");
        content.ShouldContain("trabajo facturable");
        content.ShouldContain("descarga su PDF");
        content.ShouldContain("iniciar sesión");
        content.ShouldContain("No hay registro abierto ni una demo alojada");
        content.ShouldContain("no afirma cumplir requisitos legales o fiscales");
        (
            await page.GetByRole(AriaRole.Link, new() { Name = "Leer la guía de autoalojamiento" })
                .GetAttributeAsync("href")
        ).ShouldBe(
            "https://github.com/juangcarmona/raccord/blob/main/docs/operations/self-hosting.md"
        );
        (
            await page.GetByRole(AriaRole.Link, new() { Name = "Explorar lo que ya funciona" })
                .GetAttributeAsync("href")
        ).ShouldBe("#estado");
        (await page.GetByRole(AriaRole.Combobox).CountAsync()).ShouldBe(0);
        foreach (
            var href in await page.Locator("a")
                .EvaluateAllAsync<string[]>("links => links.map(link => link.getAttribute('href'))")
        )
        {
            (
                href.StartsWith('#')
                || href.StartsWith(
                    "https://github.com/juangcarmona/raccord",
                    StringComparison.Ordinal
                )
            ).ShouldBeTrue();
        }
    }

    private static async Task AssertMobileKeyboardNavigationAsync(IPage page)
    {
        await page.SetViewportSizeAsync(375, 812);
        (
            await page.EvaluateAsync<bool>(
                "() => document.documentElement.scrollWidth > window.innerWidth"
            )
        ).ShouldBeFalse();
        await page.Keyboard.PressAsync("Tab");
        (
            await page.EvaluateAsync<string>(
                "() => document.activeElement?.getAttribute('href') ?? ''"
            )
        ).ShouldBe("#contenido");
    }

    private static async Task AssertDesktopLayoutAsync(IPage page)
    {
        await page.SetViewportSizeAsync(1440, 900);
        await page.Locator("#hero-title").ClickAsync();
        (
            await page.EvaluateAsync<bool>(
                "() => document.documentElement.scrollWidth > window.innerWidth"
            )
        ).ShouldBeFalse();
    }
}
