using Raccord.Domain.Billing;

namespace Raccord.Application.Invoicing;

/// <summary>Persistence of invoice series and their yearly number sequences.</summary>
public interface IInvoiceSeriesRepository
{
    /// <summary>Lists series by code.</summary>
    Task<IReadOnlyList<InvoiceSeries>> ListAsync(CancellationToken cancellationToken);

    /// <summary>Finds a series.</summary>
    Task<InvoiceSeries?> FindAsync(string code, CancellationToken cancellationToken);

    /// <summary>Adds a series.</summary>
    void Add(InvoiceSeries series);

    /// <summary>Finds the sequence of a series and year without starting it.</summary>
    Task<InvoiceNumberSequence?> FindSequenceAsync(string seriesCode, int year, CancellationToken cancellationToken);

    /// <summary>Loads the sequence of a series and year for allocation, starting it when it does not exist yet.</summary>
    Task<InvoiceNumberSequence> GetOrStartSequenceAsync(string seriesCode, int year, CancellationToken cancellationToken);
}
