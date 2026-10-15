using Microsoft.EntityFrameworkCore;
using Raccord.Application.Invoicing;
using Raccord.Domain.Billing;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class InvoiceReminderRepository(RaccordDbContext db) : IInvoiceReminderRepository
{
    public async Task<IReadOnlyList<InvoiceReminder>> ListAsync(
        Guid invoiceId,
        CancellationToken cancellationToken
    ) =>
        await db
            .InvoiceReminders.Where(reminder => reminder.InvoiceId == invoiceId)
            .OrderBy(reminder => reminder.Number)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    public void Add(InvoiceReminder reminder) => db.InvoiceReminders.Add(reminder);
}
