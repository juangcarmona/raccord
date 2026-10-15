using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Raccord.Contracts.Integrations;

namespace Raccord.Infrastructure.Persistence;

#pragma warning disable CS1591

public sealed class CompanyRegistryCatalog(HttpClient client, IConfiguration configuration)
{
    public async Task<IReadOnlyList<CompanyRegistryMatchResponse>> SearchAsync(
        string country,
        string query,
        CancellationToken ct
    )
    {
        var baseUrl = configuration["Raccord:CompanyRegistry:BaseUrl"];
        if (string.IsNullOrWhiteSpace(baseUrl))
            return [];

        var uri =
            $"{baseUrl.TrimEnd('/')}/search?country={Uri.EscapeDataString(country)}&query={Uri.EscapeDataString(query)}";
        return await client
                .GetFromJsonAsync<List<CompanyRegistryMatchResponse>>(uri, ct)
                .ConfigureAwait(false)
            ?? [];
    }
}

#pragma warning restore CS1591
