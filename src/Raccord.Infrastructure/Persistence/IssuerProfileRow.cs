using Raccord.SharedKernel.Parties;

namespace Raccord.Infrastructure.Persistence;

/// <summary>Persistence representation owned by the Workspace adapter.</summary>
internal sealed class IssuerProfileRow
{
    public Guid Id { get; set; }
    public string LegalName { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public int IssuedInvoiceCount { get; set; }
    public AddressFields Address { get; set; } = new();
}
