using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Raccord.Contracts.Integrations;
using Raccord.Domain.Projects;

#pragma warning disable CS1591

namespace Raccord.Infrastructure.Persistence;

public sealed class CalendarFeedCatalog(RaccordDbContext db)
{
    public async Task<CalendarFeedResponse> IssueAsync(
        string stream,
        string baseUrl,
        CancellationToken ct
    )
    {
        if (!string.Equals(stream, "all-projects", StringComparison.Ordinal))
            throw new ArgumentException("The supported stream is all-projects.", nameof(stream));

        var token = ToBase64Url(RandomNumberGenerator.GetBytes(32));
        var row = new CalendarFeedRow
        {
            Id = Guid.CreateVersion7(),
            TokenHash = Hash(token),
            Stream = stream,
            CreatedAt = DateTimeOffset.UtcNow,
        };
        db.CalendarFeeds.Add(row);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return ToResponse(row, token, baseUrl);
    }

    public async Task<IReadOnlyList<Project>?> LoadAsync(
        string token,
        string stream,
        CancellationToken ct
    )
    {
        var feed = await db
            .CalendarFeeds.AsNoTracking()
            .SingleOrDefaultAsync(
                row =>
                    row.TokenHash == Hash(token) && row.Stream == stream && row.RevokedAt == null,
                ct
            )
            .ConfigureAwait(false);
        if (feed is null)
            return null;

        var from = DateOnly.FromDateTime(DateTime.UtcNow).AddMonths(-6);
        var to = from.AddMonths(12);
        var projects = await db
            .Projects.AsNoTracking()
            .Where(project => project.StartDate <= to && project.EndDate >= from)
            .OrderBy(project => project.StartDate)
            .ThenBy(project => project.Name)
            .ToListAsync(ct)
            .ConfigureAwait(false);
        return projects;
    }

    public async Task<bool> RevokeAsync(Guid id, CancellationToken ct)
    {
        var feed = await db
            .CalendarFeeds.SingleOrDefaultAsync(row => row.Id == id, ct)
            .ConfigureAwait(false);
        if (feed is null)
            return false;
        feed.RevokedAt = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return true;
    }

    public async Task<CalendarFeedResponse?> RotateAsync(
        Guid id,
        string baseUrl,
        CancellationToken ct
    )
    {
        var feed = await db
            .CalendarFeeds.SingleOrDefaultAsync(row => row.Id == id && row.RevokedAt == null, ct)
            .ConfigureAwait(false);
        if (feed is null)
            return null;

        var token = ToBase64Url(RandomNumberGenerator.GetBytes(32));
        feed.TokenHash = Hash(token);
        feed.CreatedAt = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return ToResponse(feed, token, baseUrl);
    }

    private static CalendarFeedResponse ToResponse(
        CalendarFeedRow row,
        string token,
        string baseUrl
    ) => new(row.Id, row.Stream, $"{baseUrl.TrimEnd('/')}/calendar-feeds/{token}/{row.Stream}.ics");

    private static string Hash(string value) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value)));

    private static string ToBase64Url(byte[] bytes) =>
        Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
}

#pragma warning restore CS1591
