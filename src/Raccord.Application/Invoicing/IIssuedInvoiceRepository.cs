using Raccord.Domain.Billing;

namespace Raccord.Application.Invoicing;

/// <summary>Append-only persistence of issued invoices.</summary>
public interface IIssuedInvoiceRepository
{
    /// <summary>Adds an issued invoice with its canonical representation. There is no update or delete.</summary>
    void Add(IssuedInvoice invoice, IssuedInvoiceRepresentation representation);

    /// <summary>Loads an invoice, verifying its content hash.</summary>
    Task<IssuedInvoiceRecord?> FindAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>Lists issued invoices, newest first.</summary>
    Task<IReadOnlyList<IssuedInvoiceListItem>> ListAsync(CancellationToken cancellationToken);
}
