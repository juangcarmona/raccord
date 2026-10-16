using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Raccord.Web.Tests.Infrastructure;

namespace Raccord.Web.Tests;

public sealed class FrontendHostingTests(PostgresContainer postgres) : IAsyncLifetime
{
    private readonly string webRoot = Path.Combine(
        Path.GetTempPath(),
        "raccord-webroot",
        Guid.NewGuid().ToString("N")
    );
    private RaccordApi api = null!;

    public async ValueTask InitializeAsync()
    {
        Directory.CreateDirectory(Path.Combine(webRoot, "assets"));
        await File.WriteAllTextAsync(
            Path.Combine(webRoot, "index.html"),
            "<!doctype html><div id=\"root\"></div>"
        );
        await File.WriteAllTextAsync(
            Path.Combine(webRoot, "assets", "app.js"),
            "console.log('raccord');"
        );
        api = await RaccordApi.StartAsync(postgres);
    }

    public async ValueTask DisposeAsync()
    {
        await api.DisposeAsync();
        Directory.Delete(webRoot, recursive: true);
    }

    [Fact]
    public async Task Built_assets_Should_be_served_as_files_and_client_routes_fall_back_to_the_app()
    {
        using var client = api.WithWebHostBuilder(builder => builder.UseWebRoot(webRoot))
            .CreateClient();

        var script = await client.GetAsync("/assets/app.js");
        var clientRoute = await client.GetAsync("/projects/123");
        var missingAsset = await client.GetAsync("/assets/missing.js");

        script.Content.Headers.ContentType?.MediaType.ShouldBe("text/javascript");
        clientRoute.Content.Headers.ContentType?.MediaType.ShouldBe("text/html");
        missingAsset.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Responses_Should_include_browser_security_headers()
    {
        using var client = api.WithWebHostBuilder(builder => builder.UseWebRoot(webRoot))
            .CreateClient();

        var response = await client.GetAsync("/projects/123");

        response
            .Headers.GetValues("Content-Security-Policy")
            .Single()
            .ShouldStartWith("default-src 'self'");
        response
            .Headers.GetValues("Cross-Origin-Embedder-Policy")
            .Single()
            .ShouldBe("require-corp");
        response.Headers.GetValues("Cross-Origin-Opener-Policy").Single().ShouldBe("same-origin");
        response.Headers.GetValues("Cross-Origin-Resource-Policy").Single().ShouldBe("same-origin");
        response
            .Headers.GetValues("Permissions-Policy")
            .ShouldBe("camera=(), geolocation=(), microphone=()");
        response.Headers.GetValues("Referrer-Policy").ShouldBe("strict-origin-when-cross-origin");
        response.Headers.GetValues("X-Content-Type-Options").Single().ShouldBe("nosniff");
        response.Headers.GetValues("X-Frame-Options").Single().ShouldBe("DENY");
    }
}
