using Raccord.Domain.Common;
using Raccord.Domain.Pricing;

namespace Raccord.Domain.Projects;

/// <summary>An audiovisual job for a customer and the billable work it produces.</summary>
public sealed class Project
{
    private readonly List<BillableLine> billableLines = [];

    private Project()
    {
        Name = string.Empty;
    }

    private Project(string name, Guid customerId, DateOnly startDate, DateOnly endDate)
    {
        Id = Guid.CreateVersion7();
        Name = name;
        CustomerId = customerId;
        StartDate = startDate;
        EndDate = endDate;
    }

    /// <summary>Project identifier.</summary>
    public Guid Id { get; private set; }

    /// <summary>Human-readable project name.</summary>
    public string Name { get; private set; }

    /// <summary>The customer billed for the project.</summary>
    public Guid CustomerId { get; private set; }

    /// <summary>First day of the job.</summary>
    public DateOnly StartDate { get; private set; }

    /// <summary>Last day of the job; also the operation date on invoices.</summary>
    public DateOnly EndDate { get; private set; }

    /// <summary>Billable work recorded for the project.</summary>
    public IReadOnlyList<BillableLine> BillableLines => [.. billableLines.OrderBy(line => line.Position)];

    /// <summary>Creates a project for a customer and period.</summary>
    public static Project Create(string name, Guid customerId, DateOnly startDate, DateOnly endDate)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessRuleViolationException("projects.name-required", "Project name is required.");
        }

        if (endDate < startDate)
        {
            throw new BusinessRuleViolationException("projects.period-invalid", "A project cannot end before it starts.");
        }

        return new Project(name.Trim(), customerId, startDate, endDate);
    }

    /// <summary>Records billable work.</summary>
    public BillableLine AddBillableLine(BillableLineKind kind, string description, LinePricing pricing)
    {
        ArgumentNullException.ThrowIfNull(pricing);
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new BusinessRuleViolationException("projects.billable-line-description-required", "A billable line needs a description.");
        }

        var position = billableLines.Count == 0 ? 1 : billableLines.Max(line => line.Position) + 1;
        var line = new BillableLine(position, kind, description.Trim(), pricing);
        billableLines.Add(line);
        return line;
    }

    /// <summary>Reserves unbilled lines for one draft so they cannot be billed twice.</summary>
    public IReadOnlyList<BillableLine> ReserveForDraft(IReadOnlyCollection<Guid> lineIds, Guid draftId)
    {
        ArgumentNullException.ThrowIfNull(lineIds);
        var lines = lineIds.Distinct().Select(FindLine).OrderBy(line => line.Position).ToList();
        foreach (var line in lines)
        {
            line.Reserve(draftId);
        }

        return lines;
    }

    /// <summary>Returns lines held by a draft to the unbilled pool. Invoiced lines are unaffected.</summary>
    public void ReleaseFromDraft(Guid draftId, IReadOnlyCollection<Guid> lineIds)
    {
        ArgumentNullException.ThrowIfNull(lineIds);
        foreach (var line in billableLines.Where(line => lineIds.Contains(line.Id)))
        {
            line.Release(draftId);
        }
    }

    /// <summary>Marks every line held by the draft as invoiced.</summary>
    public void MarkInvoiced(Guid draftId, Guid invoiceId)
    {
        foreach (var line in billableLines)
        {
            line.MarkInvoiced(draftId, invoiceId);
        }
    }

    private BillableLine FindLine(Guid lineId) =>
        billableLines.Find(line => line.Id == lineId)
        ?? throw new BusinessRuleViolationException("projects.billable-line-not-found", $"Billable line '{lineId}' does not belong to this project.");
}
