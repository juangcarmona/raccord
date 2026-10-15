namespace Raccord.Application.Invoicing;

/// <summary>Persists inbound structured invoices and their reportable statuses.</summary>
public interface IInboundEInvoiceRepository
{
    /// <summary>Adds a newly received inbound invoice.</summary>
    void Add(InboundEInvoice invoice);

    /// <summary>Finds an inbound invoice by identifier.</summary>
    Task<InboundEInvoice?> FindAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>Lists inbound invoices in received order.</summary>
    Task<IReadOnlyList<InboundEInvoice>> ListAsync(CancellationToken cancellationToken);

    /// <summary>Updates the invoice's status and reporting details.</summary>
    void Update(InboundEInvoice invoice);
}
