using System.Text.Json;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Shouldly;

namespace Raccord.EndToEndTests.Integrations;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.EvidenceMode, E2ETraits.EvidenceModes.Api)]
/// <summary>
/// Given an administrator issues a scoped integration token, when external requests read and write
/// project records and the token is regenerated, then only the granted scope remains usable and the
/// previous credential is rejected.
/// ProductShape: BC-WORKSPACE, JRN-INT-BUILD-API-INTEGRATION, UC-INT-ENABLE-API-ACCESS, UC-INT-READ-RECORDS-THROUGH-API, UC-INT-WRITE-RECORDS-THROUGH-API, UC-INT-REGENERATE-API-TOKEN.
/// </summary>
[Trait(E2ETraits.Area, E2ETraits.Areas.Workspace)]
public sealed class ApiAccessJourney_Should_EnforceScopes(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    private static readonly string[] ReadScopes = ["projects.read"];

    [Fact]
    public Task Journey_Should_IssueRotateAndEnforceAProjectReadToken() =>
        RunJourneyAsync(
            "api-access",
            async page =>
            {
                var (tokenId, token) = await IssueReadTokenAsync(page);
                await AssertReadScopeAsync(page, token);
                await AssertWriteIsRejectedAsync(page, token);
                var rotatedToken = await RotateAsync(page, tokenId);
                await AssertOldTokenIsRejectedAsync(page, token, rotatedToken);
            }
        );

    private async Task<(Guid Id, string Token)> IssueReadTokenAsync(IPage page)
    {
        var response = await page.APIRequest.PostAsync(
            "/api/integrations/api-tokens/",
            new APIRequestContextOptions
            {
                DataObject = new { name = $"Project reader {RunStamp}", scopes = ReadScopes },
            }
        );
        response.Status.ShouldBe(201);
        using var json = JsonDocument.Parse(await response.TextAsync());
        var token = json.RootElement.GetProperty("token").GetString();
        token.ShouldNotBeNullOrWhiteSpace();
        return (json.RootElement.GetProperty("id").GetGuid(), token!);
    }

    private static async Task AssertReadScopeAsync(IPage page, string token)
    {
        var response = await page.APIRequest.GetAsync(
            "/api/integrations/projects",
            WithToken(token)
        );
        response.Status.ShouldBe(200);
    }

    private static async Task AssertWriteIsRejectedAsync(IPage page, string token)
    {
        var response = await page.APIRequest.PostAsync(
            "/api/integrations/projects",
            new APIRequestContextOptions
            {
                Headers = WithToken(token).Headers,
                DataObject = new
                {
                    name = "Rejected external write",
                    customerId = Guid.NewGuid(),
                    startDate = "2026-09-14",
                    endDate = "2026-09-18",
                },
            }
        );
        response.Status.ShouldBe(401);
    }

    private static async Task<string> RotateAsync(IPage page, Guid tokenId)
    {
        var response = await page.APIRequest.PostAsync(
            $"/api/integrations/api-tokens/{tokenId}/regenerate",
            new APIRequestContextOptions()
        );
        response.Status.ShouldBe(200);
        using var json = JsonDocument.Parse(await response.TextAsync());
        var token = json.RootElement.GetProperty("token").GetString();
        token.ShouldNotBeNullOrWhiteSpace();
        return token!;
    }

    private static async Task AssertOldTokenIsRejectedAsync(
        IPage page,
        string oldToken,
        string newToken
    )
    {
        var oldResponse = await page.APIRequest.GetAsync(
            "/api/integrations/projects",
            WithToken(oldToken)
        );
        oldResponse.Status.ShouldBe(401);
        var newResponse = await page.APIRequest.GetAsync(
            "/api/integrations/projects",
            WithToken(newToken)
        );
        newResponse.Status.ShouldBe(200);
    }

    private static APIRequestContextOptions WithToken(string token) =>
        new()
        {
            Headers = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["X-Api-Key"] = token,
            },
        };
}
