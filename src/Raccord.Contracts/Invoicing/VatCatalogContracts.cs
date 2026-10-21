namespace Raccord.Contracts.Invoicing;

/// <summary>A reusable VAT classification.</summary>
public sealed record VatClassResponse(Guid Id, string Name);

/// <summary>One VAT percentage mapped to a stable class identity.</summary>
public sealed record VatClassRateResponse(Guid VatClassId, decimal Percentage);

/// <summary>A configured VAT scheme and its source-date metadata.</summary>
public sealed record VatSchemeResponse(
    Guid Id,
    string Name,
    string Kind,
    decimal? FixedPercentage,
    bool IsDefault,
    bool IsUsed,
    string? Territory,
    string? SourceUrl,
    DateOnly? VerifiedOn,
    IReadOnlyList<VatClassRateResponse> ClassRates
);

/// <summary>Creates a VAT class.</summary>
public sealed record CreateVatClassRequest(string Name);

/// <summary>Changes a VAT class display name.</summary>
public sealed record RenameVatClassRequest(string Name);

/// <summary>Creates a VAT scheme.</summary>
public sealed record CreateVatSchemeRequest(
    string Name,
    string Kind,
    decimal? FixedPercentage,
    IReadOnlyList<VatClassRateRequest>? ClassRates
);

/// <summary>Changes an unused VAT scheme rate and display name.</summary>
public sealed record UpdateVatSchemeRequest(
    string Name,
    decimal? FixedPercentage,
    IReadOnlyList<VatClassRateRequest>? ClassRates
);

/// <summary>Maps one VAT class to a percentage.</summary>
public sealed record VatClassRateRequest(Guid VatClassId, decimal Percentage);
