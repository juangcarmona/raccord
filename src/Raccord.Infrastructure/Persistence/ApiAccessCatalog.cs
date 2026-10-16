using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Raccord.Contracts.Integrations;

namespace Raccord.Infrastructure.Persistence;

#pragma warning disable CS1591

public sealed class ApiAccessCatalog(RaccordDbContext db)
{
    public async Task<ApiTokenResponse> IssueAsync(
        IssueApiTokenRequest request,
        CancellationToken ct
    )
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        var row = new ApiAccessRow
        {
            Id = Guid.CreateVersion7(),
            Name = request.Name.Trim(),
            TokenHash = Hash(token),
            Scopes = JsonSerializer.Serialize(request.Scopes.Distinct(StringComparer.Ordinal)),
            CreatedAt = DateTimeOffset.UtcNow,
        };
        db.ApiAccessTokens.Add(row);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return ToResponse(row, token);
    }

    public async Task<ApiTokenResponse?> RegenerateAsync(Guid id, CancellationToken ct)
    {
        var row = await db
            .ApiAccessTokens.SingleOrDefaultAsync(item => item.Id == id, ct)
            .ConfigureAwait(false);
        if (row is null)
            return null;
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        row.TokenHash = Hash(token);
        row.CreatedAt = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return ToResponse(row, token);
    }

    public async Task<bool> HasScopeAsync(string? token, string scope, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(token))
            return false;
        var row = await db
            .ApiAccessTokens.AsNoTracking()
            .SingleOrDefaultAsync(item => item.TokenHash == Hash(token), ct)
            .ConfigureAwait(false);
        return row is not null
            && JsonSerializer
                .Deserialize<string[]>(row.Scopes)
                ?.Contains(scope, StringComparer.Ordinal) == true;
    }

    public async Task<IReadOnlyList<ApiTokenResponse>> ListAsync(CancellationToken ct)
    {
        var rows = await db
            .ApiAccessTokens.AsNoTracking()
            .OrderByDescending(item => item.CreatedAt)
            .ToListAsync(ct)
            .ConfigureAwait(false);
        return rows.Select(row => ToResponse(row)).ToArray();
    }

    private static string Hash(string value) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value)));

    private static ApiTokenResponse ToResponse(ApiAccessRow row, string? token = null) =>
        new(
            row.Id,
            row.Name,
            JsonSerializer.Deserialize<string[]>(row.Scopes) ?? [],
            row.CreatedAt,
            token
        );
}

#pragma warning restore CS1591
