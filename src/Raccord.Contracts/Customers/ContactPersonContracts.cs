#pragma warning disable CS1591

namespace Raccord.Contracts.Customers;

public sealed record CreateContactPersonRequest(
    string Name,
    string Role,
    string? Email,
    string? Phone,
    bool IsDefault = false
);

public sealed record ContactPersonResponse(
    Guid Id,
    Guid CustomerId,
    string Name,
    string Role,
    string? Email,
    string? Phone,
    bool IsDefault
);

#pragma warning restore CS1591
