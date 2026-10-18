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
            await AssertGuideCardLayoutAsync(page);
            await AssertMobileKeyboardNavigationAsync(page);
            if (shouldCaptureEvidence)
            {
                await CaptureMobileScreenshotAsync(page, evidenceDirectory);
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
        var homepagePath = Path.Combine(
            AppContext.BaseDirectory,
            "public-site",
            "es",
            "index.html"
        );
        await page.GotoAsync(new Uri(homepagePath).AbsoluteUri);
    }

    private static async Task AssertHomepageContentAsync(IPage page)
    {
        (await page.TitleAsync()).ShouldBe(
            "Raccord — Para productoras audiovisuales y empresas de alquiler"
        );
        (await page.Locator("html").GetAttributeAsync("lang")).ShouldBe("es");
        (await page.GetByRole(AriaRole.Heading, new() { Level = 1 }).InnerTextAsync()).ShouldBe(
            "Tu producción y tus facturas, bajo tu control."
        );
        var content = await page.GetByRole(AriaRole.Main).InnerTextAsync();
        content.ShouldContain("productoras audiovisuales");
        content.ShouldContain("empresas de alquiler");
        content.ShouldContain("De un trabajo a su factura");
        content.ShouldContain("Registra el cliente y el proyecto");
        content.ShouldContain("trabajo facturable");
        content.ShouldContain("Emite la factura y descarga su PDF");
        content.ShouldContain("El MVP está implementado");
        content.ShouldContain("No ofrecemos una instancia alojada ni un registro público");
        content.ShouldContain("no afirma cumplir requisitos legales o fiscales");
        (
            await page.Locator(".product-shot img")
                .EvaluateAsync<bool>("image => image.complete && image.naturalWidth > 0")
        ).ShouldBeTrue();
        (
            await page.GetByRole(AriaRole.Link, new() { Name = "Ver el proyecto" }).CountAsync()
        ).ShouldBe(1);
        (
            await page.GetByRole(AriaRole.Link, new() { Name = "Aprender a instalarlo" })
                .GetAttributeAsync("href")
        ).ShouldBe("manuales/");
        (
            await page.GetByRole(AriaRole.Link, new() { Name = "Explorar el producto" })
                .GetAttributeAsync("href")
        ).ShouldBe("producto/");
        (await page.GetByRole(AriaRole.Combobox).CountAsync()).ShouldBe(0);
        foreach (
            var href in await page.Locator("a")
                .EvaluateAllAsync<string[]>("links => links.map(link => link.getAttribute('href'))")
        )
        {
            (
                href.StartsWith('#')
                || href.StartsWith("/images/", StringComparison.Ordinal)
                || href.StartsWith("../", StringComparison.Ordinal)
                || string.Equals(href, "index.html", StringComparison.Ordinal)
                || href.StartsWith("producto/", StringComparison.Ordinal)
                || href.StartsWith("manuales/", StringComparison.Ordinal)
                || href.StartsWith("autoalojamiento/", StringComparison.Ordinal)
                || href.StartsWith("arquitectura/", StringComparison.Ordinal)
                || href.StartsWith(
                    "https://github.com/juangcarmona/raccord",
                    StringComparison.Ordinal
                )
            ).ShouldBeTrue($"Unexpected homepage link: {href}");
        }
        (
            await page.Locator(".language-switcher")
                .GetByRole(AriaRole.Link, new() { Name = "EN", Exact = true })
                .GetAttributeAsync("href")
        ).ShouldBe("../en/");
        await AssertEnglishLocaleAsync(page);
    }

    private static async Task AssertEnglishLocaleAsync(IPage page)
    {
        var englishPath = Path.Combine(AppContext.BaseDirectory, "public-site", "en", "index.html");
        await page.GotoAsync(new Uri(englishPath).AbsoluteUri);
        (await page.Locator("html").GetAttributeAsync("lang")).ShouldBe("en");
        (await page.TitleAsync()).ShouldBe(
            "Raccord — For audiovisual production and equipment-rental teams"
        );
        var content = await page.GetByRole(AriaRole.Main).InnerTextAsync();
        content.ShouldContain("The MVP is implemented");
        content.ShouldContain("There is no hosted instance or public sign-up");
        content.ShouldContain("Raccord does not claim legal or tax compliance");
        (
            await page.Locator(".product-shot img")
                .EvaluateAsync<bool>("image => image.complete && image.naturalWidth > 0")
        ).ShouldBeTrue();
        await page.SetViewportSizeAsync(375, 812);
        (
            await page.EvaluateAsync<bool>(
                "() => document.documentElement.scrollWidth > window.innerWidth"
            )
        ).ShouldBeFalse();
        (
            await page.Locator(".language-switcher")
                .GetByRole(AriaRole.Link, new() { Name = "ES", Exact = true })
                .GetAttributeAsync("href")
        ).ShouldBe("../es/");
        await OpenHomepageAsync(page);
    }

    private static async Task CaptureMobileScreenshotAsync(IPage page, string evidenceDirectory)
    {
        await page.Locator(".product-shot img").EvaluateAsync("image => image.decode()");
        await page.EvaluateAsync("() => document.activeElement?.blur()");
        await page.ScreenshotAsync(
            new()
            {
                Path = Path.Combine(evidenceDirectory, "public-homepage-mobile.png"),
                FullPage = true,
            }
        );
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

    private static async Task AssertGuideCardLayoutAsync(IPage page)
    {
        var guideGrid = page.Locator(".section-manuals .guide-grid");
        (await guideGrid.CountAsync()).ShouldBe(1);
        (await guideGrid.Locator(":scope > .guide-card").CountAsync()).ShouldBe(3);

        await page.SetViewportSizeAsync(1440, 900);
        var desktopColumns = await guideGrid.EvaluateAsync<string>(
            "element => getComputedStyle(element).gridTemplateColumns"
        );
        desktopColumns.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length.ShouldBe(2);

        await page.SetViewportSizeAsync(375, 812);
        var mobileColumns = await guideGrid.EvaluateAsync<string>(
            "element => getComputedStyle(element).gridTemplateColumns"
        );
        mobileColumns.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length.ShouldBe(1);
        (
            await page.EvaluateAsync<bool>(
                "() => document.documentElement.scrollWidth > window.innerWidth"
            )
        ).ShouldBeFalse();
    }

    private static async Task AssertDesktopLayoutAsync(IPage page)
    {
        await page.SetViewportSizeAsync(1440, 900);
        (await page.GetByRole(AriaRole.Heading, new() { Level = 1 }).CountAsync()).ShouldBe(1);
        (
            await page.EvaluateAsync<bool>(
                "() => document.documentElement.scrollWidth > window.innerWidth"
            )
        ).ShouldBeFalse();
    }
}
