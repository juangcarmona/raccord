using Raccord.Contracts.Common;

namespace Raccord.Contracts.Contacts;

/// <summary>Operational role of a contact.</summary>
public enum ContactType
{
    /// <summary>A customer billed for work.</summary>
    Customer,

    /// <summary>A supplier providing goods or services.</summary>
    Supplier,

    /// <summary>A venue used by a project.</summary>
    Venue,
}

/// <summary>Creates a contact.</summary>
/// <param name="LegalName">Display or registered name.</param>
/// <param name="Type">Operational role.</param>
/// <param name="TaxId">Tax identifier.</param>
/// <param name="Address">Postal address.</param>
/// <param name="Email">Contact e-mail.</param>
public sealed record RegisterContactRequest(
    string LegalName,
    ContactType Type,
    string? TaxId,
    AddressDto? Address,
    string? Email
);

/// <summary>Changes a contact.</summary>
/// <param name="LegalName">Display or registered name.</param>
/// <param name="Type">Operational role.</param>
/// <param name="TaxId">Tax identifier.</param>
/// <param name="Address">Postal address.</param>
/// <param name="Email">Contact e-mail.</param>
public sealed record UpdateContactRequest(
    string LegalName,
    ContactType Type,
    string? TaxId,
    AddressDto? Address,
    string? Email
);

/// <summary>A contact in the contact database.</summary>
/// <param name="Id">Contact identifier.</param>
/// <param name="LegalName">Display or registered name.</param>
/// <param name="Type">Operational role.</param>
/// <param name="TaxId">Tax identifier.</param>
/// <param name="Address">Postal address.</param>
/// <param name="Email">Contact e-mail.</param>
/// <param name="IsInvoiceable">Whether legal details are complete.</param>
public sealed record ContactResponse(
    Guid Id,
    string LegalName,
    ContactType Type,
    string? TaxId,
    AddressDto? Address,
    string? Email,
    bool IsInvoiceable
);

/// <summary>A page of contacts.</summary>
/// <param name="Items">Contacts in this page.</param>
/// <param name="Page">One-based page number.</param>
/// <param name="PageSize">Requested page size.</param>
/// <param name="TotalCount">Total contacts.</param>
public sealed record ContactPage(
    IReadOnlyList<ContactResponse> Items,
    int Page,
    int PageSize,
    int TotalCount
);
