using Raccord.Application.Compliance;
using Raccord.Domain.Billing;
using Raccord.Infrastructure.Persistence;
using Raccord.SharedKernel.Abstractions;

namespace Raccord.Infrastructure.Compliance;

/// <summary>
/// Records a pending compliance obligation for each issued invoice, in the issuing transaction.
/// Nothing is submitted yet; see docs/architecture/compliance-seam.md.
/// </summary>
internal sealed class PendingComplianceRecorder(RaccordDbContext db, IClock clock)
    : IComplianceRecorder
{
    internal const string PendingStatus = "pending";

    public void RegisterIssuedInvoice(
        IssuedInvoice invoice,
        IssuedInvoiceRepresentation representation
    )
    {
        ArgumentNullException.ThrowIfNull(invoice);
        ArgumentNullException.ThrowIfNull(representation);
        db.ComplianceRecords.Add(
            new ComplianceRecordRow
            {
                Id = Guid.CreateVersion7(),
                InvoiceId = invoice.Id,
                InvoiceNumber = invoice.Number.Value,
                ContentHash = representation.ContentHash,
                Kind = "invoice-issued",
                Status = PendingStatus,
                CreatedAt = clock.UtcNow,
            }
        );
    }
}
