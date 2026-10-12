using Raccord.SharedKernel.Pricing;

namespace Raccord.Domain.Billing;

/// <summary>A line of an issued invoice.</summary>
/// <param name="Position">One-based order on the invoice.</param>
/// <param name="Description">Text printed on the invoice.</param>
/// <param name="Pricing">Quantity, unit price, discount, VAT rate and net amount.</param>
public sealed record IssuedInvoiceLine(int Position, string Description, LinePricing Pricing);
