using Raccord.SharedKernel.Parties;

namespace Raccord.Modules.Contacts.Contracts;

/// <summary>Persisted directory values required by Contacts and its consumers.</summary>
public sealed record ContactSnapshot(
    Guid Id,
    ContactType Type,
    string? ContactNumber,
    string LegalName,
    string? TaxId,
    PostalAddress? Address,
    string? Email,
    IReadOnlySet<ContactRole> Roles
)
{
    /// <summary>The party data required by an invoice.</summary>
    public LegalParty ToLegalParty() => new(LegalName, TaxId, Address);

    /// <summary>Whether the legal details required on an invoice are complete.</summary>
    public bool IsInvoiceable => TaxId is not null && Address is { IsComplete: true };
}
