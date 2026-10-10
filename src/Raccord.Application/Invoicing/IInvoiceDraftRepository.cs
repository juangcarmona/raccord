using Raccord.Domain.Billing;

namespace Raccord.Application.Invoicing;

/// <summary>Persistence of invoice drafts.</summary>
public interface IInvoiceDraftRepository
{
    /// <summary>Finds a draft with its lines.</summary>
    Task<InvoiceDraft?> FindAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>Lists open drafts, newest first.</summary>
    Task<IReadOnlyList<InvoiceDraft>> ListOpenAsync(CancellationToken cancellationToken);

    /// <summary>Adds a new draft.</summary>
    void Add(InvoiceDraft draft);

    /// <summary>Deletes an open draft.</summary>
    void Remove(InvoiceDraft draft);
}
