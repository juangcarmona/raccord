using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Options;

namespace Raccord.Infrastructure.Compliance;

internal sealed class HttpComplianceAuthorityClient(
    IHttpClientFactory clients,
    IOptions<ComplianceOptions> options
) : IComplianceAuthorityClient
{
    public async Task<ComplianceAuthorityResponse> SubmitAsync(
        string idempotencyKey,
        string invoiceNumber,
        string representation,
        CancellationToken cancellationToken
    )
    {
        var settings = options.Value;
        if (!Uri.TryCreate(settings.Endpoint, UriKind.Absolute, out var endpoint))
            throw new InvalidOperationException("Compliance authority endpoint is not configured.");

        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = JsonContent.Create(new { invoiceNumber, representation }),
        };
        request.Headers.Add("Idempotency-Key", idempotencyKey);
        var response = await clients
            .CreateClient(nameof(HttpComplianceAuthorityClient))
            .SendAsync(request, cancellationToken)
            .ConfigureAwait(false);
        var body = await response
            .Content.ReadFromJsonAsync<AuthorityReply>(cancellationToken)
            .ConfigureAwait(false);
        return response.IsSuccessStatusCode && body?.Accepted == true
            ? new(true, body.Reference, null)
            : new(
                false,
                body?.Reference,
                body?.Error ?? $"Authority returned {(int)response.StatusCode}."
            );
    }

    private sealed record AuthorityReply(bool Accepted, string? Reference, string? Error);
}
