using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Raccord.Contracts.Contacts;
using Raccord.Contracts.Customers;
using Raccord.Web.Tests.Infrastructure;

namespace Raccord.Web.Tests;

public sealed class ContactsDirectoryTests(PostgresContainer postgres) : IAsyncLifetime
{
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

    private async Task<ContactResponse> CreateAsync(SaveContactRequest request)
    {
        var response = await client.PostAsJsonAsync("/api/contacts", request);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        return (await response.Content.ReadFromJsonAsync<ContactResponse>())!;
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
