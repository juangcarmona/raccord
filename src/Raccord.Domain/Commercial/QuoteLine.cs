namespace Raccord.Domain.Commercial;

/// <summary>Copied pricing data in an immutable quote snapshot.</summary>
public sealed class QuoteLine
{
    private QuoteLine() { Description = string.Empty; CurrencyCode = string.Empty; }

    private QuoteLine(int position, string description, decimal quantity, decimal unitPrice, decimal discountPercent, decimal vatPercentage, string currencyCode)
    {
        Id = Guid.CreateVersion7();
        Position = position;
        Description = description;
        Quantity = quantity;
        UnitPrice = unitPrice;
        DiscountPercent = discountPercent;
        VatPercentage = vatPercentage;
        CurrencyCode = currencyCode;
    }

    /// <summary>Creates a quote line from project pricing.</summary>
    public static QuoteLine FromProjectLine(int position, string description, decimal quantity, decimal unitPrice, decimal discountPercent, decimal vatPercentage, string currencyCode) =>
        new(position, description, quantity, unitPrice, discountPercent, vatPercentage, currencyCode);

    /// <summary>Line identifier.</summary>
    public Guid Id { get; private set; }
    /// <summary>Display order.</summary>
    public int Position { get; private set; }
    /// <summary>Line description.</summary>
    public string Description { get; private set; }
    /// <summary>Quantity.</summary>
    public decimal Quantity { get; private set; }
    /// <summary>Unit price.</summary>
    public decimal UnitPrice { get; private set; }
    /// <summary>Discount percentage.</summary>
    public decimal DiscountPercent { get; private set; }
    /// <summary>VAT percentage.</summary>
    public decimal VatPercentage { get; private set; }
    /// <summary>Currency code.</summary>
    public string CurrencyCode { get; private set; }
}
