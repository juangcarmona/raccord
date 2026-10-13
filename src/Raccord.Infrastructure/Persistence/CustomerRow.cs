using Raccord.SharedKernel.Parties;

namespace Raccord.Infrastructure.Persistence;

/// <summary>Persistence representation owned by the Contacts adapter.</summary>
internal sealed class CustomerRow
{
    public Guid Id { get; set; }

    public string LegalName { get; set; } = string.Empty;

    public string? TaxId { get; set; }

    public AddressFields BillingAddress { get; set; } = new();

    public string? Email { get; set; }
    public decimal DefaultDiscountPercent { get; set; }
    public string? DefaultPaymentTerms { get; set; }
    public string? Tags { get; set; }
    public string? Folder { get; set; }
}
