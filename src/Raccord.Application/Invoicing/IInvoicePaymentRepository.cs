using Raccord.Domain.Billing;

namespace Raccord.Application.Invoicing;

/// <summary>Persistence port for invoice payment history.</summary>
public interface IInvoicePaymentRepository
{
    /// <summary>Lists payments for an issued invoice.</summary>
    Task<IReadOnlyList<InvoicePayment>> ListAsync(
        Guid invoiceId,
        CancellationToken cancellationToken
    );

    /// <summary>Adds a payment to the current unit of work.</summary>
    void Add(InvoicePayment payment);
}
