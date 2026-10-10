namespace Raccord.Application.Invoicing;

/// <summary>Indexed columns of an issued invoice, for lists.</summary>
/// <param name="Id">Invoice identifier.</param>
/// <param name="Number">Printed number.</param>
/// <param name="IssueDate">Issue date.</param>
/// <param name="RecipientName">Recipient legal name.</param>
/// <param name="Total">Amount payable.</param>
/// <param name="Currency">ISO currency code.</param>
/// <param name="ProjectId">Billed project.</param>
public sealed record IssuedInvoiceListItem(
    Guid Id,
    string Number,
    DateOnly IssueDate,
    string RecipientName,
    decimal Total,
    string Currency,
    Guid ProjectId
);
