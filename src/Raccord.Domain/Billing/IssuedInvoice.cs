using Raccord.Domain.Common;
using Raccord.Domain.Parties;
using Raccord.Domain.Pricing;

namespace Raccord.Domain.Billing;

/// <summary>
/// A finalized invoice. Every value is a snapshot taken at issue time; nothing here can change afterwards.
/// Corrections are made with new documents, never by editing this one.
/// </summary>
public sealed class IssuedInvoice
{
    internal IssuedInvoice(
        Guid id,
        IssuedInvoiceSource source,
        InvoiceNumber number,
        DateOnly issueDate,
        DateOnly? operationDate,
        DateTimeOffset issuedAt,
        LegalParty issuer,
        LegalParty recipient,
        IReadOnlyList<IssuedInvoiceLine> lines,
        Currency currency
    )
    {
        Id = id;
        Source = source;
        Number = number;
        IssueDate = issueDate;
        OperationDate = operationDate;
        IssuedAt = issuedAt;
        Issuer = issuer;
        Recipient = recipient;
        Lines = lines;
        Currency = currency;
        Totals = InvoiceTotals.Calculate(lines.Select(line => line.Pricing), currency);
    }

    /// <summary>Invoice identifier.</summary>
    public Guid Id { get; }

    /// <summary>Draft, project and customer the invoice came from.</summary>
    public IssuedInvoiceSource Source { get; }

    /// <summary>The allocated number.</summary>
    public InvoiceNumber Number { get; }

    /// <summary>Legal issue date.</summary>
    public DateOnly IssueDate { get; }

    /// <summary>Date of the operation when it differs from the issue date.</summary>
    public DateOnly? OperationDate { get; }

    /// <summary>Moment the invoice was finalized (UTC).</summary>
    public DateTimeOffset IssuedAt { get; }

    /// <summary>Issuer at issue time.</summary>
    public LegalParty Issuer { get; }

    /// <summary>Recipient at issue time.</summary>
    public LegalParty Recipient { get; }

    /// <summary>Invoice lines in printed order.</summary>
    public IReadOnlyList<IssuedInvoiceLine> Lines { get; }

    /// <summary>Invoice currency.</summary>
    public Currency Currency { get; }

    /// <summary>Taxable base, VAT breakdown and total, recalculated from the lines.</summary>
    public InvoiceTotals Totals { get; }
}
