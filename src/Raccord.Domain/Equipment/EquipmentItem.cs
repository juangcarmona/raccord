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
    }

    public Guid Id { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? ScanCode { get; private set; }
    public int StockQuantity { get; private set; }
    public int AvailableQuantity { get; private set; }

    public static EquipmentItem Create(string code, string name, int stockQuantity)
    {
        if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(name))
            throw new BusinessRuleViolationException("equipment.identity-required", "Equipment code and name are required.");
        if (stockQuantity < 0)
            throw new BusinessRuleViolationException("equipment.stock-invalid", "Stock cannot be negative.");
        return new(code.Trim(), name.Trim(), stockQuantity);
    }

    public void AssignScanCode(string scanCode)
    {
        if (string.IsNullOrWhiteSpace(scanCode))
            throw new BusinessRuleViolationException("equipment.scan-code-required", "A scan code is required.");
        ScanCode = scanCode.Trim();
    }

    public void BookOut(int quantity)
    {
        if (quantity <= 0 || quantity > AvailableQuantity)
            throw new BusinessRuleViolationException("equipment.stock-unavailable", "The requested equipment is not available.");
        AvailableQuantity -= quantity;
    }

    public void Receive(int quantity)
    {
        if (quantity <= 0 || AvailableQuantity + quantity > StockQuantity)
            throw new BusinessRuleViolationException("equipment.receive-invalid", "Received quantity is invalid.");
        AvailableQuantity += quantity;
    }
}

#pragma warning restore CS1591
