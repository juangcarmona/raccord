namespace Raccord.Contracts.Commercial;

/// <summary>A quote generated from a project's current plan.</summary>
public sealed record QuoteResponse(
    Guid Id,
    Guid ProjectId,
    Guid CustomerId,
    string CustomerName,
    string ProjectName,
    string Number,
    DateOnly IssuedOn,
    DateOnly ValidUntil,
    string? PaymentTerms,
    IReadOnlyList<QuoteLineResponse> Lines
);

/// <summary>A copied priced line in a quote snapshot.</summary>
public sealed record QuoteLineResponse(
    int Position,
    string Description,
    decimal Quantity,
    decimal UnitPrice,
    decimal DiscountPercent,
    decimal VatPercentage,
    string Currency
);
