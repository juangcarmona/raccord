using Microsoft.EntityFrameworkCore;
using Raccord.Contracts.Documents;

namespace Raccord.Infrastructure.Persistence;

#pragma warning disable CS1591

public sealed class CommunicationCatalog(RaccordDbContext db)
{
    public async Task<CommunicationSnapshot?> CreateAsync(
        CreateCommunicationRequest request,
        CancellationToken ct
    )
    {
        if (
            string.IsNullOrWhiteSpace(request.Recipient)
            || string.IsNullOrWhiteSpace(request.Subject)
        )
            return null;

        var row = new CommunicationRow
        {
            Id = Guid.NewGuid(),
            Recipient = request.Recipient.Trim(),
            Subject = request.Subject.Trim(),
            DocumentKey = request.DocumentKey?.Trim(),
            Status = "Pending",
            CreatedAt = DateTimeOffset.UtcNow,
        };
        db.Communications.Add(row);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return ToSnapshot(row);
    }

    public async Task<IReadOnlyList<CommunicationSnapshot>> ListAsync(
        string? search,
        CancellationToken ct
    )
    {
        var query = db.Communications.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(row => row.Recipient.Contains(term) || row.Subject.Contains(term));
        }

        var rows = await query
            .OrderByDescending(row => row.CreatedAt)
            .ToListAsync(ct)
            .ConfigureAwait(false);
        return rows.Select(ToSnapshot).ToArray();
    }

    public async Task<CommunicationSnapshot?> RecordOutcomeAsync(
        Guid id,
        RecordDeliveryOutcomeRequest request,
        CancellationToken ct
    )
    {
        var status = request.Status.Trim();
        if (
            !string.Equals(status, "Sent", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(status, "Failed", StringComparison.OrdinalIgnoreCase)
        )
            return null;

        var row = await db
            .Communications.FirstOrDefaultAsync(candidate => candidate.Id == id, ct)
            .ConfigureAwait(false);
        if (row is null)
            return null;

        row.Status = status.Equals("Sent", StringComparison.OrdinalIgnoreCase) ? "Sent" : "Failed";
        row.FailureReason = string.Equals(row.Status, "Failed", StringComparison.Ordinal)
            ? request.FailureReason?.Trim()
            : null;
        row.DeliveredAt = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return ToSnapshot(row);
    }

    private static CommunicationSnapshot ToSnapshot(CommunicationRow row) =>
        new(
            row.Id,
            row.Recipient,
            row.Subject,
            row.DocumentKey,
            row.Status,
            row.FailureReason,
            row.CreatedAt,
            row.DeliveredAt
        );
}

#pragma warning restore CS1591
