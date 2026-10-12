using Raccord.SharedKernel.Parties;

namespace Raccord.Modules.Workspace.Contracts;

/// <summary>Issuer legal details published by Workspace.</summary>
public sealed record IssuerProfileSnapshot(
    Guid Id,
    string LegalName,
    string TaxId,
    PostalAddress Address
)
{
    /// <summary>Converts the current issuer details to invoice identity data.</summary>
    public LegalParty ToLegalParty() => new(LegalName, TaxId, Address);
}
