using Microsoft.EntityFrameworkCore;
using Raccord.Contracts.Equipment;
using Raccord.Domain.Equipment;

#pragma warning disable CS1591

namespace Raccord.Infrastructure.Persistence;

public sealed class EquipmentCatalog(RaccordDbContext db)
{
    public async Task<EquipmentItemResponse> CreateAsync(
        CreateEquipmentItemRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = EquipmentItem.Create(request.Code, request.Name, request.StockQuantity);
        db.EquipmentItems.Add(item);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(item);
    }

    public async Task<IReadOnlyList<EquipmentItemResponse>> ListAsync(CancellationToken cancellationToken) =>
        await db.EquipmentItems.AsNoTracking().OrderBy(item => item.Code).Select(item => new EquipmentItemResponse(
            item.Id, item.Code, item.Name, item.ScanCode, item.StockQuantity, item.AvailableQuantity
        )).ToListAsync(cancellationToken).ConfigureAwait(false);

    public async Task<EquipmentItemResponse?> AssignScanCodeAsync(
        Guid id,
        AssignEquipmentScanCodeRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await db.EquipmentItems.FirstOrDefaultAsync(candidate => candidate.Id == id, cancellationToken).ConfigureAwait(false);
        if (item is null)
            return null;
        item.AssignScanCode(request.ScanCode);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(item);
    }

    private static EquipmentItemResponse ToResponse(EquipmentItem item) =>
        new(item.Id, item.Code, item.Name, item.ScanCode, item.StockQuantity, item.AvailableQuantity);
}

#pragma warning restore CS1591
