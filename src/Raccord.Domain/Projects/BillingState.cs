namespace Raccord.Domain.Projects;

/// <summary>Where a billable line is in the billing process.</summary>
public enum BillingState
{
    /// <summary>Available to be added to an invoice draft.</summary>
    Unbilled = 1,

    /// <summary>Reserved by one open invoice draft.</summary>
    InDraft = 2,

    /// <summary>Included in an issued invoice; never billable again.</summary>
    Invoiced = 3,
}
