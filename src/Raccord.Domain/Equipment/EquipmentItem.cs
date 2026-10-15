using Raccord.SharedKernel.Common;

#pragma warning disable CS1591

namespace Raccord.Domain.Equipment;

public sealed class EquipmentItem
{
    private EquipmentItem() { }

    private EquipmentItem(string code, string name, int stockQuantity)
    {
        Id = Guid.CreateVersion7();
        Code = code;
        Name = name;
        StockQuantity = stockQuantity;
        AvailableQuantity = stockQuantity;
        Status = EquipmentLifecycleStatus.Active;
    }

    public Guid Id { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? ScanCode { get; private set; }
    public int StockQuantity { get; private set; }
    public int AvailableQuantity { get; private set; }
    public EquipmentLifecycleStatus Status { get; private set; }
    public DateTimeOffset? DisposedAt { get; private set; }
    public string? DispositionReason { get; private set; }
    public decimal? SaleAmount { get; private set; }

    public static EquipmentItem Create(string code, string name, int stockQuantity)
    {
        if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(name))
            throw new BusinessRuleViolationException(
                "equipment.identity-required",
                "Equipment code and name are required."
            );
        if (stockQuantity < 0)
            throw new BusinessRuleViolationException(
                "equipment.stock-invalid",
                "Stock cannot be negative."
            );
        return new(code.Trim(), name.Trim(), stockQuantity);
    }

    public void AssignScanCode(string scanCode)
    {
        if (string.IsNullOrWhiteSpace(scanCode))
            throw new BusinessRuleViolationException(
                "equipment.scan-code-required",
                "A scan code is required."
            );
        ScanCode = scanCode.Trim();
    }

    public void ValidateScanCode(string? scanCode)
    {
        if (
            scanCode is not null
            && !string.Equals(ScanCode, scanCode.Trim(), StringComparison.Ordinal)
        )
            throw new BusinessRuleViolationException(
                "equipment.scan-code-invalid",
                "The scanned equipment code does not match the item."
            );
    }

    public void BookOut(int quantity)
    {
        if (quantity <= 0 || quantity > AvailableQuantity)
            throw new BusinessRuleViolationException(
                "equipment.stock-unavailable",
                "The requested equipment is not available."
            );
        AvailableQuantity -= quantity;
    }

    public void Receive(int quantity)
    {
        if (quantity <= 0 || AvailableQuantity + quantity > StockQuantity)
            throw new BusinessRuleViolationException(
                "equipment.receive-invalid",
                "Received quantity is invalid."
            );
        AvailableQuantity += quantity;
    }

    public StockCount ReconcilePhysicalCount(
        int countedQuantity,
        string reason,
        DateTimeOffset countedAt
    )
    {
        var bookedQuantity = StockQuantity - AvailableQuantity;
        if (countedQuantity < bookedQuantity)
            throw new BusinessRuleViolationException(
                "equipment.count-below-booked",
                "A physical count cannot be lower than equipment already booked out."
            );
        var count = StockCount.Record(Id, countedQuantity, StockQuantity, reason, countedAt);
        StockQuantity = countedQuantity;
        AvailableQuantity = countedQuantity - bookedQuantity;
        return count;
    }

    public void Retire(string reason, DateTimeOffset disposedAt)
    {
        EnsureDisposable();
        if (string.IsNullOrWhiteSpace(reason))
            throw new BusinessRuleViolationException(
                "equipment.retirement-reason-required",
                "A retirement reason is required."
            );
        Status = EquipmentLifecycleStatus.Retired;
        DisposedAt = disposedAt;
        DispositionReason = reason.Trim();
        SaleAmount = null;
    }

    public void Sell(decimal amount, string? buyer, DateTimeOffset disposedAt)
    {
        EnsureDisposable();
        if (amount <= 0m)
            throw new BusinessRuleViolationException(
                "equipment.sale-amount-positive",
                "A sale amount must be positive."
            );
        Status = EquipmentLifecycleStatus.Sold;
        DisposedAt = disposedAt;
        DispositionReason = buyer?.Trim();
        SaleAmount = amount;
    }

    private void EnsureDisposable()
    {
        if (Status != EquipmentLifecycleStatus.Active)
            throw new BusinessRuleViolationException(
                "equipment.already-disposed",
                "Only active equipment can be sold or retired."
            );
        if (AvailableQuantity != StockQuantity)
            throw new BusinessRuleViolationException(
                "equipment.booked-out",
                "Equipment must be fully returned before it can be sold or retired."
            );
    }
}

#pragma warning restore CS1591
