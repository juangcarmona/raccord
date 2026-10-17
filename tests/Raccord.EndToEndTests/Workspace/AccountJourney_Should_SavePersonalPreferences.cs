using System.Text.Json;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Xunit;

namespace Raccord.EndToEndTests.Workspace;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Workspace)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
public sealed class AccountJourney_Should_SavePersonalPreferences(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_SaveProfileAndRestorePreferredLanguageAfterSignIn()
    {
        await RunJourneyAsync(
            "account-profile-preferences",
            async page =>
            {
                await page.GetByRole(AriaRole.Link, new() { Name = "My account" }).ClickAsync();
                await page.GetByRole(AriaRole.Heading, new() { Name = "My account" })
                    .WaitForAsync();
                await page.GetByLabel("Display name").FillAsync("Production Admin");
                await page.GetByLabel("Preferred language").SelectOptionAsync("es");
                await page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
                await page.GetByText("Preferencias guardadas.").WaitForAsync();
                await page.GetByRole(AriaRole.Link, new() { Name = "Administrar roles y permisos" })
                    .WaitForAsync();
                await CaptureResponsiveEvidenceAsync(page, "design-system-app-es");

                await page.ReloadAsync();
                await page.GetByLabel("Nombre para mostrar").WaitForAsync();
                Assert.Equal(
                    "Production Admin",
                    await page.GetByLabel("Nombre para mostrar").InputValueAsync()
                );
                Assert.Equal("es", await page.GetByLabel("Idioma preferido").InputValueAsync());
                await page.GetByRole(AriaRole.Heading, new() { Name = "Mi cuenta" }).WaitForAsync();

                await page.Locator(".shell__toolbar button").ClickAsync();
                await page.WaitForURLAsync(url =>
                    url.EndsWith("/sign-in", StringComparison.Ordinal)
                );
                await page.GetByLabel("Correo electrónico").FillAsync("admin@example.com");
                await page.GetByLabel("Contraseña").FillAsync("DemoPassword123!");
                await page.GetByRole(AriaRole.Button, new() { Name = "Iniciar sesión" })
                    .ClickAsync();
                await page.WaitForURLAsync(url =>
                    url.EndsWith("/account", StringComparison.Ordinal)
                );
                await page.GetByRole(AriaRole.Heading, new() { Name = "Mi cuenta" }).WaitForAsync();

                var current = await page.EvaluateAsync<string>(
                    "async () => JSON.stringify(await (await fetch('/api/auth/me')).json())"
                );
                using var currentDocument = JsonDocument.Parse(current);
                Assert.Equal(
                    "Production Admin",
                    currentDocument.RootElement.GetProperty("displayName").GetString()
                );
                Assert.Equal(
                    "es",
                    currentDocument.RootElement.GetProperty("preferredLanguage").GetString()
                );

                await page.GetByLabel("Idioma preferido").SelectOptionAsync("en");
                await page.GetByRole(AriaRole.Button, new() { Name = "Guardar" }).ClickAsync();
                await page.GetByText("Preferences saved.").WaitForAsync();
                await page.GetByRole(AriaRole.Heading, new() { Name = "My account" })
                    .WaitForAsync();
                await CaptureResponsiveEvidenceAsync(page, "design-system-app-en");
            }
        );
    }

    private static async Task CaptureResponsiveEvidenceAsync(IPage page, string name)
    {
        Directory.CreateDirectory(PlaywrightArtifacts.EvidenceDirectory);
        await page.EvaluateAsync("() => document.activeElement?.blur()");
        await page.SetViewportSizeAsync(1440, 900);
        await page.ScreenshotAsync(
            new()
            {
                Path = Path.Combine(PlaywrightArtifacts.EvidenceDirectory, $"{name}-desktop.png"),
                FullPage = true,
            }
        );
        await page.SetViewportSizeAsync(375, 812);
        Assert.False(
            await page.EvaluateAsync<bool>(
                "() => document.documentElement.scrollWidth > window.innerWidth"
            ),
            $"The {name} account page overflows at a narrow viewport."
        );
        await page.Keyboard.PressAsync("Tab");
        Assert.NotEqual(
            "none",
            await page.EvaluateAsync<string>(
                "() => getComputedStyle(document.activeElement).outlineStyle"
            )
        );
        await page.ScreenshotAsync(
            new()
            {
                Path = Path.Combine(PlaywrightArtifacts.EvidenceDirectory, $"{name}-mobile.png"),
                FullPage = true,
            }
        );
    }
}
