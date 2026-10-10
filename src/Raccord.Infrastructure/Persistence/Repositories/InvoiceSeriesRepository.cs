using Microsoft.EntityFrameworkCore;
using Raccord.Application.Invoicing;
using Raccord.Domain.Billing;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class InvoiceSeriesRepository(RaccordDbContext db) : IInvoiceSeriesRepository
{
    public async Task<IReadOnlyList<InvoiceSeries>> ListAsync(CancellationToken cancellationToken) =>
        await db.InvoiceSeries.OrderBy(series => series.Code).ToListAsync(cancellationToken).ConfigureAwait(false);

    public Task<InvoiceSeries?> FindAsync(string code, CancellationToken cancellationToken) =>
        db.InvoiceSeries.FirstOrDefaultAsync(series => series.Code == code, cancellationToken);

    public void Add(InvoiceSeries series) => db.InvoiceSeries.Add(series);

    public Task<InvoiceNumberSequence?> FindSequenceAsync(string seriesCode, int year, CancellationToken cancellationToken) =>
        db.InvoiceNumberSequences.AsNoTracking().FirstOrDefaultAsync(sequence => sequence.SeriesCode == seriesCode && sequence.Year == year, cancellationToken);

    public async Task<InvoiceNumberSequence> GetOrStartSequenceAsync(string seriesCode, int year, CancellationToken cancellationToken)
    {
        var existing = await db.InvoiceNumberSequences
            .FirstOrDefaultAsync(sequence => sequence.SeriesCode == seriesCode && sequence.Year == year, cancellationToken)
            .ConfigureAwait(false);
        if (existing is not null)
        {
            return existing;
        }

        // Two first invoices of a year racing here both insert; the primary key makes one of them fail and retry.
        var started = InvoiceNumberSequence.Start(seriesCode, year);
        db.InvoiceNumberSequences.Add(started);
        return started;
    }
}
