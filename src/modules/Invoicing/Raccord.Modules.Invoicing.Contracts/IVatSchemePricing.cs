namespace Raccord.Modules.Invoicing.Contracts;

/// <summary>Resolves and marks a catalog rate used within the caller's unit of work.</summary>
public interface IVatSchemePricing
{
    /// <summary>Resolves a required class/scheme pair to its configured percentage.</summary>
    Task<VatRateSelection> ResolveRateAsync(
        Guid vatClassId,
        Guid vatSchemeId,
        CancellationToken cancellationToken
    );

    /// <summary>Marks the resolved scheme used before the caller commits its unit of work.</summary>
    void MarkUsed(Guid vatSchemeId);
}
