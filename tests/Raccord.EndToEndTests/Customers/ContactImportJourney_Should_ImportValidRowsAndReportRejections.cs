using System.Text;
using System.Text.Json;
using Microsoft.Playwright;
using Raccord.EndToEndTests.Infrastructure;
using Shouldly;
using Xunit;

namespace Raccord.EndToEndTests.Customers;

/// <summary>
/// Given a synthetic contact spreadsheet, when an administrator previews and imports it, then valid rows are saved, rejected rows are reported, and the audit trail stores only the outcome (SB-CON-IMPORT-VALID-AND-INVALID-ROWS).
/// </summary>
[Collection(E2ECollection.Name)]
[Trait(E2ETraits.Depth, E2ETraits.Depths.Smoke)]
[Trait(E2ETraits.Area, E2ETraits.Areas.Contacts)]
public sealed class ContactImportJourney_Should_ImportValidRowsAndReportRejections(
    DistributedAppFixture app
) : BrowserJourneyBase(app)
{
    [Fact]
    public async Task Journey_Should_ImportValidRowsAndShowRejectedRowsAndAuditOutcome()
    {
        await RunJourneyAsync(
            "contacts-spreadsheet-import",
            async page =>
            {
                var contactName = $"Northlight Import {RunStamp}";
                var csv =
                    $"type,legalName,roles,email\nCompany,{contactName},Customer,billing@example.test\nCompany,,Supplier,rejected@example.test\n";
                var importId = await ImportSpreadsheetAsync(page, csv, contactName);
                await AssertAuditOutcomeAsync(page, importId, contactName);
            }
        );
    }

    private static async Task<string> ImportSpreadsheetAsync(
        IPage page,
        string csv,
        string contactName
    )
    {
        await page.GotoAsync("/contacts/import");
        await page.GetByLabel("CSV or XLSX file")
            .SetInputFilesAsync(
                new FilePayload
                {
                    Name = "contacts.csv",
                    MimeType = "text/csv",
                    Buffer = Encoding.UTF8.GetBytes(csv),
                }
            );

        await page.GetByRole(AriaRole.Table, new() { Name = "Sample rows" }).WaitForAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = "Validate and preview" }).ClickAsync();
        await page.GetByText("Rows: 2. Valid: 1. Rejected: 1.").WaitForAsync();
        await page.GetByRole(AriaRole.Table, new() { Name = "First rows and validation" })
            .GetByRole(AriaRole.Row)
            .Filter(new() { HasText = contactName })
            .WaitForAsync();

        var importResponseTask = page.WaitForResponseAsync(response =>
            response.Url.EndsWith("/api/contacts/import", StringComparison.Ordinal)
            && string.Equals(response.Request.Method, "POST", StringComparison.Ordinal)
        );
        await page.GetByRole(AriaRole.Button, new() { Name = "Import 1 valid contacts" })
            .ClickAsync();
        var importResponse = await importResponseTask;
        importResponse.Ok.ShouldBeTrue();
        using var result = JsonDocument.Parse(await importResponse.TextAsync());
        var importId = result.RootElement.GetProperty("importId").GetString();
        importId.ShouldNotBeNullOrWhiteSpace();

        await page.GetByText("Import complete").WaitForAsync();
        await page.GetByText("Row 3: Contact legal name is required.").WaitForAsync();
        var contactsResponse = await page.APIRequest.GetAsync("/api/contacts?page=1&pageSize=100");
        contactsResponse.Ok.ShouldBeTrue();
        using var contacts = JsonDocument.Parse(await contactsResponse.TextAsync());
        contacts
            .RootElement.GetProperty("items")
            .EnumerateArray()
            .Count(item =>
                string.Equals(
                    item.GetProperty("legalName").GetString(),
                    contactName,
                    StringComparison.Ordinal
                )
            )
            .ShouldBe(1);
        return importId;
    }

    private static async Task AssertAuditOutcomeAsync(
        IPage page,
        string importId,
        string contactName
    )
    {
        var auditResponse = await page.APIRequest.GetAsync(
            $"/api/audit-events?entityId={importId}"
        );
        auditResponse.Ok.ShouldBeTrue();
        using var audit = JsonDocument.Parse(await auditResponse.TextAsync());
        var events = audit.RootElement.EnumerateArray().ToArray();
        events.Length.ShouldBe(2);
        events
            .Select(item => item.GetProperty("action").GetString())
            .ShouldBe(["contacts.import.started", "contacts.import.completed"]);
        events
            .All(item => !string.IsNullOrWhiteSpace(item.GetProperty("actor").GetString()))
            .ShouldBeTrue();
        events
            .All(item =>
                item.GetProperty("occurredAt").GetDateTimeOffset() > DateTimeOffset.MinValue
            )
            .ShouldBeTrue();
        var auditDetails = events.SelectMany(item =>
            item.GetProperty("details")
                .EnumerateObject()
                .Select(detail => detail.Value.GetString() ?? "")
        );
        auditDetails.ShouldNotContain(value =>
            value.Contains(contactName, StringComparison.Ordinal)
        );
        auditDetails.ShouldNotContain(value =>
            value.Contains("billing@example.test", StringComparison.Ordinal)
        );
        auditDetails.ShouldNotContain(value =>
            value.Contains("rejected@example.test", StringComparison.Ordinal)
        );
        auditDetails.ShouldContain("1");
    }
}
