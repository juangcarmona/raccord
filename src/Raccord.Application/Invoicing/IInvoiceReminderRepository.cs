using Raccord.Domain.Billing;

namespace Raccord.Application.Invoicing;

/// <summary>Append-only persistence of invoice payment reminders.</summary>
public interface IInvoiceReminderRepository
{
    /// <summary>Lists reminders for one invoice in generation order.</summary>
    Task<IReadOnlyList<InvoiceReminder>> ListAsync(
        Guid invoiceId,
        CancellationToken cancellationToken
    );

    /// <summary>Adds a generated reminder.</summary>
    void Add(InvoiceReminder reminder);
}
