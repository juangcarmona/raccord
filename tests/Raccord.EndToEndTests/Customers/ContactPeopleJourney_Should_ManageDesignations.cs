using System.Text.Json;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Customers;

/// <summary>Creates people under a contact and verifies their designations preserve existing associations and issued data.</summary>
[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Contacts)]
public sealed class ContactPeopleJourney_Should_ManageDesignations(DistributedAppFixture app)
    : BrowserJourneyBase(app)
{
    [Fact]
    public Task Journey_Should_ReassignDefaultAndKeepProjectAndInvoiceSnapshots() =>
        RunJourneyAsync("contacts-people-designations", CompleteJourneyAsync);

    private async Task CompleteJourneyAsync(IPage page)
    {
        var contactName = $"Productora E2E {RunStamp}";
        await CreateCustomerAsync(page);
        var contactId = await FindCustomerIdAsync(page, contactName);
        await CreateProjectWithBillableLineAsync(page);
        var projectId = new Uri(page.Url).Segments[^1].TrimEnd('/');
        var projectBefore = await ReadProjectAsync(page, projectId);
        await FinalizeDraftAsync(page);
        var invoiceId = new Uri(page.Url).Segments[^1].TrimEnd('/');
        var invoiceBefore = await ReadInvoiceAsync(page, invoiceId);
        invoiceBefore
            .GetProperty("recipient")
            .GetProperty("legalName")
            .GetString()
            .ShouldBe(contactName);

        await AddAndAssertDesignationsAsync(page, contactId);
        await AssertAssociationsUnchangedAsync(
            page,
            projectId,
            projectBefore,
            invoiceId,
            invoiceBefore
        );
    }

    private static async Task AddAndAssertDesignationsAsync(IPage page, string contactId)
    {
        await page.GotoAsync($"/contacts/{contactId}");
        await AddPersonAsync(page, "Billing lead", isDefault: true, isAdministrative: false);
        await AddPersonAsync(page, "Production contact", isDefault: true, isAdministrative: true);

        var response = await page.APIRequest.GetAsync($"/api/contacts/{contactId}/people");
        response.Ok.ShouldBeTrue();
        using var document = JsonDocument.Parse(await response.TextAsync());
        var persons = document.RootElement.EnumerateArray().ToArray();
        persons.Length.ShouldBe(2);
        persons.Count(person => person.GetProperty("isDefault").GetBoolean()).ShouldBe(1);
        persons.Count(person => person.GetProperty("isAdministrative").GetBoolean()).ShouldBe(1);
        FindPerson(persons, "Billing lead").GetProperty("isDefault").GetBoolean().ShouldBeFalse();
        var selected = FindPerson(persons, "Production contact");
        selected.GetProperty("isDefault").GetBoolean().ShouldBeTrue();
        selected.GetProperty("isAdministrative").GetBoolean().ShouldBeTrue();
        await page.GetByText("Production contact").WaitForAsync();
    }

    private static JsonElement FindPerson(JsonElement[] persons, string name) =>
        persons.Single(person =>
            string.Equals(person.GetProperty("name").GetString(), name, StringComparison.Ordinal)
        );

    private static async Task AssertAssociationsUnchangedAsync(
        IPage page,
        string projectId,
        JsonElement projectBefore,
        string invoiceId,
        JsonElement invoiceBefore
    )
    {
        var projectAfter = await ReadProjectAsync(page, projectId);
        projectAfter
            .GetProperty("customerId")
            .GetGuid()
            .ShouldBe(projectBefore.GetProperty("customerId").GetGuid());
        var invoiceAfter = await ReadInvoiceAsync(page, invoiceId);
        invoiceAfter
            .GetProperty("recipient")
            .GetProperty("legalName")
            .GetString()
            .ShouldBe(invoiceBefore.GetProperty("recipient").GetProperty("legalName").GetString());
        invoiceAfter
            .GetProperty("recipient")
            .GetProperty("address")
            .GetRawText()
            .ShouldBe(invoiceBefore.GetProperty("recipient").GetProperty("address").GetRawText());
    }

    private static async Task AddPersonAsync(
        IPage page,
        string name,
        bool isDefault,
        bool isAdministrative
    )
    {
        await page.GetByLabel("Person name").FillAsync(name);
        await page.GetByLabel("Person email").FillAsync($"{name.Replace(' ', '.')}@example.test");
        if (isDefault)
        {
            await page.GetByLabel("Default person").CheckAsync();
        }
        if (isAdministrative)
        {
            await page.GetByLabel("Administrative person").CheckAsync();
        }
        await page.GetByRole(AriaRole.Button, new() { Name = "Add person" }).ClickAsync();
        await page.GetByText(name).WaitForAsync();
    }

    private static async Task<JsonElement> ReadProjectAsync(IPage page, string projectId)
    {
        var response = await page.APIRequest.GetAsync($"/api/projects/{projectId}");
        response.Ok.ShouldBeTrue();
        using var document = JsonDocument.Parse(await response.TextAsync());
        return document.RootElement.Clone();
    }

    private static async Task<JsonElement> ReadInvoiceAsync(IPage page, string invoiceId)
    {
        var response = await page.APIRequest.GetAsync($"/api/invoices/{invoiceId}");
        response.Ok.ShouldBeTrue();
        using var document = JsonDocument.Parse(await response.TextAsync());
        return document.RootElement.Clone();
    }

    private static async Task<string> FindCustomerIdAsync(IPage page, string legalName)
    {
        var response = await page.APIRequest.GetAsync("/api/customers");
        response.Ok.ShouldBeTrue();
        using var customers = JsonDocument.Parse(await response.TextAsync());
        return customers
            .RootElement.EnumerateArray()
            .Single(customer =>
                string.Equals(
                    customer.GetProperty("legalName").GetString(),
                    legalName,
                    StringComparison.Ordinal
                )
            )
            .GetProperty("id")
            .GetString()!;
    }
}
