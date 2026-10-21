using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Pricing;

namespace Raccord.Domain.Projects;

/// <summary>A priced piece of project work that can be invoiced once.</summary>
public sealed class BillableLine
{
    private BillableLine()
    {
        Description = string.Empty;
        CurrencyCode = Currency.Eur.Code;
    }

    internal BillableLine(
        int position,
        BillableLineKind kind,
        string description,
        LinePricing pricing,
        Guid? vatClassId = null,
        Guid? vatSchemeId = null
    )
    {
        if ((vatClassId is null) != (vatSchemeId is null))
        {
            throw new BusinessRuleViolationException(
                "invoicing.vat-reference-incomplete",
                "A billable line must store both its VAT class and scheme identities."
            );
        }

        Id = Guid.CreateVersion7();
        Position = position;
        Kind = kind;
        Description = description;
        Quantity = pricing.Quantity;
        UnitPrice = pricing.UnitPrice;
        DiscountPercent = pricing.DiscountPercent;
        VatPercentage = pricing.VatRate.Percentage;
        VatClassId = vatClassId;
        VatSchemeId = vatSchemeId;
        CurrencyCode = pricing.Currency.Code;
        State = BillingState.Unbilled;
    }

    /// <summary>Line identifier.</summary>
    public Guid Id { get; private set; }

    /// <summary>One-based order in which the line was recorded on the project.</summary>
    public int Position { get; private set; }

    /// <summary>What the line charges for.</summary>
    public BillableLineKind Kind { get; private set; }

    /// <summary>Text shown on the invoice.</summary>
    public string Description { get; private set; }

    /// <summary>Units, days or hours.</summary>
    public decimal Quantity { get; private set; }

    /// <summary>Net unit price.</summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>Line discount percentage.</summary>
    public decimal DiscountPercent { get; private set; }

    /// <summary>VAT percentage.</summary>
    public decimal VatPercentage { get; private set; }

    /// <summary>Stable VAT class identity selected when the line was saved.</summary>
    public Guid? VatClassId { get; private set; }

    /// <summary>Stable VAT scheme identity selected when the line was saved.</summary>
    public Guid? VatSchemeId { get; private set; }

    /// <summary>ISO currency code.</summary>
    public string CurrencyCode { get; private set; }

    /// <summary>Billing progress of the line.</summary>
    public BillingState State { get; private set; }

    /// <summary>The open draft holding the line, if any.</summary>
    public Guid? DraftId { get; private set; }

    /// <summary>The issued invoice that billed the line, if any.</summary>
    public Guid? InvoiceId { get; private set; }

    /// <summary>The pricing of the line as a value object.</summary>
    public LinePricing Pricing =>
        LinePricing.Create(Quantity, UnitPrice, DiscountPercent, VatRate.Of(VatPercentage));

    internal void Reserve(Guid draftId)
    {
        if (State != BillingState.Unbilled)
        {
            throw new BusinessRuleViolationException(
                "billing.line-not-unbilled",
                $"Billable line '{Description}' is already in a draft or invoiced."
            );
        }

        State = BillingState.InDraft;
        DraftId = draftId;
    }

    internal void Release(Guid draftId)
    {
        if (State == BillingState.InDraft && DraftId == draftId)
        {
            State = BillingState.Unbilled;
            DraftId = null;
        }
    }

    internal void MarkInvoiced(Guid draftId, Guid invoiceId)
    {
        if (State == BillingState.InDraft && DraftId == draftId)
        {
            State = BillingState.Invoiced;
            InvoiceId = invoiceId;
        }
    }
}
