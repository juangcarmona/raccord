namespace Raccord.Domain.Billing;

/// <summary>A named invoice numbering series, for example ordinary invoices or rectifying invoices.</summary>
public sealed class InvoiceSeries
{
    private InvoiceSeries()
    {
        Code = string.Empty;
        Description = string.Empty;
    }

    /// <summary>Series code used as the number prefix.</summary>
    public string Code { get; private set; }

    /// <summary>What the series is used for.</summary>
    public string Description { get; private set; }

    /// <summary>Whether new drafts use this series unless told otherwise.</summary>
    public bool IsDefault { get; private set; }

    /// <summary>Defines a series.</summary>
    public static InvoiceSeries Define(string code, string description, bool isDefault)
    {
        InvoiceNumberSequence.EnsureValidSeriesCode(code);
        return new InvoiceSeries
        {
            Code = code,
            Description = description.Trim(),
            IsDefault = isDefault,
        };
    }
}
