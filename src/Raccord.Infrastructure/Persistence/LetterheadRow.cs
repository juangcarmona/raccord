namespace Raccord.Infrastructure.Persistence;

internal sealed class LetterheadRow
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DocumentTypes { get; set; } = string.Empty;
    public decimal PageWidthMillimetres { get; set; }
    public decimal PageHeightMillimetres { get; set; }
    public decimal MarginTopMillimetres { get; set; }
    public decimal MarginRightMillimetres { get; set; }
    public decimal MarginBottomMillimetres { get; set; }
    public decimal MarginLeftMillimetres { get; set; }
    public bool IsDefault { get; set; }
    public string StorageKey { get; set; } = string.Empty;
}
