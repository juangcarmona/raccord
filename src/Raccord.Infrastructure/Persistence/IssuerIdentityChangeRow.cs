namespace Raccord.Infrastructure.Persistence;

/// <summary>Persistence representation of a retained issuer identity review.</summary>
internal sealed class IssuerIdentityChangeRow
{
    public Guid Id { get; set; }
    public Guid IssuerId { get; set; }
    public string OldLegalName { get; set; } = string.Empty;
    public string OldTaxId { get; set; } = string.Empty;
    public string ProposedLegalName { get; set; } = string.Empty;
    public string ProposedTaxId { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string RequestedById { get; set; } = string.Empty;
    public string RequestedByName { get; set; } = string.Empty;
    public DateTimeOffset RequestedAt { get; set; }
    public string Status { get; set; } = "pending";
    public string? DecidedById { get; set; }
    public string? DecidedByName { get; set; }
    public DateTimeOffset? DecidedAt { get; set; }
}
