using Raccord.Domain.Billing;

namespace Raccord.Application.Invoicing;

/// <summary>Persistence operations for the installation-scoped VAT catalog.</summary>
public interface IVatCatalogRepository
{
    /// <summary>Lists VAT classes in stable display order.</summary>
    Task<IReadOnlyList<VatClass>> ListClassesAsync(CancellationToken cancellationToken);

    /// <summary>Finds one VAT class by stable identity.</summary>
    Task<VatClass?> FindClassAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>Adds a VAT class.</summary>
    void Add(VatClass vatClass);

    /// <summary>Lists VAT schemes in stable display order.</summary>
    Task<IReadOnlyList<VatScheme>> ListSchemesAsync(CancellationToken cancellationToken);

    /// <summary>Finds one VAT scheme by stable identity.</summary>
    Task<VatScheme?> FindSchemeAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>Adds a VAT scheme.</summary>
    void Add(VatScheme scheme);
}
