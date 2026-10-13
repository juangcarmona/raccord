namespace Raccord.Contracts.Organization;

/// <summary>Updates company-wide quote and contract validity settings.</summary>
public sealed record UpdateFinancialSettingsRequest(
    int QuoteValidityDays,
    int ContractValidityDays,
    bool AllowSigningAfterExpiry
);

/// <summary>Company-wide quote and contract validity settings.</summary>
public sealed record FinancialSettingsResponse(
    int QuoteValidityDays,
    int ContractValidityDays,
    bool AllowSigningAfterExpiry
);
