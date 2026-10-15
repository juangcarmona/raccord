using System.Net;
using System.Net.Http.Json;
using Raccord.Contracts.Documents;
using Raccord.Web.Tests.Infrastructure;

namespace Raccord.Web.Tests;

public sealed class CommunicationTests(PostgresContainer postgres) : IAsyncLifetime
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
    public async Task Communication_Should_RecordFailedDeliveryAndRemainSearchable()
    {
        var created = await (
            await client.PostAsJsonAsync(
                "/api/documents/communications",
                new CreateCommunicationRequest(
                    "customer@example.com",
                    "Invoice ready",
                    "invoices/1.pdf"
                )
            )
        )
            .EnsureSuccessStatusCode()
            .Content.ReadFromJsonAsync<CommunicationSnapshot>();

        created.ShouldNotBeNull();
        created!.Status.ShouldBe("Pending");

        var failed = await (
            await client.PutAsJsonAsync(
                $"/api/documents/communications/{created.Id}/delivery",
                new RecordDeliveryOutcomeRequest("Failed", "Mailbox rejected the message")
            )
        )
            .EnsureSuccessStatusCode()
            .Content.ReadFromJsonAsync<CommunicationSnapshot>();

        failed.ShouldNotBeNull();
        failed!.Status.ShouldBe("Failed");
        failed.FailureReason.ShouldBe("Mailbox rejected the message");
        failed.DeliveredAt.ShouldNotBeNull();

        var search = await client.GetFromJsonAsync<CommunicationSnapshot[]>(
            "/api/documents/communications?search=customer@example.com"
        );
        search.ShouldNotBeNull();
        search!.ShouldContain(item => item.Id == created.Id);
    }

    [Fact]
    public async Task Communication_Should_RejectIncompleteSendRequest()
    {
        var response = await client.PostAsJsonAsync(
            "/api/documents/communications",
            new CreateCommunicationRequest("", "Invoice ready")
        );

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
