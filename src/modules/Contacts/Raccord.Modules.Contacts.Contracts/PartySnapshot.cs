using Raccord.SharedKernel.Parties;

namespace Raccord.Modules.Contacts.Contracts;

/// <summary>Read-only legal identity data published by Contacts.</summary>
public sealed record PartySnapshot(
    Guid Id,
    string LegalName,
    string? TaxId,
    PostalAddress? Address,
    string? Email,
    decimal DefaultDiscountPercent = 0,
    string? DefaultPaymentTerms = null
)
{
    /// <summary>The party data required by an invoice.</summary>
    public LegalParty ToLegalParty() => new(LegalName, TaxId, Address);

    /// <summary>Whether the party has the required invoice details.</summary>
    public bool IsInvoiceable => TaxId is not null && Address is { IsComplete: true };
}
