using System.Text.Json;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Workspace;

[Collection(IssuerIdentityChangeE2ECollection.Name)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Workspace)]
public sealed class IssuerIdentityChangeJourney_Should_RequireSecondReview(
    DistributedAppFixture app
) : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_KeepIdentityPendingUntilAnotherAdministratorApproves()
    {
        var reviewer = await App.CreateSecondAdministratorAsync();
        await RunJourneyAsync(
            "issuer-identity-second-review",
            async page =>
            {
                var invoiceId = await SubmitIdentityChangeAsync(page);
                await ReviewIdentityChangeAsync(page, reviewer, invoiceId);
            }
        );
    }

    private async Task<string> SubmitIdentityChangeAsync(IPage page)
    {
        await CreateCustomerAsync(page);
        await CreateProjectWithBillableLineAsync(page);
        await FinalizeDraftAsync(page);
        var invoiceId = new Uri(page.Url).Segments[^1];
        await page.GotoAsync("/settings/issuer");
        await page.GetByRole(AriaRole.Heading, new() { Name = "Issuing company" }).WaitForAsync();
        await page.GetByLabel("Legal name").FillAsync($"Raccord Media {RunStamp}");
        await page.GetByLabel("Tax ID").FillAsync("B12345678");
        await page.GetByLabel("Reason for legal identity change")
            .FillAsync("The company updated its registered identity.");
        await page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
        await page.GetByText(
                "Change sent for review. The current identity remains active until approved."
            )
            .WaitForAsync();
        await page.GetByText("Status: Pending").WaitForAsync();
        return invoiceId;
    }

    private async Task ReviewIdentityChangeAsync(
        IPage requesterPage,
        (string Email, string Password) reviewer,
        string invoiceId
    )
    {
        await using var reviewerContext = await App.NewContextAsync();
        var reviewerPage = await reviewerContext.NewPageAsync();
        await reviewerPage.GotoAsync("/sign-in");
        await reviewerPage.GetByLabel("Email").FillAsync(reviewer.Email);
        await reviewerPage.GetByLabel("Password").FillAsync(reviewer.Password);
        await reviewerPage.GetByRole(AriaRole.Button, new() { Name = "Sign in" }).ClickAsync();
        await reviewerPage.WaitForURLAsync(url =>
            url.EndsWith("/projects", StringComparison.Ordinal)
        );
        await reviewerPage.GotoAsync("/settings/issuer");
        await reviewerPage
            .GetByText($"Proposed: Raccord Media {RunStamp} · B12345678")
            .WaitForAsync();
        await reviewerPage.GetByText("The company updated its registered identity.").WaitForAsync();
        await reviewerPage
            .GetByRole(AriaRole.Button, new() { Name = "Approve change" })
            .ClickAsync();
        await reviewerPage.GetByText("Status: Approved").WaitForAsync();
        await AssertNewAndHistoricalIdentityAsync(reviewerPage, invoiceId);
        await requesterPage.ReloadAsync();
        await requesterPage.GetByText("Status: Approved").WaitForAsync();
    }

    private async Task AssertNewAndHistoricalIdentityAsync(IPage page, string invoiceId)
    {
        var issuerResponse = await page.APIRequest.GetAsync("/api/organization/issuer");
        issuerResponse.Status.ShouldBe(200);
        using var issuer = JsonDocument.Parse(await issuerResponse.TextAsync());
        issuer
            .RootElement.GetProperty("legalName")
            .GetString()
            .ShouldBe($"Raccord Media {RunStamp}");

        var historicalResponse = await page.APIRequest.GetAsync($"/api/invoices/{invoiceId}");
        historicalResponse.Status.ShouldBe(200);
        using var historical = JsonDocument.Parse(await historicalResponse.TextAsync());
        historical
            .RootElement.GetProperty("issuer")
            .GetProperty("legalName")
            .GetString()
            .ShouldBe("Fotograma Rental Demo S.L.");
    }
}
