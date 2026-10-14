using Microsoft.EntityFrameworkCore;
using Raccord.BuildingBlocks.Storage;
using Raccord.Contracts.Documents;

namespace Raccord.Infrastructure.Persistence;

#pragma warning disable CS1591

public sealed class LetterheadCatalog(RaccordDbContext db, IDocumentStore documents)
{
    public async Task<LetterheadSnapshot?> CreateAsync(
        CreateLetterheadRequest request,
        Stream pdf,
        CancellationToken ct
    )
    {
        if (
            !IsValid(request)
            || pdf.Length == 0
            || await db
                .Letterheads.AnyAsync(row => row.Name == request.Name.Trim(), ct)
                .ConfigureAwait(false)
        )
            return null;
        using var memory = new MemoryStream();
        await pdf.CopyToAsync(memory, ct).ConfigureAwait(false);
        var row = new LetterheadRow
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            DocumentTypes = request.DocumentTypes.Trim(),
            PageWidthMillimetres = request.PageWidthMillimetres,
            PageHeightMillimetres = request.PageHeightMillimetres,
            MarginTopMillimetres = request.MarginTopMillimetres,
            MarginRightMillimetres = request.MarginRightMillimetres,
            MarginBottomMillimetres = request.MarginBottomMillimetres,
            MarginLeftMillimetres = request.MarginLeftMillimetres,
            IsDefault = !await db.Letterheads.AnyAsync(ct).ConfigureAwait(false),
            StorageKey = $"letterheads/{Guid.NewGuid():N}.pdf",
        };
        db.Letterheads.Add(row);
        await documents
            .SaveAsync(row.StorageKey, memory.ToArray(), "application/pdf", ct)
            .ConfigureAwait(false);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return ToSnapshot(row);
    }

    public async Task<IReadOnlyList<LetterheadSnapshot>> ListAsync(CancellationToken ct) =>
        (
            await db
                .Letterheads.AsNoTracking()
                .OrderBy(row => row.Name)
                .ToListAsync(ct)
                .ConfigureAwait(false)
        )
            .Select(ToSnapshot)
            .ToArray();

    public async Task<bool> SetDefaultAsync(Guid id, CancellationToken ct)
    {
        var row = await db
            .Letterheads.FirstOrDefaultAsync(candidate => candidate.Id == id, ct)
            .ConfigureAwait(false);
        if (row is null)
            return false;
        await db
            .Letterheads.Where(candidate => candidate.IsDefault)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(candidate => candidate.IsDefault, false),
                ct
            )
            .ConfigureAwait(false);
        row.IsDefault = true;
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return true;
    }

    private static bool IsValid(CreateLetterheadRequest request) =>
        !string.IsNullOrWhiteSpace(request.Name)
        && !string.IsNullOrWhiteSpace(request.DocumentTypes)
        && request.PageWidthMillimetres > 0
        && request.PageHeightMillimetres > 0
        && request.MarginTopMillimetres >= 0
        && request.MarginRightMillimetres >= 0
        && request.MarginBottomMillimetres >= 0
        && request.MarginLeftMillimetres >= 0
        && request.MarginTopMillimetres < request.PageHeightMillimetres
        && request.MarginBottomMillimetres < request.PageHeightMillimetres
        && request.MarginLeftMillimetres < request.PageWidthMillimetres
        && request.MarginRightMillimetres < request.PageWidthMillimetres;

    private static LetterheadSnapshot ToSnapshot(LetterheadRow row) =>
        new(
            row.Id,
            row.Name,
            row.DocumentTypes,
            row.PageWidthMillimetres,
            row.PageHeightMillimetres,
            row.MarginTopMillimetres,
            row.MarginRightMillimetres,
            row.MarginBottomMillimetres,
            row.MarginLeftMillimetres,
            row.IsDefault,
            row.StorageKey
        );
}

#pragma warning restore CS1591
