using Raccord.Contracts.Common;

namespace Raccord.Contracts.Invoicing;

/// <summary>Creates an invoice draft from unbilled project lines.</summary>
/// <param name="ProjectId">The project to bill.</param>
/// <param name="BillableLineIds">Unbilled lines to include.</param>
/// <param name="SeriesCode">Series that will number the invoice; the default series when omitted.</param>
public sealed record CreateInvoiceDraftRequest(
    Guid ProjectId,
    IReadOnlyList<Guid> BillableLineIds,
    string? SeriesCode
);

/// <summary>Finalizes a draft.</summary>
/// <param name="IssueDate">Legal issue date; today in the organization's time zone when omitted.</param>
public sealed record FinalizeInvoiceDraftRequest(DateOnly? IssueDate);

/// <summary>A line of a draft or issued invoice.</summary>
/// <param name="Id">Draft line identifier; null on issued invoices.</param>
/// <param name="Position">One-based order.</param>
/// <param name="Description">Printed text.</param>
/// <param name="Quantity">Units, days or hours.</param>
/// <param name="UnitPrice">Net unit price.</param>
/// <param name="DiscountPercent">Line discount percentage.</param>
/// <param name="VatRate">VAT percentage.</param>
/// <param name="NetAmount">Net amount.</param>
public sealed record InvoiceLineResponse(
    Guid? Id,
    int Position,
    string Description,
    decimal Quantity,
    decimal UnitPrice,
    decimal DiscountPercent,
    decimal VatRate,
    decimal NetAmount
);

/// <summary>Taxable base and VAT for one rate.</summary>
/// <param name="Rate">VAT percentage.</param>
/// <param name="TaxableBase">Sum of net amounts at this rate.</param>
/// <param name="VatAmount">VAT at this rate.</param>
public sealed record VatGroupResponse(decimal Rate, decimal TaxableBase, decimal VatAmount);

/// <summary>Server-calculated totals.</summary>
/// <param name="Currency">ISO currency code.</param>
/// <param name="TaxableBase">Sum of taxable bases.</param>
/// <param name="VatAmount">Sum of VAT.</param>
/// <param name="Total">Amount payable.</param>
/// <param name="VatBreakdown">One entry per VAT rate.</param>
public sealed record InvoiceTotalsResponse(
    string Currency,
    decimal TaxableBase,
    decimal VatAmount,
    decimal Total,
    IReadOnlyList<VatGroupResponse> VatBreakdown
);

/// <summary>An invoice draft.</summary>
/// <param name="Id">Draft identifier.</param>
/// <param name="Status">Either <c>open</c> or <c>issued</c>.</param>
/// <param name="ProjectId">Billed project.</param>
/// <param name="ProjectName">Project name.</param>
/// <param name="CustomerId">Billed customer.</param>
/// <param name="CustomerName">Customer legal name.</param>
/// <param name="SeriesCode">Numbering series.</param>
/// <param name="OperationDate">Date the work was delivered.</param>
/// <param name="CreatedAt">Creation moment (UTC).</param>
/// <param name="Lines">Draft lines.</param>
/// <param name="Totals">Server-calculated totals.</param>
/// <param name="ValidationIssues">What prevents issuing it today; empty when it can be finalized.</param>
/// <param name="IssuedInvoiceId">The issued invoice, once finalized.</param>
public sealed record InvoiceDraftResponse(
    Guid Id,
    string Status,
    Guid ProjectId,
    string ProjectName,
    Guid CustomerId,
    string CustomerName,
    string SeriesCode,
    DateOnly OperationDate,
    DateTimeOffset CreatedAt,
    IReadOnlyList<InvoiceLineResponse> Lines,
    InvoiceTotalsResponse Totals,
    IReadOnlyList<ValidationIssueResponse> ValidationIssues,
    Guid? IssuedInvoiceId
);

/// <summary>A draft in a list.</summary>
/// <param name="Id">Draft identifier.</param>
/// <param name="ProjectId">Billed project.</param>
/// <param name="ProjectName">Project name.</param>
/// <param name="CustomerName">Customer legal name.</param>
/// <param name="LineCount">Number of lines.</param>
/// <param name="Total">Current total.</param>
/// <param name="Currency">ISO currency code.</param>
/// <param name="CreatedAt">Creation moment (UTC).</param>
public sealed record InvoiceDraftSummaryResponse(
    Guid Id,
    Guid ProjectId,
    string ProjectName,
    string CustomerName,
    int LineCount,
    decimal Total,
    string Currency,
    DateTimeOffset CreatedAt
);

/// <summary>One project currently eligible for the staged-invoicing queue.</summary>
public sealed record InvoiceQueueEntryResponse(
    Guid ProjectId,
    string ProjectName,
    Guid CustomerId,
    string CustomerName,
    DateOnly DueDate,
    bool Overdue,
    int UnbilledLineCount,
    decimal UnbilledNetAmount,
    string Currency
);

/// <summary>An issued, immutable invoice.</summary>
/// <param name="Id">Invoice identifier.</param>
/// <param name="Number">Printed number.</param>
/// <param name="SeriesCode">Numbering series.</param>
/// <param name="IssueDate">Legal issue date.</param>
/// <param name="OperationDate">Operation date when different from the issue date.</param>
/// <param name="IssuedAt">Finalization moment (UTC).</param>
/// <param name="Issuer">Issuer snapshot.</param>
/// <param name="Recipient">Recipient snapshot.</param>
/// <param name="Lines">Invoice lines.</param>
/// <param name="Totals">Totals.</param>
/// <param name="ContentHash">SHA-256 of the canonical issued representation.</param>
/// <param name="ProjectId">Billed project.</param>
/// <param name="CustomerId">Billed customer.</param>
/// <param name="DraftId">Finalized draft.</param>
public sealed record IssuedInvoiceResponse(
    Guid Id,
    string Number,
    string SeriesCode,
    DateOnly IssueDate,
    DateOnly? OperationDate,
    DateTimeOffset IssuedAt,
    PartyResponse Issuer,
    PartyResponse Recipient,
    IReadOnlyList<InvoiceLineResponse> Lines,
    InvoiceTotalsResponse Totals,
    string ContentHash,
    Guid ProjectId,
    Guid CustomerId,
    Guid DraftId
);

/// <summary>An issued invoice in a list.</summary>
/// <param name="Id">Invoice identifier.</param>
/// <param name="Number">Printed number.</param>
/// <param name="IssueDate">Legal issue date.</param>
/// <param name="RecipientName">Recipient legal name.</param>
/// <param name="Total">Amount payable.</param>
/// <param name="Currency">ISO currency code.</param>
/// <param name="ProjectId">Billed project.</param>
public sealed record IssuedInvoiceSummaryResponse(
    Guid Id,
    string Number,
    DateOnly IssueDate,
    string RecipientName,
    decimal Total,
    string Currency,
    Guid ProjectId
);

/// <summary>An invoice numbering series.</summary>
/// <param name="Code">Series code.</param>
/// <param name="Description">What the series is used for.</param>
/// <param name="IsDefault">Whether new drafts use it by default.</param>
public sealed record InvoiceSeriesResponse(string Code, string Description, bool IsDefault);
