namespace Raccord.Contracts.Finance;

/// <summary>Current financial totals for a date range.</summary>
public sealed record FinanceSummaryResponse(
    DateOnly From,
    DateOnly To,
    int InvoiceCount,
    decimal InvoicedTotal,
    decimal VatTotal,
    decimal PaidTotal,
    decimal OutstandingTotal,
    int OverdueInvoiceCount
);
