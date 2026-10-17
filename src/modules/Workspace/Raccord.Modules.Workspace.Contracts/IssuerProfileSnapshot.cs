using Raccord.SharedKernel.Parties;

namespace Raccord.Modules.Workspace.Contracts;

/// <summary>Issuer legal details published by Workspace.</summary>
public sealed record IssuerProfileSnapshot(
    Guid Id,
    string LegalName,
    string TaxId,
    PostalAddress Address,
    int IssuedInvoiceCount = 0
)
{
    /// <summary>Whether an invoice has been issued for this issuer profile.</summary>
    public bool HasIssuedInvoice => IssuedInvoiceCount > 0;

    /// <summary>Converts the current issuer details to invoice identity data.</summary>
    public LegalParty ToLegalParty() => new(LegalName, TaxId, Address);
}
