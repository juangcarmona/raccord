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
    public async Task Built_assets_are_served_as_files_and_client_routes_fall_back_to_the_app()
    {
        using var client = api.WithWebHostBuilder(builder => builder.UseWebRoot(webRoot))
            .CreateClient();

        var script = await client.GetAsync("/assets/app.js");
        var clientRoute = await client.GetAsync("/projects/123");
        var missingAsset = await client.GetAsync("/assets/missing.js");

        Assert.Equal("text/javascript", script.Content.Headers.ContentType?.MediaType);
        Assert.Equal("text/html", clientRoute.Content.Headers.ContentType?.MediaType);
        Assert.Equal(HttpStatusCode.NotFound, missingAsset.StatusCode);
    }
}
