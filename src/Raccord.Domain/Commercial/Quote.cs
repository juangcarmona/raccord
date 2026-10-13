using Raccord.SharedKernel.Common;

namespace Raccord.Domain.Commercial;

/// <summary>Immutable commercial snapshot generated from a project plan.</summary>
public sealed class Quote
{
    private readonly List<QuoteLine> lines = [];

    private Quote() { }

    private Quote(
        Guid projectId,
        Guid customerId,
        string customerName,
        string projectName,
        QuoteNumber number,
        DateOnly issuedOn,
        DateOnly validUntil,
        string? paymentTerms,
        IEnumerable<QuoteLine> snapshotLines
    )
    {
        if (validUntil < issuedOn)
            throw new BusinessRuleViolationException("quotes.validity-invalid", "Quote validity cannot end before issue date.");
        Id = Guid.CreateVersion7();
        ProjectId = projectId;
        CustomerId = customerId;
        CustomerName = customerName;
        ProjectName = projectName;
        SeriesCode = number.SeriesCode;
        Year = number.Year;
        Sequence = number.Sequence;
        Number = number.Value;
        IssuedOn = issuedOn;
        ValidUntil = validUntil;
        PaymentTerms = paymentTerms;
        lines.AddRange(snapshotLines);
    }

    /// <summary>Quote identifier.</summary>
    public Guid Id { get; private set; }
    /// <summary>Source project identifier.</summary>
    public Guid ProjectId { get; private set; }
    /// <summary>Customer identifier at generation time.</summary>
    public Guid CustomerId { get; private set; }
    /// <summary>Customer name snapshot.</summary>
    public string CustomerName { get; private set; } = string.Empty;
    /// <summary>Project name snapshot.</summary>
    public string ProjectName { get; private set; } = string.Empty;
    /// <summary>Number series.</summary>
    public string SeriesCode { get; private set; } = string.Empty;
    /// <summary>Number year.</summary>
    public int Year { get; private set; }
    /// <summary>Number sequence.</summary>
    public int Sequence { get; private set; }
    /// <summary>Printed quote number.</summary>
    public string Number { get; private set; } = string.Empty;
    /// <summary>Generation date.</summary>
    public DateOnly IssuedOn { get; private set; }
    /// <summary>Last date on which the quote is valid.</summary>
    public DateOnly ValidUntil { get; private set; }
    /// <summary>Payment terms snapshot.</summary>
    public string? PaymentTerms { get; private set; }
    /// <summary>Immutable planned lines.</summary>
    public IReadOnlyList<QuoteLine> Lines => [.. lines.OrderBy(line => line.Position)];

    /// <summary>Creates an immutable snapshot of the current project plan.</summary>
    public static Quote Generate(
        Guid projectId,
        Guid customerId,
        string customerName,
        string projectName,
        QuoteNumber number,
        DateOnly issuedOn,
        int validityDays,
        string? paymentTerms,
        IEnumerable<QuoteLine> snapshotLines
    )
    {
        if (validityDays <= 0)
            throw new BusinessRuleViolationException("quotes.validity-positive", "Quote validity must be greater than zero days.");
        var materialized = snapshotLines.ToList();
        if (materialized.Count == 0)
            throw new BusinessRuleViolationException("quotes.plan-empty", "A quote requires at least one planned priced line.");
        return new(projectId, customerId, customerName, projectName, number, issuedOn, issuedOn.AddDays(validityDays), paymentTerms, materialized);
    }
}
