namespace Raccord.Infrastructure.Persistence;

/// <summary>Persistence representation of a Workspace invoice series.</summary>
internal sealed class InvoiceSeriesRow
{
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
}
