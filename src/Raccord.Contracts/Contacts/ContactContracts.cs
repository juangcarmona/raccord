using Raccord.Contracts.Common;

namespace Raccord.Contracts.Contacts;

/// <summary>Creates or updates a shared company or private-person contact.</summary>
public sealed record SaveContactRequest(
    string Type,
    string LegalName,
    string? ContactNumber,
    string? TaxId,
    AddressDto? Address,
    string? Email,
    IReadOnlyList<string> Roles
);

/// <summary>A contact shown in the shared directory.</summary>
public sealed record ContactResponse(
    Guid Id,
    string Type,
    string ContactNumber,
    string LegalName,
    string? TaxId,
    AddressDto? Address,
    string? Email,
    IReadOnlyList<string> Roles,
    bool IsInvoiceable
);

/// <summary>A paginated directory result.</summary>
public sealed record ContactPageResponse(
    IReadOnlyList<ContactResponse> Items,
    int Page,
    int PageSize,
    int TotalCount,
    bool HasNextPage,
    bool HasPreviousPage
);
