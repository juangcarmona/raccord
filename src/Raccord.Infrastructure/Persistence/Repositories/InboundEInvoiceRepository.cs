using Microsoft.EntityFrameworkCore;
using Raccord.Application.Invoicing;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class InboundEInvoiceRepository(RaccordDbContext db) : IInboundEInvoiceRepository
{
    public void Add(InboundEInvoice invoice) => db.InboundEInvoices.Add(ToRow(invoice));

    public async Task<InboundEInvoice?> FindAsync(Guid id, CancellationToken cancellationToken) =>
        await db
            .InboundEInvoices.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken)
            .ConfigureAwait(false)
            is { } row
            ? FromRow(row)
            : null;

    public async Task<IReadOnlyList<InboundEInvoice>> ListAsync(
        CancellationToken cancellationToken
    ) =>
        [
            .. (
                await db
                    .InboundEInvoices.AsNoTracking()
                    .OrderBy(item => item.StatusDueOn)
                    .ThenByDescending(item => item.ReceivedAt)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false)
            ).Select(FromRow),
        ];

    public void Update(InboundEInvoice invoice) => db.InboundEInvoices.Update(ToRow(invoice));

    private static InboundEInvoiceRow ToRow(InboundEInvoice invoice) =>
        new()
        {
            Id = invoice.Id,
            InvoiceNumber = invoice.InvoiceNumber,
            SupplierName = invoice.SupplierName,
            SupplierTaxId = invoice.SupplierTaxId,
            IssueDate = invoice.IssueDate,
            PayableAmount = invoice.PayableAmount,
            Currency = invoice.Currency,
            ReceivedAt = invoice.ReceivedAt,
            StatusDueOn = invoice.StatusDueOn,
            Status = invoice.Status,
            Reason = invoice.Reason,
            PaidOn = invoice.PaidOn,
            ContentHash = invoice.ContentHash,
            Xml = invoice.Xml,
        };

    private static InboundEInvoice FromRow(InboundEInvoiceRow row) =>
        new()
        {
            Id = row.Id,
            InvoiceNumber = row.InvoiceNumber,
            SupplierName = row.SupplierName,
            SupplierTaxId = row.SupplierTaxId,
            IssueDate = row.IssueDate,
            PayableAmount = row.PayableAmount,
            Currency = row.Currency,
            ReceivedAt = row.ReceivedAt,
            StatusDueOn = row.StatusDueOn,
            Status = row.Status,
            Reason = row.Reason,
            PaidOn = row.PaidOn,
            ContentHash = row.ContentHash,
            Xml = row.Xml,
        };
}
