using Raccord.SharedKernel.Common;

namespace Raccord.Domain.Equipment;

#pragma warning disable CS1591

public sealed class StockCount
{
    private StockCount() { }

    private StockCount(
        Guid equipmentId,
        int countedQuantity,
        int previousQuantity,
        string reason,
        DateTimeOffset countedAt
    )
    {
        Id = Guid.CreateVersion7();
        EquipmentId = equipmentId;
        CountedQuantity = countedQuantity;
        PreviousQuantity = previousQuantity;
        Difference = countedQuantity - previousQuantity;
        Reason = reason.Trim();
        CountedAt = countedAt;
    }

    public Guid Id { get; private set; }
    public Guid EquipmentId { get; private set; }
    public int CountedQuantity { get; private set; }
    public int PreviousQuantity { get; private set; }
    public int Difference { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public DateTimeOffset CountedAt { get; private set; }

    public static StockCount Record(
        Guid equipmentId,
        int countedQuantity,
        int previousQuantity,
        string reason,
        DateTimeOffset countedAt
    )
    {
        if (equipmentId == Guid.Empty)
            throw new BusinessRuleViolationException(
                "stock-count.equipment-required",
                "An equipment item is required."
            );
        if (countedQuantity < 0)
            throw new BusinessRuleViolationException(
                "stock-count.quantity-invalid",
                "A physical count cannot be negative."
            );
        if (string.IsNullOrWhiteSpace(reason))
            throw new BusinessRuleViolationException(
                "stock-count.reason-required",
                "A reason is required for every stock correction."
            );
        return new(equipmentId, countedQuantity, previousQuantity, reason, countedAt);
    }
}

#pragma warning restore CS1591
