using System.Text.Json;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Projects;

[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Projects)]
public sealed class ProjectDetailsJourney_Should_EditDetails(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_EditAndReloadProjectDetails()
    {
        await RunJourneyAsync(
            "project-edit-details",
            async page =>
            {
                await CreateCustomerAsync(page);
                await CreateProjectWithBillableLineAsync(page);
                var projectUrl = page.Url;
                await CreateAlternativeCustomerAsync(page);
                var contactPersonId = await CreateAlternativeContactPersonAsync(page);
                await page.GotoAsync(projectUrl);
                var projectId = new Uri(projectUrl).Segments[^1];
                var currentProject = await page.APIRequest.GetAsync($"/api/projects/{projectId}");
                currentProject.Ok.ShouldBeTrue();
                using var projectDocument = JsonDocument.Parse(await currentProject.TextAsync());
                await AssertOverlongReferenceRejectedAsync(
                    page,
                    projectId,
                    projectDocument.RootElement.GetProperty("customerId").GetString()!
                );

                await page.GetByLabel("Project name").FillAsync("Summer campaign shoot");
                await page.GetByLabel("Customer")
                    .SelectOptionAsync(
                        new SelectOptionValue { Label = $"Alternative E2E {RunStamp}" }
                    );
                await page.GetByLabel("Contact person").SelectOptionAsync(contactPersonId);
                await page.GetByLabel("Start", new() { Exact = true }).FillAsync("2026-10-01");
                await page.GetByLabel("End", new() { Exact = true }).FillAsync("2026-10-03");
                await page.GetByLabel("External reference").FillAsync("PO-2026-041");
                await page.GetByRole(AriaRole.Button, new() { Name = "Save details" }).ClickAsync();
                await page.GetByRole(AriaRole.Heading, new() { Name = "Summer campaign shoot" })
                    .WaitForAsync();
                await page.ReloadAsync();

                (await page.GetByLabel("Project name").InputValueAsync()).ShouldBe(
                    "Summer campaign shoot"
                );
                (
                    await page.GetByLabel("Customer").Locator("option:checked").TextContentAsync()
                ).ShouldBe($"Alternative E2E {RunStamp}");
                (
                    await page.GetByLabel("Contact person")
                        .Locator("option:checked")
                        .TextContentAsync()
                ).ShouldBe("Production lead");
                (await page.GetByLabel("Start", new() { Exact = true }).InputValueAsync()).ShouldBe(
                    "2026-10-01"
                );
                (await page.GetByLabel("End", new() { Exact = true }).InputValueAsync()).ShouldBe(
                    "2026-10-03"
                );
                (await page.GetByLabel("External reference").InputValueAsync()).ShouldBe(
                    "PO-2026-041"
                );
            }
        );
    }

    private async Task CreateAlternativeCustomerAsync(IPage page)
    {
        await page.GotoAsync("/customers/new");
        await page.GetByLabel("Legal name").FillAsync($"Alternative E2E {RunStamp}");
        await page.GetByLabel("Tax ID").FillAsync("B87654322");
        await page.GetByLabel("Billing e-mail").FillAsync("alternative@productora-e2e.example");
        await page.GetByLabel("Street and number").FillAsync("Calle de pruebas 2");
        await page.GetByLabel("Postal code").FillAsync("28002");
        await page.GetByLabel("City").FillAsync("Madrid");
        await page.GetByLabel("Country (ISO").FillAsync("ES");
        await page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
        await page.WaitForURLAsync(url => url.EndsWith("/customers", StringComparison.Ordinal));
    }

    private async Task<string> CreateAlternativeContactPersonAsync(IPage page)
    {
        var alternativeCustomer = await page.APIRequest.GetAsync("/api/customers");
        using var customerList = JsonDocument.Parse(await alternativeCustomer.TextAsync());
        var alternativeCustomerId = customerList
            .RootElement.EnumerateArray()
            .Single(customer =>
                string.Equals(
                    customer.GetProperty("legalName").GetString(),
                    $"Alternative E2E {RunStamp}",
                    StringComparison.Ordinal
                )
            )
            .GetProperty("id")
            .GetString()!;
        var response = await page.APIRequest.PostAsync(
            $"/api/contacts/{alternativeCustomerId}/people",
            new APIRequestContextOptions
            {
                Data = JsonSerializer.Serialize(
                    new
                    {
                        name = "Production lead",
                        email = "production.lead@example.test",
                        phone = (string?)null,
                        isDefault = true,
                        isAdministrative = false,
                    }
                ),
                Headers = new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    ["content-type"] = "application/json",
                },
            }
        );
        response.Status.ShouldBe(201);
        using var personDocument = JsonDocument.Parse(await response.TextAsync());
        return personDocument.RootElement.GetProperty("id").GetString()!;
    }

    private static async Task AssertOverlongReferenceRejectedAsync(
        IPage page,
        string projectId,
        string customerId
    )
    {
        var invalidDetails = await page.APIRequest.PutAsync(
            $"/api/projects/{projectId}",
            new APIRequestContextOptions
            {
                Data = JsonSerializer.Serialize(
                    new
                    {
                        customerId,
                        name = "Summer campaign shoot",
                        startDate = "2026-10-01",
                        endDate = "2026-10-03",
                        externalReference = new string('x', 201),
                    }
                ),
                Headers = new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    ["content-type"] = "application/json",
                },
            }
        );
        invalidDetails.Status.ShouldBe(422);
    }
}
