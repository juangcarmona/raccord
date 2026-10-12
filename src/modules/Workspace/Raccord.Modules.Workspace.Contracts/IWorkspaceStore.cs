namespace Raccord.Modules.Workspace.Contracts;

/// <summary>Persistence port used by Workspace use cases and maintenance.</summary>
public interface IWorkspaceStore : IInvoiceSeriesCatalog
{
    /// <summary>Reads the issuer profile.</summary>
    Task<IssuerProfileSnapshot?> FindIssuerAsync(CancellationToken cancellationToken);

    /// <summary>Adds an issuer profile.</summary>
    void AddIssuer(IssuerProfileSnapshot issuer);

    /// <summary>Updates an issuer profile.</summary>
    void UpdateIssuer(IssuerProfileSnapshot issuer);

    /// <summary>Adds an invoice series.</summary>
    void AddSeries(InvoiceSeriesSnapshot series);
}
