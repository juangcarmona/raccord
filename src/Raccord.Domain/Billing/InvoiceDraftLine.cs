using Raccord.Domain.Projects;
using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Pricing;

namespace Raccord.Domain.Billing;

/// <summary>A line of an invoice draft, copied from a billable line.</summary>
public sealed class InvoiceDraftLine
{
    private InvoiceDraftLine()
    {
        Description = string.Empty;
        CurrencyCode = Currency.Eur.Code;
    }

    private LinePricing? correctionPricing;

    internal InvoiceDraftLine(int position, BillableLine source)
    {
        Id = Guid.CreateVersion7();
        Position = position;
        SourceBillableLineId = source.Id;
        Description = source.Description;
        Quantity = source.Quantity;
        UnitPrice = source.UnitPrice;
        DiscountPercent = source.DiscountPercent;
        VatPercentage = source.VatPercentage;
        CurrencyCode = source.CurrencyCode;
    }

    internal InvoiceDraftLine(int position, IssuedInvoiceLine source, bool asDifference)
    {
        Id = Guid.CreateVersion7();
        Position = position;
        Description = source.Description;
        Quantity = asDifference ? 1m : source.Pricing.Quantity;
        UnitPrice = asDifference ? -source.Pricing.NetAmount.Amount : source.Pricing.UnitPrice;
        DiscountPercent = source.Pricing.DiscountPercent;
        VatPercentage = source.Pricing.VatRate.Percentage;
        CurrencyCode = source.Pricing.Currency.Code;
        if (asDifference)
        {
            IsAdjustment = true;
            correctionPricing = LinePricing.CreateAdjustment(
                -source.Pricing.NetAmount.Amount,
                source.Pricing.VatRate,
                source.Pricing.VatRegime,
                source.Pricing.ExemptionCause
            );
        }
    }

    /// <summary>Draft line identifier.</summary>
    public Guid Id { get; private set; }

    /// <summary>Order of the line on the invoice.</summary>
    public int Position { get; private set; }

    /// <summary>The project billable line this line bills.</summary>
    public Guid SourceBillableLineId { get; private set; }

    /// <summary>Text printed on the invoice.</summary>
    public string Description { get; private set; }

    /// <summary>Units, days or hours.</summary>
    public decimal Quantity { get; private set; }

    /// <summary>Net unit price.</summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>Line discount percentage.</summary>
    public decimal DiscountPercent { get; private set; }

    /// <summary>VAT percentage.</summary>
    public decimal VatPercentage { get; private set; }

    /// <summary>ISO currency code.</summary>
    public string CurrencyCode { get; private set; }

    /// <summary>Whether the line is a signed rectification adjustment.</summary>
    public bool IsAdjustment { get; private set; }

    /// <summary>The line's pricing.</summary>
    public LinePricing Pricing =>
        IsAdjustment
            ? LinePricing.CreateAdjustment(Quantity * UnitPrice, VatRate.Of(VatPercentage))
            : correctionPricing
                ?? LinePricing.Create(
                    Quantity,
                    UnitPrice,
                    DiscountPercent,
                    VatRate.Of(VatPercentage)
                );
}
