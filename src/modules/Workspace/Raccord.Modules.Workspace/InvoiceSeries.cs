using Raccord.SharedKernel.Common;

namespace Raccord.Modules.Workspace.Domain;

/// <summary>A named invoice numbering series.</summary>
internal sealed class InvoiceSeries
{
    private InvoiceSeries()
    {
        Code = string.Empty;
        Description = string.Empty;
    }

    internal string Code { get; private set; }

    internal string Description { get; private set; }

    internal bool IsDefault { get; private set; }

    internal static InvoiceSeries Define(string code, string description, bool isDefault)
    {
        if (
            string.IsNullOrEmpty(code)
            || code.Length > 10
            || !code.All(c => char.IsAsciiDigit(c) || char.IsAsciiLetterUpper(c))
        )
        {
            throw new BusinessRuleViolationException(
                "numbering.series-code-invalid",
                "Series codes have 1 to 10 uppercase letters or digits."
            );
        }

        return new InvoiceSeries
        {
            Code = code,
            Description = description.Trim(),
            IsDefault = isDefault,
        };
    }
}
