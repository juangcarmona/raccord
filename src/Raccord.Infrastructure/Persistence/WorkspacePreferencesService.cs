using Microsoft.EntityFrameworkCore;

namespace Raccord.Infrastructure.Persistence;

#pragma warning disable CS1591, MA0048
public sealed class WorkspacePreferencesService(RaccordDbContext db)
{
    public async Task<WorkspacePreferencesResponse> GetAsync(CancellationToken cancellationToken)
    {
        var row = await db.Set<WorkspacePreferencesRow>()
            .SingleOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
        return row is null
            ? new WorkspacePreferencesResponse("en", "UTC")
            : new(row.Language, row.TimeZone);
    }

    public async Task<WorkspacePreferencesResponse> UpdateAsync(
        string language,
        string timeZone,
        CancellationToken cancellationToken
    )
    {
        if (language is not ("en" or "es"))
            throw new ArgumentException("Language must be en or es.", nameof(language));
        _ = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
        var row = await db.Set<WorkspacePreferencesRow>()
            .SingleOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
        if (row is null)
            db.Add(
                new WorkspacePreferencesRow
                {
                    Id = 1,
                    Language = language,
                    TimeZone = timeZone,
                }
            );
        else
        {
            row.Language = language;
            row.TimeZone = timeZone;
        }
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return new WorkspacePreferencesResponse(language, timeZone);
    }
}

public sealed record WorkspacePreferencesResponse(string Language, string TimeZone);
#pragma warning restore CS1591, MA0048
