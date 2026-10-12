namespace Raccord.SharedKernel.Common;

/// <summary>An ISO 4217 currency with its number of minor units.</summary>
public sealed record Currency
{
    private Currency(string code, int minorUnits)
    {
        Code = code;
        MinorUnits = minorUnits;
    }

    /// <summary>Euro, the only invoicing currency supported by the first release.</summary>
    public static Currency Eur { get; } = new("EUR", 2);

    /// <summary>The three-letter ISO 4217 code.</summary>
    public string Code { get; }

    /// <summary>The number of decimals used by amounts in this currency.</summary>
    public int MinorUnits { get; }

    /// <summary>Resolves a supported currency from its ISO code.</summary>
    public static Currency FromCode(string code)
    {
        ArgumentNullException.ThrowIfNull(code);
        return string.Equals(code, Eur.Code, StringComparison.Ordinal)
            ? Eur
            : throw new BusinessRuleViolationException(
                "currency.unsupported",
                $"Currency '{code}' is not supported."
            );
    }

    /// <inheritdoc />
    public override string ToString() => Code;
}
