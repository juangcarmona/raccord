using Microsoft.EntityFrameworkCore;
using Raccord.Application.Invoicing;
using Raccord.Domain.Billing;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class IssuedInvoiceRepository(RaccordDbContext db) : IIssuedInvoiceRepository
{
    public void Add(IssuedInvoice invoice, IssuedInvoiceRepresentation representation) =>
        db.IssuedInvoices.Add(
            new IssuedInvoiceRow
            {
                Id = invoice.Id,
                SeriesCode = invoice.Number.SeriesCode,
                Year = invoice.Number.Year,
                Sequence = invoice.Number.Sequence,
                Number = invoice.Number.Value,
                IssueDate = invoice.IssueDate,
                IssuedAt = invoice.IssuedAt,
                DraftId = invoice.Source.DraftId,
                ProjectId = invoice.Source.ProjectId,
                CustomerId = invoice.Source.CustomerId,
                RecipientName = invoice.Recipient.LegalName,
                RecipientTaxId = invoice.Recipient.TaxId,
                Total = invoice.Totals.Total.Amount,
                Currency = invoice.Currency.Code,
                Representation = representation.Json,
                ContentHash = representation.ContentHash,
            }
        );

    public async Task<IssuedInvoiceRecord?> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        var row = await db
            .IssuedInvoices.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken)
            .ConfigureAwait(false);
        return row is null
            ? null
            : new IssuedInvoiceRecord(
                IssuedInvoiceRepresentation.Restore(row.Representation, row.ContentHash),
                row.ContentHash
            );
    }

    public async Task<IReadOnlyList<IssuedInvoiceListItem>> ListAsync(
        CancellationToken cancellationToken
    ) =>
        await db
            .IssuedInvoices.AsNoTracking()
            .OrderByDescending(row => row.IssueDate)
            .ThenByDescending(row => row.Number)
            .Select(row => new IssuedInvoiceListItem(
                row.Id,
                row.Number,
                row.IssueDate,
                row.RecipientName,
                row.Total,
                row.Currency,
                row.ProjectId
            ))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
}
