using Raccord.Domain.Billing;

namespace Raccord.Application.Invoicing;

/// <summary>Persistence of yearly invoice number sequences.</summary>
public interface IInvoiceNumberSequenceRepository
{
    /// <summary>Finds a sequence without starting it.</summary>
    Task<InvoiceNumberSequence?> FindSequenceAsync(
        string seriesCode,
        int year,
        CancellationToken cancellationToken
    );

    /// <summary>Loads or starts a sequence for allocation.</summary>
    Task<InvoiceNumberSequence> GetOrStartSequenceAsync(
        string seriesCode,
        int year,
        CancellationToken cancellationToken
    );
}
