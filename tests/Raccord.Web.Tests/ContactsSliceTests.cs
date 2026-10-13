using System.Net;
using System.Net.Http.Json;
using Raccord.Contracts.Contacts;
using Raccord.Contracts.Customers;
using Raccord.Web.Tests.Infrastructure;

namespace Raccord.Web.Tests;

public sealed class ContactsSliceTests(PostgresContainer postgres) : IAsyncLifetime
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
    public async Task Contacts_Should_CreateEditAndPageExistingCustomerData()
    {
        var customer = await (
            await client.PostAsJsonAsync(
                "/api/customers",
                new RegisterCustomerRequest(
                    "Existing customer",
                    "A00000000",
                    RaccordApi.Madrid,
                    null
                )
            )
        )
            .EnsureSuccessStatusCode()
            .Content.ReadFromJsonAsync<CustomerResponse>();

        var supplier = await (
            await client.PostAsJsonAsync(
                "/api/contacts",
                new RegisterContactRequest(
                    "Supplier contact",
                    ContactType.Supplier,
                    null,
                    null,
                    null
                )
            )
        )
            .EnsureSuccessStatusCode()
            .Content.ReadFromJsonAsync<ContactResponse>();

        var venue = await (
            await client.PostAsJsonAsync(
                "/api/contacts",
                new RegisterContactRequest("Venue contact", ContactType.Venue, null, null, null)
            )
        )
            .EnsureSuccessStatusCode()
            .Content.ReadFromJsonAsync<ContactResponse>();

        var page = await client.GetFromJsonAsync<ContactPage>("/api/contacts?page=1&pageSize=2");
        page!.TotalCount.ShouldBe(3);
        page.Items.Count.ShouldBe(2);

        var update = await client.PutAsJsonAsync(
            $"/api/contacts/{customer!.Id}",
            new UpdateContactRequest(
                "Renamed customer",
                ContactType.Customer,
                "A00000000",
                RaccordApi.Madrid,
                null
            )
        );
        update.StatusCode.ShouldBe(HttpStatusCode.OK);
        var renamed = await update.Content.ReadFromJsonAsync<ContactResponse>();
        renamed!.LegalName.ShouldBe("Renamed customer");
        renamed.Type.ShouldBe(ContactType.Customer);
        supplier!.Type.ShouldBe(ContactType.Supplier);
        venue!.Type.ShouldBe(ContactType.Venue);
    }
}
