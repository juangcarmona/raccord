using Raccord.Contracts.Common;

namespace Raccord.Contracts.Customers;

/// <summary>Registers a customer.</summary>
/// <param name="LegalName">Registered legal name.</param>
/// <param name="TaxId">Tax ID; required before the customer can be invoiced.</param>
/// <param name="Address">Billing address; required before the customer can be invoiced.</param>
/// <param name="Email">Billing e-mail.</param>
/// <param name="DefaultDiscountPercent">Discount copied into new projects.</param>
/// <param name="DefaultPaymentTerms">Payment terms copied into new projects.</param>
public sealed record RegisterCustomerRequest(
    string LegalName,
    string? TaxId,
    AddressDto? Address,
    string? Email,
    decimal DefaultDiscountPercent = 0,
    string? DefaultPaymentTerms = null
);

/// <summary>A customer.</summary>
/// <param name="Id">Customer identifier.</param>
/// <param name="LegalName">Registered legal name.</param>
/// <param name="TaxId">Normalized tax ID.</param>
/// <param name="Address">Billing address.</param>
/// <param name="Email">Billing e-mail.</param>
/// <param name="IsInvoiceable">Whether the legal details required on an invoice are complete.</param>
/// <param name="DefaultDiscountPercent">Discount copied into new projects.</param>
/// <param name="DefaultPaymentTerms">Payment terms copied into new projects.</param>
public sealed record CustomerResponse(
    Guid Id,
    string LegalName,
    string? TaxId,
    AddressDto? Address,
    string? Email,
    bool IsInvoiceable,
    decimal DefaultDiscountPercent,
    string? DefaultPaymentTerms
);
