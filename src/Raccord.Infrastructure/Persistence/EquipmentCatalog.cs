using Microsoft.EntityFrameworkCore;
using Raccord.Contracts.Equipment;
using Raccord.Domain.Equipment;
using Raccord.Modules.Projects.Contracts;
using Raccord.SharedKernel.Common;

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
        (
            await db
                .EquipmentItems.AsNoTracking()
                .OrderBy(item => item.Code)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false)
        )
            .Select(ToResponse)
            .ToList();

    public async Task<EquipmentItemResponse?> FindAsync(
        Guid id,
        CancellationToken cancellationToken
    ) =>
        (
            await db
                .EquipmentItems.AsNoTracking()
                .FirstOrDefaultAsync(item => item.Id == id, cancellationToken)
                .ConfigureAwait(false)
        )
            is { } item
            ? ToResponse(item)
            : null;

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

    public async Task<StockMovementResponse?> ScheduleMovementAsync(
        Guid equipmentId,
        ScheduleStockMovementRequest request,
        DateTimeOffset createdAt,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        if (
            !Enum.TryParse<StockMovementDirection>(
                request.Direction,
                ignoreCase: true,
                out var direction
            )
        )
            throw new BusinessRuleViolationException(
                "stock-movement.direction-invalid",
                "Direction must be inbound or outbound."
            );
        if (
            await db
                .EquipmentItems.AnyAsync(item => item.Id == equipmentId, cancellationToken)
                .ConfigureAwait(false)
            is false
        )
            return null;
        var movement = StockMovement.Schedule(
            equipmentId,
            direction,
            request.Quantity,
            request.ScheduledDate,
            request.Reference,
            createdAt
        );
        db.StockMovements.Add(movement);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(movement);
    }

    public async Task<IReadOnlyList<StockMovementResponse>> ListMovementsAsync(
        Guid equipmentId,
        CancellationToken cancellationToken
    ) =>
        await db
            .StockMovements.AsNoTracking()
            .Where(movement => movement.EquipmentId == equipmentId)
            .OrderBy(movement => movement.ScheduledDate)
            .ThenBy(movement => movement.CreatedAt)
            .Select(movement => new StockMovementResponse(
                movement.Id,
                movement.EquipmentId,
                movement.Direction.ToString().ToLowerInvariant(),
                movement.Quantity,
                movement.ScheduledDate,
                movement.Reference,
                movement.Status.ToString().ToLowerInvariant(),
                movement.CreatedAt
            ))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    public async Task<StockCountResponse?> ReconcileCountAsync(
        Guid equipmentId,
        ReconcileStockCountRequest request,
        DateTimeOffset countedAt,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await FindEntityAsync(equipmentId, cancellationToken).ConfigureAwait(false);
        if (item is null)
            return null;
        var count = item.ReconcilePhysicalCount(request.CountedQuantity, request.Reason, countedAt);
        db.StockCounts.Add(count);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(count);
    }

    public async Task<IReadOnlyList<StockCountResponse>> ListCountsAsync(
        Guid equipmentId,
        CancellationToken cancellationToken
    ) =>
        await db
            .StockCounts.AsNoTracking()
            .Where(count => count.EquipmentId == equipmentId)
            .OrderByDescending(count => count.CountedAt)
            .Select(count => new StockCountResponse(
                count.Id,
                count.EquipmentId,
                count.CountedQuantity,
                count.PreviousQuantity,
                count.Difference,
                count.Reason,
                count.CountedAt
            ))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    public async Task<EquipmentItemResponse?> RetireAsync(
        Guid id,
        RetireEquipmentRequest request,
        DateTimeOffset disposedAt,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await FindEntityAsync(id, cancellationToken).ConfigureAwait(false);
        if (item is null)
            return null;
        item.Retire(request.Reason, disposedAt);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(item);
    }

    public async Task<EquipmentItemResponse?> SellAsync(
        Guid id,
        SellEquipmentRequest request,
        DateTimeOffset disposedAt,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await FindEntityAsync(id, cancellationToken).ConfigureAwait(false);
        if (item is null)
            return null;
        item.Sell(request.Amount, request.Buyer, disposedAt);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(item);
    }

    public async Task<IReadOnlyList<EquipmentProjectHistoryResponse>> ListHistoryAsync(
        Guid equipmentId,
        CancellationToken cancellationToken
    )
    {
        var projects = await db
            .Projects.AsNoTracking()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return projects
            .SelectMany(project =>
                project
                    .PackingLines.Where(line => line.EquipmentId == equipmentId)
                    .Select(line => new EquipmentProjectHistoryResponse(
                        project.Id,
                        project.Name,
                        line.Id,
                        line.Quantity,
                        line.Role.ToString().ToLowerInvariant(),
                        line.IsPacked,
                        line.IsBookedOut,
                        line.IsReceived
                    ))
            )
            .OrderBy(history => history.ProjectName, StringComparer.Ordinal)
            .ThenBy(history => history.PackingLineId)
            .ToList();
    }

    public async Task<EquipmentInsightsResponse?> GetInsightsAsync(
        Guid equipmentId,
        CancellationToken cancellationToken
    )
    {
        var item = await FindEntityAsync(equipmentId, cancellationToken).ConfigureAwait(false);
        if (item is null)
            return null;
        var history = await ListHistoryAsync(equipmentId, cancellationToken).ConfigureAwait(false);
        return new EquipmentInsightsResponse(
            equipmentId,
            await db
                .StockMovements.CountAsync(
                    movement => movement.EquipmentId == equipmentId,
                    cancellationToken
                )
                .ConfigureAwait(false),
            await db
                .StockCounts.CountAsync(
                    count => count.EquipmentId == equipmentId,
                    cancellationToken
                )
                .ConfigureAwait(false),
            history.Select(entry => entry.ProjectId).Distinct().Count(),
            history.Sum(entry => entry.Quantity),
            item.Status.ToString().ToLowerInvariant()
        );
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
            item.AvailableQuantity,
            item.Status.ToString().ToLowerInvariant(),
            item.DisposedAt,
            item.DispositionReason,
            item.SaleAmount
        );

    private static StockMovementResponse ToResponse(StockMovement movement) =>
        new(
            movement.Id,
            movement.EquipmentId,
            movement.Direction.ToString().ToLowerInvariant(),
            movement.Quantity,
            movement.ScheduledDate,
            movement.Reference,
            movement.Status.ToString().ToLowerInvariant(),
            movement.CreatedAt
        );

    private static StockCountResponse ToResponse(StockCount count) =>
        new(
            count.Id,
            count.EquipmentId,
            count.CountedQuantity,
            count.PreviousQuantity,
            count.Difference,
            count.Reason,
            count.CountedAt
        );
}

#pragma warning restore CS1591
