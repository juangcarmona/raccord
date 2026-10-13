using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Raccord.Modules.Equipment.Contracts;

namespace Raccord.Infrastructure.Persistence;

#pragma warning disable CS1591

public sealed class EquipmentCatalog(RaccordDbContext db)
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
            row.IsPlanned
        );

    private static string[] ParseSerialNumbers(string value) =>
        JsonSerializer.Deserialize<string[]>(value) ?? [];
}

#pragma warning restore CS1591
