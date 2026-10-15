using Microsoft.EntityFrameworkCore;
using Raccord.Application.Invoicing;
using Raccord.Domain.Billing;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class InvoicePaymentRepository(RaccordDbContext db) : IInvoicePaymentRepository
{
    public async Task<IReadOnlyList<InvoicePayment>> ListAsync(
        Guid invoiceId,
        CancellationToken cancellationToken
    ) =>
        await db
            .InvoicePayments.Where(payment => payment.InvoiceId == invoiceId)
            .OrderBy(payment => payment.Date)
            .ThenBy(payment => payment.RecordedAt)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    public void Add(InvoicePayment payment) => db.InvoicePayments.Add(payment);
}
