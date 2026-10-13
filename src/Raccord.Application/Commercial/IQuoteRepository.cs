using Raccord.Domain.Commercial;

namespace Raccord.Application.Commercial;

/// <summary>Persistence ports for quote snapshots and their number sequences.</summary>
public interface IQuoteRepository
{
    /// <summary>Finds a quote by identifier.</summary>
    Task<Quote?> FindAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>Finds the numbering sequence for a year.</summary>
    Task<QuoteNumberSequence?> FindSequenceAsync(
        string seriesCode,
        int year,
        CancellationToken cancellationToken
    );

    /// <summary>Adds a quote.</summary>
    void Add(Quote quote);

    /// <summary>Adds a numbering sequence.</summary>
    void AddSequence(QuoteNumberSequence sequence);
}
