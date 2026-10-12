using System.Text.Json;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Raccord.BuildingBlocks.Api;

namespace Raccord.BuildingBlocks.Tests.Api;

public sealed class ApiResultsTests
{
    [Fact]
    public async Task ApiResults_Should_IncludeAllValidationIssues()
    {
        var result = ApiResults.ToProblem([
            Error.Validation("name.required", "Name is required."),
            Error.Validation("email.invalid", "Email is invalid."),
        ]);
        var context = new DefaultHttpContext();
        context.RequestServices = new ServiceCollection().AddLogging().BuildServiceProvider();
        await using var body = new MemoryStream();
        context.Response.Body = body;

        await result.ExecuteAsync(context);
        body.Position = 0;
        using var document = await JsonDocument.ParseAsync(body);
        context.Response.StatusCode.ShouldBe(StatusCodes.Status422UnprocessableEntity);
        document.RootElement.GetProperty("code").GetString().ShouldBe("name.required");
        document.RootElement.GetProperty("issues").GetArrayLength().ShouldBe(2);
        document
            .RootElement.GetProperty("issues")[0]
            .GetProperty("code")
            .GetString()
            .ShouldBe("name.required");
    }

    [Fact]
    public async Task ApiResults_Should_UseSafeDetailForUnexpectedFailure()
    {
        var result = ApiResults.ToProblem([Error.Unexpected("boom", "secret details")]);
        var context = new DefaultHttpContext();
        context.RequestServices = new ServiceCollection().AddLogging().BuildServiceProvider();
        await using var body = new MemoryStream();
        context.Response.Body = body;

        await result.ExecuteAsync(context);
        body.Position = 0;
        using var document = await JsonDocument.ParseAsync(body);
        document.RootElement.GetProperty("status").GetInt32().ShouldBe(500);
        (document.RootElement.GetProperty("detail").GetString() ?? string.Empty).ShouldNotContain(
            "secret"
        );
    }
}
