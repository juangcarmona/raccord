namespace Raccord.Modules.Workspace.Contracts;

/// <summary>Reads invoice series definitions owned by Workspace.</summary>
public interface IInvoiceSeriesCatalog
{
    /// <summary>Lists invoice series by code.</summary>
    Task<IReadOnlyList<InvoiceSeriesSnapshot>> ListAsync(CancellationToken cancellationToken);

    /// <summary>Finds an invoice series by code.</summary>
    Task<InvoiceSeriesSnapshot?> FindAsync(string code, CancellationToken cancellationToken);
}
