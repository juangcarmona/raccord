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

    /// <summary>Marks the issuer row when an invoice is issued, sharing its concurrency boundary.</summary>
    void MarkInvoiceIssued();

    /// <summary>Lists retained issuer identity change requests, newest first.</summary>
    Task<IReadOnlyList<IssuerIdentityChangeSnapshot>> ListIssuerIdentityChangesAsync(
        CancellationToken cancellationToken
    );

    /// <summary>Loads one issuer identity request for a decision.</summary>
    Task<IssuerIdentityChangeSnapshot?> FindIssuerIdentityChangeAsync(
        Guid id,
        CancellationToken cancellationToken
    );

    /// <summary>Adds an issuer identity change request.</summary>
    void AddIssuerIdentityChange(IssuerIdentityChangeSnapshot request);

    /// <summary>Updates a previously loaded issuer identity change request.</summary>
    void UpdateIssuerIdentityChange(IssuerIdentityChangeSnapshot request);

    /// <summary>Adds an invoice series.</summary>
    void AddSeries(InvoiceSeriesSnapshot series);
}
