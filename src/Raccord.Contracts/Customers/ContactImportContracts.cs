#pragma warning disable CS1591

namespace Raccord.Contracts.Customers;

public sealed record ContactImportRowResponse(
    int Row,
    bool Imported,
    string? Error,
    CustomerResponse? Contact
);

public sealed record ContactImportResponse(
    int ImportedCount,
    int RejectedCount,
    IReadOnlyList<ContactImportRowResponse> Rows
);

#pragma warning restore CS1591
