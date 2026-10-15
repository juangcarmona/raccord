using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Raccord.Application.Abstractions;
using Raccord.Application.Auditing;
using Raccord.Contracts.Invoicing;
using Raccord.SharedKernel.Abstractions;
using Raccord.SharedKernel.Auditing;
using Raccord.SharedKernel.Common;

namespace Raccord.Application.Invoicing;

/// <summary>Receives inbound UBL documents and records their reportable status lifecycle.</summary>
public sealed class InboundEInvoiceWorkflow(
    IInboundEInvoiceRepository invoices,
    IAuditTrail audit,
    IClock clock,
    ICurrentUser actor,
    IUnitOfWork unitOfWork
)
{
    /// <summary>Receives and persists a structured inbound invoice.</summary>
    public async Task<InboundEInvoiceResponse> ReceiveAsync(
        ReceiveInboundEInvoiceRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var document = InboundEInvoiceDocument.Parse(request.Xml);
        var receivedAt = clock.UtcNow;
        var invoice = new InboundEInvoice
        {
            Id = Guid.CreateVersion7(),
            InvoiceNumber = document.InvoiceNumber,
            SupplierName = document.SupplierName,
            SupplierTaxId = document.SupplierTaxId,
            IssueDate = document.IssueDate,
            PayableAmount = document.PayableAmount,
            Currency = document.Currency,
            ReceivedAt = receivedAt,
            StatusDueOn = AddReportingDays(DateOnly.FromDateTime(receivedAt.DateTime)),
            Status = "pending",
            ContentHash = Convert.ToHexStringLower(
                SHA256.HashData(Encoding.UTF8.GetBytes(request.Xml))
            ),
            Xml = request.Xml,
        };
        invoices.Add(invoice);
        Record("inbound-e-invoice.received", invoice, ("status", invoice.Status));
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(invoice);
    }

    /// <summary>Lists received inbound invoices and their current statuses.</summary>
    public async Task<IReadOnlyList<InboundEInvoiceResponse>> ListAsync(
        CancellationToken cancellationToken
    ) =>
        [.. (await invoices.ListAsync(cancellationToken).ConfigureAwait(false)).Select(ToResponse)];

    /// <summary>Reports acceptance, rejection or full payment for an inbound invoice.</summary>
    public async Task<InboundEInvoiceResponse?> UpdateStatusAsync(
        Guid id,
        UpdateInboundEInvoiceStatusRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var invoice = await invoices.FindAsync(id, cancellationToken).ConfigureAwait(false);
        if (invoice is null)
            return null;
        if (request.Status is not ("accepted" or "rejected" or "paid"))
            throw new BusinessRuleViolationException(
                "inbound-e-invoice.status-unsupported",
                "Status must be accepted, rejected or paid."
            );
        if (
            string.Equals(request.Status, "rejected", StringComparison.Ordinal)
            && string.IsNullOrWhiteSpace(request.Reason)
        )
            throw new BusinessRuleViolationException(
                "inbound-e-invoice.rejection-reason-missing",
                "A rejection reason is required."
            );
        if (
            string.Equals(request.Status, "paid", StringComparison.Ordinal)
            && request.PaidOn is null
        )
            throw new BusinessRuleViolationException(
                "inbound-e-invoice.payment-date-missing",
                "The full payment date is required."
            );
        if (string.Equals(invoice.Status, "paid", StringComparison.Ordinal))
            throw new BusinessRuleViolationException(
                "inbound-e-invoice.status-closed",
                "A paid inbound e-invoice cannot change status."
            );

        invoice.Status = request.Status;
        invoice.Reason = request.Reason?.Trim();
        invoice.PaidOn = string.Equals(request.Status, "paid", StringComparison.Ordinal)
            ? request.PaidOn
            : null;
        invoices.Update(invoice);
        Record("inbound-e-invoice.status-reported", invoice, ("status", invoice.Status));
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(invoice);
    }

    private void Record(
        string action,
        InboundEInvoice invoice,
        params (string Key, string Value)[] details
    ) =>
        audit.Append(
            new AuditEntry(
                clock.UtcNow,
                actor.Name,
                action,
                "inbound-e-invoice",
                invoice.Id,
                details
                    .Append(("contentHash", invoice.ContentHash))
                    .ToDictionary(item => item.Item1, item => item.Item2, StringComparer.Ordinal)
            )
        );

    private static InboundEInvoiceResponse ToResponse(InboundEInvoice invoice) =>
        new(
            invoice.Id,
            invoice.InvoiceNumber,
            invoice.SupplierName,
            invoice.SupplierTaxId,
            invoice.IssueDate,
            invoice.PayableAmount,
            invoice.Currency,
            invoice.ReceivedAt,
            invoice.StatusDueOn,
            invoice.Status,
            invoice.Reason,
            invoice.PaidOn,
            invoice.ContentHash
        );

    private static DateOnly AddReportingDays(DateOnly receivedOn)
    {
        var date = receivedOn;
        var remaining = 4;
        while (remaining > 0)
        {
            date = date.AddDays(1);
            if (date.DayOfWeek is not (DayOfWeek.Saturday or DayOfWeek.Sunday))
                remaining--;
        }
        return date;
    }
}
