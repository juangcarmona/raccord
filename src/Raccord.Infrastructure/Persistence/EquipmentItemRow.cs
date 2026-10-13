using Raccord.Modules.Equipment.Contracts;

namespace Raccord.Infrastructure.Persistence;

internal sealed class EquipmentItemRow
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public EquipmentItemType Type { get; set; }
    public int StockQuantity { get; set; }
    public string SerialNumbers { get; set; } = "[]";
    public bool IsPlanned { get; set; }
}
