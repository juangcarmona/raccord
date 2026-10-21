using Raccord.SharedKernel.Common;

namespace Raccord.Domain.Billing;

/// <summary>A reusable classification for the VAT treatment of a commercial line.</summary>
public sealed class VatClass
{
    private VatClass() => Name = string.Empty;

    private VatClass(string name)
    {
        Name = string.Empty;
        Id = Guid.CreateVersion7();
        Rename(name);
    }

    /// <summary>Stable class identity.</summary>
    public Guid Id { get; private set; }

    /// <summary>Administrator-maintained display name.</summary>
    public string Name { get; private set; }

    /// <summary>Creates a VAT class.</summary>
    public static VatClass Create(string name) => new(name);

    /// <summary>Changes the display name without changing class identity.</summary>
    public void Rename(string name)
    {
        var normalized = name?.Trim();
        if (string.IsNullOrWhiteSpace(normalized) || normalized.Length > 100)
        {
            throw new BusinessRuleViolationException(
                "invoicing.vat-class-name-invalid",
                "VAT class name must contain between 1 and 100 characters."
            );
        }

        Name = normalized;
    }
}
