using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Pricing;

namespace Raccord.Domain.Billing;

/// <summary>A reusable VAT scheme whose identity and rates are retained by commercial lines.</summary>
public sealed class VatScheme
{
    private readonly List<VatClassRate> classRates = [];

    private VatScheme() => Name = string.Empty;

    private VatScheme(string name, VatSchemeKind kind)
    {
        Id = Guid.CreateVersion7();
        Name = NormalizeName(name);
        Kind = kind;
    }

    /// <summary>Stable scheme identity.</summary>
    public Guid Id { get; private set; }

    /// <summary>Administrator-maintained display name.</summary>
    public string Name { get; private set; }

    /// <summary>Rate resolution behavior.</summary>
    public VatSchemeKind Kind { get; private set; }

    /// <summary>One rate for every class when the scheme is fixed-rate.</summary>
    public decimal? FixedPercentage { get; private set; }

    /// <summary>Whether a saved commercial record has captured this scheme.</summary>
    public bool IsUsed { get; private set; }

    /// <summary>Per-class rate mappings.</summary>
    public IReadOnlyCollection<VatClassRate> ClassRates => classRates.AsReadOnly();

    /// <summary>Creates a scheme that explicitly maps one or more VAT classes to rates.</summary>
    public static VatScheme CreatePerClass(string name, IEnumerable<VatClassRate> rates)
    {
        ArgumentNullException.ThrowIfNull(rates);
        var scheme = new VatScheme(name, VatSchemeKind.PerClass);
        scheme.ReplaceClassRates(rates);
        if (scheme.classRates.Count == 0)
        {
            throw new BusinessRuleViolationException(
                "invoicing.vat-class-rates-required",
                "A per-class VAT scheme must map at least one VAT class."
            );
        }

        return scheme;
    }

    /// <summary>Creates a scheme with one rate shared by all selected classes.</summary>
    public static VatScheme CreateFixedRate(string name, decimal percentage)
    {
        var scheme = new VatScheme(name, VatSchemeKind.FixedRate)
        {
            FixedPercentage = VatRate.Of(percentage).Percentage,
        };
        return scheme;
    }

    /// <summary>Creates a reverse-charge scheme with a zero calculated percentage.</summary>
    public static VatScheme CreateReverseCharge(string name) =>
        new(name, VatSchemeKind.ReverseCharge);

    /// <summary>Resolves the configured rate for a class identity.</summary>
    public VatRate RateFor(Guid vatClassId)
    {
        if (vatClassId == Guid.Empty)
        {
            throw new BusinessRuleViolationException(
                "invoicing.vat-class-identity-invalid",
                "A VAT class identity is required to resolve a scheme."
            );
        }

        return Kind switch
        {
            VatSchemeKind.PerClass => classRates.FirstOrDefault(rate =>
                rate.VatClassId == vatClassId
            )
                is { } rate
                ? VatRate.Of(rate.Percentage)
                : throw new BusinessRuleViolationException(
                    "invoicing.vat-class-unmapped",
                    "The selected VAT scheme has no rate for this VAT class."
                ),
            VatSchemeKind.FixedRate => VatRate.Of(FixedPercentage!.Value),
            VatSchemeKind.ReverseCharge => VatRate.Of(0m),
            _ => throw new BusinessRuleViolationException(
                "invoicing.vat-scheme-kind-invalid",
                "The VAT scheme kind is unsupported."
            ),
        };
    }

    /// <summary>Changes per-class mappings while the scheme has not been used.</summary>
    public void UpdatePerClassRates(IEnumerable<VatClassRate> rates)
    {
        ArgumentNullException.ThrowIfNull(rates);
        EnsureUnused();
        if (Kind != VatSchemeKind.PerClass)
        {
            throw new BusinessRuleViolationException(
                "invoicing.vat-scheme-kind-mismatch",
                "Only a per-class scheme accepts class-rate mappings."
            );
        }

        ReplaceClassRates(rates);
        if (classRates.Count == 0)
        {
            throw new BusinessRuleViolationException(
                "invoicing.vat-class-rates-required",
                "A per-class VAT scheme must map at least one VAT class."
            );
        }
    }

    /// <summary>Changes a fixed rate while the scheme has not been used.</summary>
    public void UpdateFixedRate(decimal percentage)
    {
        EnsureUnused();
        if (Kind != VatSchemeKind.FixedRate)
        {
            throw new BusinessRuleViolationException(
                "invoicing.vat-scheme-kind-mismatch",
                "Only a fixed-rate scheme accepts one shared percentage."
            );
        }

        FixedPercentage = VatRate.Of(percentage).Percentage;
    }

    /// <summary>Changes the display name without changing the scheme identity.</summary>
    public void Rename(string name) => Name = NormalizeName(name);

    /// <summary>Marks the scheme as used by a saved commercial record.</summary>
    public void MarkUsed() => IsUsed = true;

    private static string NormalizeName(string name)
    {
        var normalized = name?.Trim();
        if (string.IsNullOrWhiteSpace(normalized) || normalized.Length > 100)
        {
            throw new BusinessRuleViolationException(
                "invoicing.vat-scheme-name-invalid",
                "VAT scheme name must contain between 1 and 100 characters."
            );
        }

        return normalized;
    }

    private void ReplaceClassRates(IEnumerable<VatClassRate> rates)
    {
        var mapped = rates.ToArray();
        if (mapped.Any(rate => rate is null))
        {
            throw new BusinessRuleViolationException(
                "invoicing.vat-class-rate-invalid",
                "A VAT scheme cannot contain an empty class-rate mapping."
            );
        }

        if (mapped.Select(rate => rate.VatClassId).Distinct().Count() != mapped.Length)
        {
            throw new BusinessRuleViolationException(
                "invoicing.vat-class-rate-duplicate",
                "A VAT scheme can map each VAT class only once."
            );
        }

        classRates.Clear();
        classRates.AddRange(mapped);
    }

    private void EnsureUnused()
    {
        if (IsUsed)
        {
            throw new BusinessRuleViolationException(
                "invoicing.vat-scheme-used",
                "A VAT scheme rate cannot change after a saved commercial record uses it. Create a replacement scheme instead."
            );
        }
    }
}
