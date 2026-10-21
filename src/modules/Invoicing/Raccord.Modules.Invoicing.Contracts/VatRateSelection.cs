namespace Raccord.Modules.Invoicing.Contracts;

/// <summary>Pricing selection returned by the Invoicing catalog to a consuming module.</summary>
public sealed record VatRateSelection(Guid VatClassId, Guid VatSchemeId, decimal Percentage);
