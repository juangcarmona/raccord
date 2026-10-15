using Raccord.Domain.Projects;
using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Parties;
using Raccord.SharedKernel.Pricing;

namespace Raccord.Domain.Billing;

/// <summary>An editable, unnumbered invoice prepared from project billable lines.</summary>
public sealed class InvoiceDraft
{
    private readonly List<InvoiceDraftLine> lines = [];

    private InvoiceDraft()
    {
        SeriesCode = string.Empty;
    }

    private InvoiceDraft(
        Guid? projectId,
        Guid customerId,
        DateOnly operationDate,
        string seriesCode,
        DateTimeOffset createdAt
    )
    {
        Id = Guid.CreateVersion7();
        ProjectId = projectId;
        CustomerId = customerId;
        OperationDate = operationDate;
        SeriesCode = seriesCode;
        CreatedAt = createdAt;
        Status = DraftStatus.Open;
    }

    /// <summary>Draft identifier.</summary>
    public Guid Id { get; private set; }

    /// <summary>The billed project.</summary>
    public Guid? ProjectId { get; private set; }

    /// <summary>Creates a draft from manually entered standalone invoice lines.</summary>
    public static InvoiceDraft CreateStandalone(
        Guid customerId,
        DateOnly operationDate,
        IReadOnlyCollection<(
            string Description,
            decimal Quantity,
            decimal UnitPrice,
            decimal DiscountPercent,
            decimal VatRate
        )> lines,
        string seriesCode,
        DateTimeOffset createdAt
    )
    {
        ArgumentNullException.ThrowIfNull(lines);
        InvoiceNumberSequence.EnsureValidSeriesCode(seriesCode);
        if (lines.Count == 0)
            throw new BusinessRuleViolationException(
                "draft.no-lines",
                "Add at least one invoice line."
            );
        var draft = new InvoiceDraft(null, customerId, operationDate, seriesCode, createdAt);
        draft.lines.AddRange(
            lines.Select(
                (line, index) =>
                    new InvoiceDraftLine(
                        index + 1,
                        line.Description,
                        line.Quantity,
                        line.UnitPrice,
                        line.DiscountPercent,
                        line.VatRate
                    )
            )
        );
        return draft;
    }

    /// <summary>Creates a new project draft from source lines without reserving those lines again.</summary>
    public static InvoiceDraft DuplicateFromProject(
        Project project,
        IReadOnlyCollection<Guid> billableLineIds,
        string seriesCode,
        DateTimeOffset createdAt
    )
    {
        ArgumentNullException.ThrowIfNull(project);
        var selected = project
            .BillableLines.Where(line => billableLineIds.Contains(line.Id))
            .ToList();
        if (selected.Count != billableLineIds.Count)
            throw new BusinessRuleViolationException(
                "invoice.duplicate-source-missing",
                "The invoice source line no longer exists in the project."
            );
        var draft = new InvoiceDraft(
            project.Id,
            project.CustomerId,
            project.EndDate,
            seriesCode,
            createdAt
        );
        draft.lines.AddRange(
            selected.Select((line, index) => new InvoiceDraftLine(index + 1, line))
        );
        return draft;
    }

    /// <summary>The billed customer.</summary>
    public Guid CustomerId { get; private set; }

    /// <summary>Date the work was delivered; the project's last day.</summary>
    public DateOnly OperationDate { get; private set; }

    /// <summary>Series that will number the invoice.</summary>
    public string SeriesCode { get; private set; }

    /// <summary>Creation moment (UTC).</summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>Lifecycle state.</summary>
    public DraftStatus Status { get; private set; }

    /// <summary>The issued invoice, once finalized.</summary>
    public Guid? IssuedInvoiceId { get; private set; }

    /// <summary>Lines in invoice order.</summary>
    public IReadOnlyList<InvoiceDraftLine> Lines => [.. lines.OrderBy(line => line.Position)];

    /// <summary>Totals calculated from the current lines.</summary>
    public InvoiceTotals Totals =>
        InvoiceTotals.Calculate(lines.Select(line => line.Pricing), Currency.Eur);

    /// <summary>Creates a draft from unbilled project lines and reserves them.</summary>
    public static InvoiceDraft CreateFromProject(
        Project project,
        IReadOnlyCollection<Guid> billableLineIds,
        string seriesCode,
        DateTimeOffset createdAt
    )
    {
        ArgumentNullException.ThrowIfNull(project);
        ArgumentNullException.ThrowIfNull(billableLineIds);
        if (billableLineIds.Count == 0)
        {
            throw new BusinessRuleViolationException(
                "draft.no-lines",
                "Select at least one billable line."
            );
        }

        InvoiceNumberSequence.EnsureValidSeriesCode(seriesCode);
        var draft = new InvoiceDraft(
            project.Id,
            project.CustomerId,
            project.EndDate,
            seriesCode,
            createdAt
        );
        var reserved = project.ReserveForDraft(billableLineIds.Distinct().ToList(), draft.Id);
        draft.lines.AddRange(
            reserved.Select((line, index) => new InvoiceDraftLine(index + 1, line))
        );
        return draft;
    }

    /// <summary>Removes a line and returns the billable line that must be released on the project.</summary>
    public Guid? RemoveLine(Guid draftLineId)
    {
        EnsureOpen();
        var line =
            lines.Find(candidate => candidate.Id == draftLineId)
            ?? throw new BusinessRuleViolationException(
                "draft.line-not-found",
                $"Line '{draftLineId}' is not in this draft."
            );
        lines.Remove(line);
        return line.SourceBillableLineId;
    }

    /// <summary>Lists every reason the draft cannot be issued with these parties and date.</summary>
    public IReadOnlyList<ValidationIssue> Validate(
        LegalParty? issuer,
        LegalParty? recipient,
        DateOnly issueDate,
        InvoiceNumberSequence sequence
    )
    {
        ArgumentNullException.ThrowIfNull(sequence);
        var issues = new List<ValidationIssue>();

        if (lines.Count == 0)
        {
            issues.Add(new("draft.no-lines", "The draft has no lines."));
        }
        else if (Totals.Total.Amount <= 0m)
        {
            issues.Add(
                new("draft.total-not-positive", "The invoice total must be greater than zero.")
            );
        }

        AddPartyIssues(issues, "issuer", issuer);
        AddPartyIssues(issues, "recipient", recipient);

        if (!sequence.Accepts(issueDate))
        {
            issues.Add(
                new(
                    "numbering.issue-date-not-accepted",
                    $"Issue date {issueDate:yyyy-MM-dd} is outside the series year or earlier than the last issued invoice."
                )
            );
        }

        return issues;
    }

    /// <summary>Validates, numbers and issues the invoice. The draft becomes read-only.</summary>
    public IssuedInvoice Finalize(
        LegalParty issuer,
        LegalParty recipient,
        InvoiceNumberSequence sequence,
        DateOnly issueDate,
        DateTimeOffset issuedAt
    )
    {
        ArgumentNullException.ThrowIfNull(issuer);
        ArgumentNullException.ThrowIfNull(recipient);
        ArgumentNullException.ThrowIfNull(sequence);
        EnsureOpen();

        if (!string.Equals(sequence.SeriesCode, SeriesCode, StringComparison.Ordinal))
        {
            throw new BusinessRuleViolationException(
                "numbering.series-mismatch",
                $"Draft uses series {SeriesCode}, not {sequence.SeriesCode}."
            );
        }

        var issues = Validate(issuer, recipient, issueDate, sequence);
        if (issues.Count > 0)
        {
            throw new BusinessRuleViolationException(
                "draft.invalid",
                string.Join(" ", issues.Select(issue => issue.Message))
            );
        }

        var number = sequence.Allocate(issueDate);
        var invoice = new IssuedInvoice(
            Guid.CreateVersion7(),
            new IssuedInvoiceSource(Id, ProjectId, CustomerId),
            number,
            issueDate,
            OperationDate == issueDate ? null : OperationDate,
            issuedAt,
            issuer,
            recipient,
            Lines
                .Select(line => new IssuedInvoiceLine(
                    line.Position,
                    line.Description,
                    line.Pricing
                ))
                .ToList(),
            Currency.Eur
        );

        Status = DraftStatus.Issued;
        IssuedInvoiceId = invoice.Id;
        return invoice;
    }

    /// <summary>Ensures the draft can still change.</summary>
    public void EnsureOpen()
    {
        if (Status != DraftStatus.Open)
        {
            throw new BusinessRuleViolationException(
                "draft.not-open",
                "The draft has already been issued."
            );
        }
    }

    private static void AddPartyIssues(List<ValidationIssue> issues, string role, LegalParty? party)
    {
        if (party is null)
        {
            issues.Add(new($"{role}.profile-missing", $"The {role} details are missing."));
            return;
        }

        if (string.IsNullOrWhiteSpace(party.LegalName))
        {
            issues.Add(new($"{role}.legal-name-missing", $"The {role} legal name is missing."));
        }

        if (string.IsNullOrWhiteSpace(party.TaxId))
        {
            issues.Add(new($"{role}.tax-id-missing", $"The {role} tax ID is missing."));
        }

        if (party.Address is not { IsComplete: true })
        {
            issues.Add(new($"{role}.address-missing", $"The {role} address is incomplete."));
        }
    }
}
