using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Raccord.Contracts.Auditing;
using Raccord.Contracts.Contacts;
using Raccord.Contracts.Customers;
using Raccord.Web.Tests.Infrastructure;

namespace Raccord.Web.Tests;

public sealed class ContactsDirectoryTests(PostgresContainer postgres) : IAsyncLifetime
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private RaccordApi api = null!;
    private HttpClient client = null!;

    public async ValueTask InitializeAsync()
    {
        api = await RaccordApi.StartAsync(postgres);
        client = api.CreateClient();
    }

    public async ValueTask DisposeAsync()
    {
        client.Dispose();
        await api.DisposeAsync();
    }

    [Fact]
    public async Task Contacts_Should_CreateEditFilterAndPaginate()
    {
        var company = await CreateAsync(
            new SaveContactRequest(
                "Company",
                "Northlight Films",
                null,
                "A12345678",
                null,
                "billing@northlight.example",
                ["Customer", "Supplier"]
            )
        );
        var venue = await CreateAsync(VenueRequest);

        company.ContactNumber.ShouldNotBeNullOrWhiteSpace();
        company.ContactNumber.ShouldNotBe(venue.ContactNumber);

        var firstPage = await client.GetFromJsonAsync<ContactPageResponse>(
            "/api/contacts?page=1&pageSize=1"
        );
        firstPage!.Items.Count.ShouldBe(1);
        firstPage.HasNextPage.ShouldBeTrue();
        firstPage.Items[0].ContactNumber.ShouldBe(company.ContactNumber);

        var supplierPage = await client.GetFromJsonAsync<ContactPageResponse>(
            "/api/contacts?role=Supplier"
        );
        supplierPage!.TotalCount.ShouldBe(1);
        supplierPage.Items[0].Id.ShouldBe(company.Id);

        var customerProjection = await client.GetFromJsonAsync<List<CustomerResponse>>(
            "/api/customers"
        );
        customerProjection!.Select(item => item.Id).ShouldBe([company.Id]);
        (await client.GetAsync($"/api/customers/{venue.Id}")).StatusCode.ShouldBe(
            HttpStatusCode.NotFound
        );

        await AssertCustomerProjectionAndEditAsync(company, venue);
    }

    [Fact]
    public async Task Contacts_Should_RejectDuplicateContactNumbers()
    {
        await CreateAsync(
            new SaveContactRequest(
                "Company",
                "First Contact",
                "CONTACT-100",
                null,
                null,
                null,
                ["Supplier"]
            )
        );

        var duplicate = await client.PostAsJsonAsync(
            "/api/contacts",
            new SaveContactRequest(
                "Company",
                "Second Contact",
                "CONTACT-100",
                null,
                null,
                null,
                ["Supplier"]
            )
        );

        duplicate.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        var problem = await duplicate.Content.ReadFromJsonAsync<ProblemDetails>();
        problem!.Extensions["code"]!.ToString().ShouldBe("contacts.number-duplicate");
        var saved = await client.GetFromJsonAsync<ContactPageResponse>("/api/contacts");
        saved!.TotalCount.ShouldBe(1);
    }

    [Fact]
    public async Task Contacts_Should_RejectDuplicateNumberOnEditWithoutChangingEitherContact()
    {
        var first = await CreateAsync(
            new SaveContactRequest(
                "Company",
                "First Contact",
                "CONTACT-100",
                null,
                null,
                null,
                ["Supplier"]
            )
        );
        var second = await CreateAsync(
            new SaveContactRequest(
                "Company",
                "Second Contact",
                "CONTACT-200",
                null,
                null,
                null,
                ["Venue"]
            )
        );

        var duplicate = await client.PutAsJsonAsync(
            $"/api/contacts/{second.Id}",
            new SaveContactRequest(
                "PrivatePerson",
                "Changed Contact",
                first.ContactNumber,
                null,
                null,
                null,
                ["Customer"]
            )
        );

        duplicate.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        var problem = await duplicate.Content.ReadFromJsonAsync<ProblemDetails>();
        problem!.Extensions["code"]!.ToString().ShouldBe("contacts.number-duplicate");

        var unchangedFirst = await client.GetFromJsonAsync<ContactResponse>(
            $"/api/contacts/{first.Id}"
        );
        unchangedFirst!.LegalName.ShouldBe("First Contact");
        unchangedFirst.ContactNumber.ShouldBe("CONTACT-100");
        unchangedFirst.Roles.ShouldBe(["Supplier"]);

        var unchangedSecond = await client.GetFromJsonAsync<ContactResponse>(
            $"/api/contacts/{second.Id}"
        );
        unchangedSecond!.LegalName.ShouldBe("Second Contact");
        unchangedSecond.ContactNumber.ShouldBe("CONTACT-200");
        unchangedSecond.Roles.ShouldBe(["Venue"]);
    }

    [Fact]
    public async Task Contacts_Should_AssignDistinctNumbersDuringConcurrentCreation()
    {
        var creates = Enumerable
            .Range(1, 8)
            .Select(index =>
                client.PostAsJsonAsync(
                    "/api/contacts",
                    new SaveContactRequest(
                        "Company",
                        $"Concurrent Contact {index}",
                        null,
                        null,
                        null,
                        null,
                        ["Supplier"]
                    )
                )
            );
        var responses = await Task.WhenAll(creates);
        responses
            .Select(response => response.StatusCode)
            .ShouldAllBe(status => status == HttpStatusCode.Created);
        var contacts = await Task.WhenAll(
            responses.Select(response => response.Content.ReadFromJsonAsync<ContactResponse>())
        );

        contacts
            .Select(contact => contact!.ContactNumber)
            .Distinct(StringComparer.Ordinal)
            .Count()
            .ShouldBe(8);
    }

    [Fact]
    public async Task Contacts_Should_SkipASequenceValueAlreadyEnteredManually()
    {
        await CreateAsync(
            new SaveContactRequest(
                "Company",
                "Manual Number",
                "10001",
                null,
                null,
                null,
                ["Supplier"]
            )
        );

        var assigned = await CreateAsync(
            new SaveContactRequest(
                "Company",
                "Automatic Number",
                null,
                null,
                null,
                null,
                ["Supplier"]
            )
        );

        assigned.ContactNumber.ShouldBe("10002");
    }

    [Fact]
    public async Task Contacts_Should_RejectOutOfRangePageRequests()
    {
        var response = await client.GetAsync("/api/contacts?page=0&pageSize=10");

        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problem!.Extensions["code"]!.ToString().ShouldBe("contacts.page-invalid");
    }

    [Fact]
    public async Task Contacts_import_Should_PreviewImportValidRowsAndAuditOnlyTheOutcome()
    {
        const string csv =
            "type,legalName,roles,email\r\ncompany,Northlight Films,customer,billing@northlight.example\r\ncompany,,supplier,rejected@example.test\r\n";
        var mapping = new ContactImportMapping(
            HasHeader: true,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["type"] = 0,
                ["legalName"] = 1,
                ["roles"] = 2,
                ["email"] = 3,
            }
        );

        using var previewRequest = CreateImportRequest(csv, mapping, digest: null);
        var previewResponse = await client.PostAsync(
            "/api/contacts/import/preview",
            previewRequest
        );
        previewResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        var preview =
            await previewResponse.Content.ReadFromJsonAsync<ContactImportPreviewResponse>();
        preview!.TotalRowCount.ShouldBe(2);
        preview.ValidRowCount.ShouldBe(1);
        preview.RejectedRowCount.ShouldBe(1);
        preview.Rows[0].Values["legalName"].ShouldBe("Northlight Films");
        preview.Rows[1].Errors.ShouldNotBeEmpty();

        var beforeImport = await client.GetFromJsonAsync<ContactPageResponse>("/api/contacts");
        beforeImport!.TotalCount.ShouldBe(0);

        using var importRequest = CreateImportRequest(csv, mapping, preview.Sha256);
        var importResponse = await client.PostAsync("/api/contacts/import", importRequest);
        importResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = await importResponse.Content.ReadFromJsonAsync<ContactImportResultResponse>();
        result!.AcceptedRowCount.ShouldBe(1);
        result.RejectedRowCount.ShouldBe(1);
        result.RejectedRows.Single().RowNumber.ShouldBe(3);

        var contacts = await client.GetFromJsonAsync<ContactPageResponse>("/api/contacts");
        contacts!.TotalCount.ShouldBe(1);
        contacts.Items[0].LegalName.ShouldBe("Northlight Films");
        await AssertImportAuditAsync(result.ImportId);
    }

    private async Task AssertImportAuditAsync(Guid importId)
    {
        var audit = await client.GetFromJsonAsync<List<AuditEventResponse>>(
            $"/api/audit-events?entityId={importId}"
        );
        var auditEvents = audit!;
        auditEvents
            .Select(item => item.Action)
            .ShouldBe(["contacts.import.started", "contacts.import.completed"]);
        auditEvents.ShouldAllBe(item =>
            item.EntityId == importId
            && !string.IsNullOrWhiteSpace(item.Actor)
            && item.OccurredAt != default
            && string.Equals(item.Details["fileType"], "csv", StringComparison.Ordinal)
        );
        auditEvents[1].Details["acceptedRowCount"].ShouldBe("1");
        auditEvents[1].Details["rejectedRowCount"].ShouldBe("1");
        auditEvents
            .SelectMany(item => item.Details.Values)
            .ShouldNotContain(value => value.Contains("Northlight", StringComparison.Ordinal));
        auditEvents
            .SelectMany(item => item.Details.Values)
            .ShouldNotContain(value => value.Contains("billing@", StringComparison.Ordinal));
        auditEvents
            .SelectMany(item => item.Details.Values)
            .ShouldNotContain(value => value.Contains("rejected@", StringComparison.Ordinal));
    }

    [Fact]
    public async Task Contacts_import_Should_RejectChangedFileAndCreateNoContacts()
    {
        var mapping = new ContactImportMapping(
            HasHeader: false,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["type"] = 0,
                ["legalName"] = 1,
                ["roles"] = 2,
            }
        );
        using var request = CreateImportRequest(
            "company,Changed File,customer\n",
            mapping,
            new string('0', 64)
        );

        var response = await client.PostAsync("/api/contacts/import", request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var contacts = await client.GetFromJsonAsync<ContactPageResponse>("/api/contacts");
        contacts!.TotalCount.ShouldBe(0);
    }

    [Fact]
    public async Task Contacts_import_Should_ReportAllRowsRejectedWithoutCreatingContacts()
    {
        const string csv = "type,legalName,roles\ncompany,,customer\n";
        var mapping = new ContactImportMapping(
            HasHeader: true,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["type"] = 0,
                ["legalName"] = 1,
                ["roles"] = 2,
            }
        );
        using var previewRequest = CreateImportRequest(csv, mapping, digest: null);
        var previewResponse = await client.PostAsync(
            "/api/contacts/import/preview",
            previewRequest
        );
        previewResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        var preview =
            await previewResponse.Content.ReadFromJsonAsync<ContactImportPreviewResponse>();

        using var importRequest = CreateImportRequest(csv, mapping, preview!.Sha256);
        var importResponse = await client.PostAsync("/api/contacts/import", importRequest);

        importResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = await importResponse.Content.ReadFromJsonAsync<ContactImportResultResponse>();
        result!.AcceptedRowCount.ShouldBe(0);
        result.RejectedRowCount.ShouldBe(1);
        result.RejectedRows.Single().RowNumber.ShouldBe(2);
        var contacts = await client.GetFromJsonAsync<ContactPageResponse>("/api/contacts");
        contacts!.TotalCount.ShouldBe(0);
    }

    [Fact]
    public async Task Contacts_import_Should_RequireMultipartForInspection()
    {
        using var request = new StringContent("not a multipart upload");

        var response = await client.PostAsync("/api/contacts/import/inspect", request);

        response.StatusCode.ShouldBe(HttpStatusCode.UnsupportedMediaType);
    }

    [Fact]
    public async Task Contacts_import_Should_RejectFilesLargerThanTenMebibytes()
    {
        using var request = new MultipartFormDataContent();
        request.Add(
            new ByteArrayContent(new byte[ContactImportLimits.MaximumFileBytes + 1]),
            "file",
            "contacts.csv"
        );

        var response = await client.PostAsync("/api/contacts/import/inspect", request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Contacts_import_Should_RequireContactCreatePermission()
    {
        using var unauthorized = api.CreateClient();
        unauthorized.DefaultRequestHeaders.Add("X-Test-NoPermissions", "true");
        var mapping = new ContactImportMapping(
            HasHeader: false,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["type"] = 0,
                ["legalName"] = 1,
                ["roles"] = 2,
            }
        );
        using var request = CreateImportRequest("company,Denied,customer\n", mapping, null);

        var response = await unauthorized.PostAsync("/api/contacts/import/preview", request);

        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    private async Task<ContactResponse> CreateAsync(SaveContactRequest request)
    {
        var response = await client.PostAsJsonAsync("/api/contacts", request);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        return (await response.Content.ReadFromJsonAsync<ContactResponse>())!;
    }

    private static MultipartFormDataContent CreateImportRequest(
        string csv,
        ContactImportMapping mapping,
        string? digest
    )
    {
        var content = new MultipartFormDataContent();
        content.Add(new ByteArrayContent(Encoding.UTF8.GetBytes(csv)), "file", "contacts.csv");
        content.Add(new StringContent(JsonSerializer.Serialize(mapping, JsonOptions)), "mapping");
        if (digest is not null)
        {
            content.Add(new StringContent(digest), "sha256");
        }

        return content;
    }

    private async Task AssertCustomerProjectionAndEditAsync(
        ContactResponse company,
        ContactResponse venue
    )
    {
        var customerProjection = await client.GetFromJsonAsync<List<CustomerResponse>>(
            "/api/customers"
        );
        customerProjection!.Select(item => item.Id).ShouldBe([company.Id]);
        (await client.GetAsync($"/api/customers/{venue.Id}")).StatusCode.ShouldBe(
            HttpStatusCode.NotFound
        );

        var updated = await client.PutAsJsonAsync(
            $"/api/contacts/{company.Id}",
            new SaveContactRequest(
                "Company",
                "Northlight Films Updated",
                company.ContactNumber,
                "A12345678",
                null,
                "billing@northlight.example",
                ["Customer"]
            )
        );
        updated.StatusCode.ShouldBe(HttpStatusCode.OK);
        var updatedContact = (await updated.Content.ReadFromJsonAsync<ContactResponse>())!;
        updatedContact.Id.ShouldBe(company.Id);
        updatedContact.LegalName.ShouldBe("Northlight Films Updated");
        updatedContact.Roles.ShouldBe(["Customer"]);
    }

    private static readonly SaveContactRequest VenueRequest = new(
        "PrivatePerson",
        "Marina Costa",
        null,
        null,
        null,
        null,
        ["Venue"]
    );
}
