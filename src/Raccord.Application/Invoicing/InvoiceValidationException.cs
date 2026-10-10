using Raccord.Domain.Billing;

namespace Raccord.Application.Invoicing;

/// <summary>Raised when a draft cannot be finalized; lists every issue so the user can fix them at once.</summary>
public sealed class InvoiceValidationException : Exception
{
    /// <summary>Creates the exception.</summary>
    public InvoiceValidationException(IReadOnlyList<ValidationIssue> issues)
        : base("The invoice draft cannot be finalized.")
    {
        Issues = issues;
    }

    /// <summary>The validation issues.</summary>
    public IReadOnlyList<ValidationIssue> Issues { get; }
}
