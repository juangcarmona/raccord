using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Raccord.BuildingBlocks.Storage;
using Raccord.Modules.Equipment.Contracts;

namespace Raccord.Infrastructure.Persistence;

#pragma warning disable CS1591

public sealed class EquipmentCatalog(RaccordDbContext db, IDocumentStore documents)
{
    public async Task<EquipmentItemSnapshot> CreateAsync(
        CreateEquipmentItemRequest request,
        CancellationToken cancellationToken
    )
    {
        var row = new EquipmentItemRow
        {
            Id = Guid.NewGuid(),
            Code = request.Code.Trim(),
            Name = request.Name.Trim(),
            Type = request.Type,
            StockQuantity = request.StockQuantity,
            SerialNumbers = JsonSerializer.Serialize(request.SerialNumbers ?? []),
        };
        db.EquipmentItems.Add(row);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToSnapshot(row);
    }

    public async Task<EquipmentItemPage> ListAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken
    )
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);
        var query = db.EquipmentItems.AsNoTracking().OrderBy(item => item.Code);
        var total = await query.CountAsync(cancellationToken).ConfigureAwait(false);
        var rows = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return new EquipmentItemPage(rows.Select(ToSnapshot).ToArray(), page, pageSize, total);
    }

    private static EquipmentItemSnapshot ToSnapshot(EquipmentItemRow row) =>
        new(
            row.Id,
            row.Code,
            row.Name,
            row.Type,
            row.StockQuantity,
            ParseSerialNumbers(row.SerialNumbers),
            row.IsPlanned,
            row.Details,
            row.ImageKey,
            ParseIds(row.AccessoryIds),
            ParseIds(row.AlternativeIds)
        );

    public async Task<EquipmentItemSnapshot?> UpdateDetailsAsync(
        Guid id,
        UpdateEquipmentItemDetailsRequest request,
        CancellationToken ct
    )
    {
        var row = await db
            .EquipmentItems.FirstOrDefaultAsync(item => item.Id == id, ct)
            .ConfigureAwait(false);
        if (row is null)
            return null;
        row.Details = request.Details?.Trim();
        row.AccessoryIds = JsonSerializer.Serialize(request.AccessoryIds ?? []);
        row.AlternativeIds = JsonSerializer.Serialize(request.AlternativeIds ?? []);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return ToSnapshot(row);
    }

    public async Task<EquipmentItemSnapshot?> SaveImageAsync(
        Guid id,
        string fileName,
        string contentType,
        byte[] content,
        CancellationToken ct
    )
    {
        if (
            content.Length is 0 or > 5_000_000
            || !contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)
        )
            return null;
        var row = await db
            .EquipmentItems.FirstOrDefaultAsync(item => item.Id == id, ct)
            .ConfigureAwait(false);
        if (row is null)
            return null;
        row.ImageKey = $"equipment/{id:N}/{Path.GetFileName(fileName)}";
        await documents.SaveAsync(row.ImageKey, content, contentType, ct).ConfigureAwait(false);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return ToSnapshot(row);
    }

    private static string[] ParseSerialNumbers(string value) =>
        JsonSerializer.Deserialize<string[]>(value) ?? [];

    private static Guid[] ParseIds(string value) => JsonSerializer.Deserialize<Guid[]>(value) ?? [];
}

#pragma warning restore CS1591
