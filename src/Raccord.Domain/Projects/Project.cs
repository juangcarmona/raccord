using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Pricing;

namespace Raccord.Domain.Projects;

/// <summary>An audiovisual job for a customer and the billable work it produces.</summary>
public sealed class Project
{
    private readonly List<BillableLine> billableLines = [];
    private readonly List<PackingLine> packingLines = [];

    private Project()
    {
        Name = string.Empty;
    }

    private Project(
        string name,
        Guid customerId,
        DateOnly startDate,
        DateOnly endDate,
        Guid? projectTypeId,
        Guid? teamId
    )
    {
        Id = Guid.CreateVersion7();
        Name = name;
        CustomerId = customerId;
        StartDate = startDate;
        EndDate = endDate;
        ProjectTypeId = projectTypeId;
        TeamId = teamId;
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

    /// <summary>Configured project type, when assigned.</summary>
    public Guid? ProjectTypeId { get; private set; }

    /// <summary>Assigned project team, when assigned.</summary>
    public Guid? TeamId { get; private set; }

    /// <summary>Commercial lifecycle status, independent of equipment warehouse status.</summary>
    public ProjectStatus Status { get; private set; } = ProjectStatus.Draft;

    /// <summary>Automatic completion percentage based on task statuses.</summary>
    public decimal ProgressPercent { get; private set; }

    /// <summary>Fixed refundable security deposit, when requested.</summary>
    public decimal? RefundableDepositAmount { get; private set; }

    /// <summary>Current refundable deposit lifecycle status.</summary>
    public DepositStatus DepositStatus { get; private set; }

    /// <summary>Requests or changes the fixed refundable deposit.</summary>
    public void SetRefundableDeposit(decimal amount)
    {
        if (amount <= 0m)
            throw new BusinessRuleViolationException(
                "deposit.amount-positive",
                "A refundable deposit must be a positive fixed amount."
            );
        RefundableDepositAmount = amount;
        DepositStatus = DepositStatus.NotPaid;
    }

    /// <summary>Moves a deposit through receipt and return.</summary>
    public void SetDepositStatus(DepositStatus status)
    {
        if (RefundableDepositAmount is null)
            throw new BusinessRuleViolationException(
                "deposit.not-configured",
                "Configure a deposit first."
            );
        if (status == DepositStatus.Returned && DepositStatus != DepositStatus.Paid)
            throw new BusinessRuleViolationException(
                "deposit.return-requires-paid",
                "A deposit must be paid before it can be returned."
            );
        DepositStatus = status;
    }

    /// <summary>Updates the automatic progress percentage.</summary>
    public void UpdateProgress(decimal progressPercent)
    {
        if (progressPercent is < 0 or > 100)
            throw new BusinessRuleViolationException(
                "projects.progress-invalid",
                "Project progress must be between 0 and 100."
            );
        ProgressPercent = progressPercent;
    }

    /// <summary>Changes the project lifecycle status manually.</summary>
    public void ChangeStatus(ProjectStatus status)
    {
        if (!Enum.IsDefined(status))
            throw new BusinessRuleViolationException(
                "projects.status-invalid",
                "Project status must be one of the fixed lifecycle statuses."
            );
        Status = status;
    }

    /// <summary>Updates the project's editable identifying and scheduling details.</summary>
    public void EditDetails(
        string name,
        DateOnly startDate,
        DateOnly endDate,
        Guid? projectTypeId,
        Guid? teamId
    )
    {
        if (Status == ProjectStatus.Complete)
            throw new BusinessRuleViolationException(
                "projects.complete-read-only",
                "Reopen a completed project before editing its operational details."
            );
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessRuleViolationException(
                "projects.name-required",
                "Project name is required."
            );
        if (endDate < startDate)
            throw new BusinessRuleViolationException(
                "projects.period-invalid",
                "A project cannot end before it starts."
            );

        Name = name.Trim();
        StartDate = startDate;
        EndDate = endDate;
        ProjectTypeId = projectTypeId;
        TeamId = teamId;
    }

    /// <summary>Billable work recorded for the project.</summary>
    public IReadOnlyList<BillableLine> BillableLines =>
        [.. billableLines.OrderBy(line => line.Position)];

    /// <summary>Equipment lines currently handled by the warehouse.</summary>
    public IReadOnlyList<PackingLine> PackingLines => [.. packingLines];

    /// <summary>Adds a quantity of equipment to the project's packing list.</summary>
    public PackingLine AddPackingLine(
        Guid equipmentId,
        int quantity,
        PackingLineRole role = PackingLineRole.Required
    )
    {
        var line = PackingLine.Create(equipmentId, quantity, role);
        packingLines.Add(line);
        return line;
    }

    /// <summary>Adjusts an unlocked packing line's quantity and role.</summary>
    public PackingLine AdjustPackingLine(Guid lineId, int quantity, PackingLineRole role)
    {
        var line = FindPackingLine(lineId);
        line.Adjust(quantity, role);
        return line;
    }

    /// <summary>Marks a packing line as prepared.</summary>
    public PackingLine Pack(Guid lineId, string actor, DateTimeOffset at)
    {
        var line = FindPackingLine(lineId);
        line.Pack(actor, at);
        return line;
    }

    /// <summary>Marks a prepared line as dispatched.</summary>
    public PackingLine BookOut(Guid lineId, string actor, DateTimeOffset at)
    {
        var line = FindPackingLine(lineId);
        line.BookOut(actor, at);
        return line;
    }

    /// <summary>Marks a dispatched line as received.</summary>
    public PackingLine Receive(Guid lineId, string actor, DateTimeOffset at)
    {
        var line = FindPackingLine(lineId);
        line.Receive(actor, at);
        return line;
    }

    /// <summary>Creates a project for a customer and period.</summary>
    public static Project Create(
        string name,
        Guid customerId,
        DateOnly startDate,
        DateOnly endDate,
        Guid? projectTypeId = null,
        Guid? teamId = null
    )
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessRuleViolationException(
                "projects.name-required",
                "Project name is required."
            );
        }

        if (endDate < startDate)
        {
            throw new BusinessRuleViolationException(
                "projects.period-invalid",
                "A project cannot end before it starts."
            );
        }

        return new Project(name.Trim(), customerId, startDate, endDate, projectTypeId, teamId);
    }

    /// <summary>Records billable work.</summary>
    public BillableLine AddBillableLine(
        BillableLineKind kind,
        string description,
        LinePricing pricing
    )
    {
        ArgumentNullException.ThrowIfNull(pricing);
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new BusinessRuleViolationException(
                "projects.billable-line-description-required",
                "A billable line needs a description."
            );
        }

        var position = billableLines.Count == 0 ? 1 : billableLines.Max(line => line.Position) + 1;
        var line = new BillableLine(position, kind, description.Trim(), pricing);
        billableLines.Add(line);
        return line;
    }

    /// <summary>Reserves unbilled lines for one draft so they cannot be billed twice.</summary>
    public IReadOnlyList<BillableLine> ReserveForDraft(
        IReadOnlyCollection<Guid> lineIds,
        Guid draftId
    )
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
        ?? throw new BusinessRuleViolationException(
            "projects.billable-line-not-found",
            $"Billable line '{lineId}' does not belong to this project."
        );

    private PackingLine FindPackingLine(Guid lineId) =>
        packingLines.Find(line => line.Id == lineId)
        ?? throw new BusinessRuleViolationException(
            "projects.packing-line-not-found",
            $"Packing line '{lineId}' does not belong to this project."
        );
}
