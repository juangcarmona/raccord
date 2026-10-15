using Raccord.SharedKernel.Common;

namespace Raccord.Domain.Equipment;

#pragma warning disable CS1591

public sealed class StockMovement
{
    private StockMovement() { }

    private StockMovement(
        Guid equipmentId,
        StockMovementDirection direction,
        int quantity,
        DateOnly scheduledDate,
        string? reference,
        DateTimeOffset createdAt
    )
    {
        Id = Guid.CreateVersion7();
        EquipmentId = equipmentId;
        Direction = direction;
        Quantity = quantity;
        ScheduledDate = scheduledDate;
        Reference = reference?.Trim();
        Status = StockMovementStatus.Planned;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public Guid EquipmentId { get; private set; }
    public StockMovementDirection Direction { get; private set; }
    public int Quantity { get; private set; }
    public DateOnly ScheduledDate { get; private set; }
    public string? Reference { get; private set; }
    public StockMovementStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public static StockMovement Schedule(
        Guid equipmentId,
        StockMovementDirection direction,
        int quantity,
        DateOnly scheduledDate,
        string? reference,
        DateTimeOffset createdAt
    )
    {
        if (equipmentId == Guid.Empty)
            throw new BusinessRuleViolationException(
                "stock-movement.equipment-required",
                "An equipment item is required."
            );
        if (!Enum.IsDefined(direction))
            throw new BusinessRuleViolationException(
                "stock-movement.direction-invalid",
                "A valid stock movement direction is required."
            );
        if (quantity <= 0)
            throw new BusinessRuleViolationException(
                "stock-movement.quantity-positive",
                "A stock movement quantity must be positive."
            );
        if (scheduledDate == default)
            throw new BusinessRuleViolationException(
                "stock-movement.date-required",
                "A stock movement date is required."
            );
        return new(equipmentId, direction, quantity, scheduledDate, reference, createdAt);
    }
}

#pragma warning restore CS1591
