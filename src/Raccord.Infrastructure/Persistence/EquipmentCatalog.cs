using Microsoft.EntityFrameworkCore;
using Raccord.Contracts.Equipment;
using Raccord.Domain.Equipment;
using Raccord.Modules.Projects.Contracts;

#pragma warning disable CS1591

namespace Raccord.Infrastructure.Persistence;

public sealed class EquipmentCatalog(RaccordDbContext db) : IEquipmentStock
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

    public async Task<IReadOnlyList<EquipmentItemResponse>> ListAsync(
        CancellationToken cancellationToken
    ) =>
        await db
            .EquipmentItems.AsNoTracking()
            .OrderBy(item => item.Code)
            .Select(item => new EquipmentItemResponse(
                item.Id,
                item.Code,
                item.Name,
                item.ScanCode,
                item.StockQuantity,
                item.AvailableQuantity
            ))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    public async Task<EquipmentItemResponse?> FindAsync(
        Guid id,
        CancellationToken cancellationToken
    ) =>
        await db
            .EquipmentItems.AsNoTracking()
            .Where(item => item.Id == id)
            .Select(item => new EquipmentItemResponse(
                item.Id,
                item.Code,
                item.Name,
                item.ScanCode,
                item.StockQuantity,
                item.AvailableQuantity
            ))
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

    public async Task<EquipmentItemResponse?> AssignScanCodeAsync(
        Guid id,
        AssignEquipmentScanCodeRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await db
            .EquipmentItems.FirstOrDefaultAsync(candidate => candidate.Id == id, cancellationToken)
            .ConfigureAwait(false);
        if (item is null)
            return null;
        item.AssignScanCode(request.ScanCode);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(item);
    }

    public async Task BookOutAsync(
        Guid equipmentId,
        int quantity,
        string? scanCode,
        CancellationToken cancellationToken
    )
    {
        var item = await FindEntityAsync(equipmentId, cancellationToken).ConfigureAwait(false);
        if (item is null)
            throw new InvalidOperationException($"Equipment '{equipmentId}' does not exist.");
        item.ValidateScanCode(scanCode);
        item.BookOut(quantity);
    }

    public async Task ReceiveAsync(
        Guid equipmentId,
        int quantity,
        CancellationToken cancellationToken
    )
    {
        var item = await FindEntityAsync(equipmentId, cancellationToken).ConfigureAwait(false);
        if (item is null)
            throw new InvalidOperationException($"Equipment '{equipmentId}' does not exist.");
        item.Receive(quantity);
    }

    private Task<EquipmentItem?> FindEntityAsync(Guid id, CancellationToken cancellationToken) =>
        db.EquipmentItems.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

    private static EquipmentItemResponse ToResponse(EquipmentItem item) =>
        new(
            item.Id,
            item.Code,
            item.Name,
            item.ScanCode,
            item.StockQuantity,
            item.AvailableQuantity
        );
}

#pragma warning restore CS1591
