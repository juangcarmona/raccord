namespace Raccord.Infrastructure.Persistence;

internal sealed class EquipmentStockMovementRow
{
    public Guid Id { get; set; }
    public Guid EquipmentItemId { get; set; }
    public int Quantity { get; set; }
    public int StockQuantityAfter { get; set; }
    public DateOnly EffectiveDate { get; set; }
    public string Kind { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
