using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Raccord.Application.Compliance;
using Raccord.Domain.Billing;
using Raccord.Infrastructure.Persistence;
using Raccord.Modules.Compliance;
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
        var previousHash = db
            .ComplianceRecords.OrderByDescending(record => record.CreatedAt)
            .ThenByDescending(record => record.Id)
            .Select(record => record.RecordHash)
            .FirstOrDefault();
        var id = Guid.CreateVersion7();
        var createdAt = clock.UtcNow;
        var recordHash = Convert.ToHexStringLower(
            SHA256.HashData(
                Encoding.UTF8.GetBytes(
                    $"{invoice.Id:N}|{invoice.Number.Value}|invoice-issued|{representation.ContentHash}|{previousHash}|{createdAt:O}"
                )
            )
        );
        db.ComplianceRecords.Add(
            new ComplianceRecordRow
            {
                Id = id,
                InvoiceId = invoice.Id,
                InvoiceNumber = invoice.Number.Value,
                ContentHash = representation.ContentHash,
                Kind = "invoice-issued",
                Status = PendingStatus,
                PreviousHash = previousHash,
                RecordHash = recordHash,
                CreatedAt = createdAt,
            }
        );
    }
}
