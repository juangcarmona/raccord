namespace Raccord.Domain.Billing;

/// <summary>Source and verification metadata for one installation-time VAT default.</summary>
public sealed record VatRateBootstrap(
    string Territory,
    decimal Percentage,
    string SourceUrl,
    DateOnly VerifiedOn
);
