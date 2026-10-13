using Microsoft.EntityFrameworkCore;
using Raccord.Application.Commercial;
using Raccord.Domain.Commercial;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class QuoteRepository(RaccordDbContext db) : IQuoteRepository
{
    public Task<Quote?> FindAsync(Guid id, CancellationToken cancellationToken) =>
        db
            .Quotes.Include(quote => quote.Lines)
            .FirstOrDefaultAsync(quote => quote.Id == id, cancellationToken);

    public Task<QuoteNumberSequence?> FindSequenceAsync(
        string seriesCode,
        int year,
        CancellationToken cancellationToken
    ) =>
        db.QuoteNumberSequences.FirstOrDefaultAsync(
            sequence => sequence.SeriesCode == seriesCode && sequence.Year == year,
            cancellationToken
        );

    public void Add(Quote quote) => db.Quotes.Add(quote);

    public void AddSequence(QuoteNumberSequence sequence) => db.QuoteNumberSequences.Add(sequence);
}
