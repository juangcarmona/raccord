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
            throw new BusinessRuleViolationException(
                "quotes.validity-invalid",
                "Quote validity cannot end before issue date."
            );
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
        ReviewStatus = QuoteReviewStatus.Draft;
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

    /// <summary>Current review state.</summary>
    public QuoteReviewStatus ReviewStatus { get; private set; }

    /// <summary>Trusted timestamp at which customer acceptance was recorded.</summary>
    public DateTimeOffset? AcceptedAt { get; private set; }

    /// <summary>Display name of the signer.</summary>
    public string? SignedBy { get; private set; }

    /// <summary>Trusted timestamp at which signature metadata was recorded.</summary>
    public DateTimeOffset? SignedAt { get; private set; }

    /// <summary>Immutable planned lines.</summary>
    public IReadOnlyList<QuoteLine> Lines => [.. lines.OrderBy(line => line.Position)];

    /// <summary>Marks the immutable quote as ready for customer signature.</summary>
    public void MarkReadyForSignature()
    {
        if (ReviewStatus != QuoteReviewStatus.Draft)
            throw new BusinessRuleViolationException(
                "quotes.review-transition-invalid",
                "Only draft quotes can be marked ready for signature."
            );
        ReviewStatus = QuoteReviewStatus.ReadyForSignature;
    }

    /// <summary>Records customer acceptance of a quote ready for signature.</summary>
    public void Accept(DateTimeOffset acceptedAt)
    {
        if (ReviewStatus != QuoteReviewStatus.ReadyForSignature)
            throw new BusinessRuleViolationException(
                "quotes.acceptance-invalid",
                "Only quotes ready for signature can be accepted."
            );
        ReviewStatus = QuoteReviewStatus.Accepted;
        AcceptedAt = acceptedAt;
    }

    /// <summary>Records signature metadata for an accepted quote.</summary>
    public void Sign(string signerName, DateTimeOffset signedAt)
    {
        if (ReviewStatus != QuoteReviewStatus.Accepted)
            throw new BusinessRuleViolationException(
                "quotes.signature-invalid",
                "Only accepted quotes can be signed."
            );
        if (string.IsNullOrWhiteSpace(signerName))
            throw new BusinessRuleViolationException(
                "quotes.signer-required",
                "Signer name is required."
            );
        ReviewStatus = QuoteReviewStatus.SignatureRecorded;
        SignedBy = signerName.Trim();
        SignedAt = signedAt;
    }

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
            throw new BusinessRuleViolationException(
                "quotes.validity-positive",
                "Quote validity must be greater than zero days."
            );
        var materialized = snapshotLines.ToList();
        if (materialized.Count == 0)
            throw new BusinessRuleViolationException(
                "quotes.plan-empty",
                "A quote requires at least one planned priced line."
            );
        return new(
            projectId,
            customerId,
            customerName,
            projectName,
            number,
            issuedOn,
            issuedOn.AddDays(validityDays),
            paymentTerms,
            materialized
        );
    }
}
