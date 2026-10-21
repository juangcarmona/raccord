using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Pricing;

namespace Raccord.Domain.Projects;

/// <summary>An audiovisual job for a customer and the billable work it produces.</summary>
public sealed class Project
{
    private readonly List<BillableLine> billableLines = [];
    private readonly List<PackingLine> packingLines = [];
    private readonly List<ProjectTimePeriod> timePeriods = [];

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
        timePeriods.Add(new ProjectTimePeriod("Planning", 0, isDefaultPlanning: true, false));
        timePeriods.Add(new ProjectTimePeriod("Usage", 1, false, isDefaultUsage: true));
    }

    /// <summary>Project identifier.</summary>
    public Guid Id { get; private set; }

    /// <summary>Human-readable project name.</summary>
    public string Name { get; private set; }

    /// <summary>Customer or production reference associated with the project.</summary>
    public string? ExternalReference { get; private set; }

    /// <summary>The customer billed for the project.</summary>
    public Guid CustomerId { get; private set; }

    /// <summary>The selected person at the customer contact, when applicable.</summary>
    public Guid? ContactPersonId { get; private set; }

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

    /// <summary>Whether open work remained when the project was cancelled.</summary>
    public bool HasCancellationWarning { get; private set; }

    /// <summary>UTC instant when the project was cancelled, when applicable.</summary>
    public DateTimeOffset? CancelledAt { get; private set; }

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
        if (status == ProjectStatus.Cancelled)
            throw new BusinessRuleViolationException(
                "projects.cancellation-confirmation-required",
                "Use the cancellation workflow to cancel a project."
            );
        Status = status;
    }

    /// <summary>Cancels the project and records whether operational work remains.</summary>
    public void Cancel(bool hasOpenItems, DateTimeOffset cancelledAt)
    {
        if (Status == ProjectStatus.Cancelled)
            throw new BusinessRuleViolationException(
                "projects.already-cancelled",
                "This project has already been cancelled."
            );
        Status = ProjectStatus.Cancelled;
        HasCancellationWarning = hasOpenItems;
        CancelledAt = cancelledAt.ToUniversalTime();
    }

    /// <summary>Updates the project's editable identifying and scheduling details.</summary>
    public void EditDetails(
        Guid customerId,
        string name,
        DateOnly startDate,
        DateOnly endDate,
        Guid? projectTypeId,
        Guid? teamId,
        string? externalReference = null,
        Guid? contactPersonId = null
    )
    {
        if (Status == ProjectStatus.Complete)
            throw new BusinessRuleViolationException(
                "projects.complete-read-only",
                "Reopen a completed project before editing its operational details."
            );
        if (customerId == Guid.Empty)
            throw new BusinessRuleViolationException(
                "projects.customer-required",
                "A project needs a customer."
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
        if (externalReference?.Trim() is { Length: > 200 })
            throw new BusinessRuleViolationException(
                "projects.external-reference-too-long",
                "A project external reference cannot exceed 200 characters."
            );

        Name = name.Trim();
        ExternalReference = NormalizeExternalReference(externalReference);
        CustomerId = customerId;
        ContactPersonId = contactPersonId;
        StartDate = startDate;
        EndDate = endDate;
        ProjectTypeId = projectTypeId;
        TeamId = teamId;
    }

    private static string? NormalizeExternalReference(string? externalReference)
    {
        var normalized = externalReference?.Trim();
        return string.IsNullOrEmpty(normalized) ? null : normalized;
    }

    /// <summary>Billable work recorded for the project.</summary>
    public IReadOnlyList<BillableLine> BillableLines =>
        [.. billableLines.OrderBy(line => line.Position)];

    /// <summary>Equipment lines currently handled by the warehouse.</summary>
    public IReadOnlyList<PackingLine> PackingLines => [.. packingLines];

    /// <summary>Named periods on the project time schedule.</summary>
    public IReadOnlyList<ProjectTimePeriod> TimePeriods =>
        [.. timePeriods.OrderBy(period => period.Position)];

    /// <summary>Sets a period range when it preserves the usage-within-planning rule.</summary>
    public void SetTimePeriod(Guid periodId, DateTimeOffset? start, DateTimeOffset? end)
    {
        EnsureOperationalEditable();
        var period = timePeriods.Find(candidate => candidate.Id == periodId);
        if (period is null)
            throw new BusinessRuleViolationException(
                "projects.time-period-not-found",
                "The time period does not belong to this project."
            );
        if (start.HasValue != end.HasValue || (start.HasValue && end <= start))
            throw new BusinessRuleViolationException(
                "projects.time-period-invalid",
                "A time period needs both boundaries, and its end must follow its start."
            );

        var planning = timePeriods.Single(candidate => candidate.IsDefaultPlanning);
        var usage = timePeriods.Single(candidate => candidate.IsDefaultUsage);
        ValidateUsageWithinPlanning(
            planning == period ? start : planning.Start,
            planning == period ? end : planning.End,
            usage == period ? start : usage.Start,
            usage == period ? end : usage.End
        );

        period.SetRange(start, end);
    }

    /// <summary>Adds an untimed period to the end of the schedule.</summary>
    public ProjectTimePeriod AddTimePeriod(string name)
    {
        EnsureOperationalEditable();
        var normalizedName = ValidateTimePeriodName(name);
        var period = new ProjectTimePeriod(
            normalizedName,
            timePeriods.Count,
            isDefaultPlanning: false,
            isDefaultUsage: false
        );
        timePeriods.Add(period);
        return period;
    }

    /// <summary>Updates a named period, its range and document visibility.</summary>
    public void UpdateTimePeriod(
        Guid periodId,
        string name,
        DateTimeOffset? start,
        DateTimeOffset? end,
        bool showOnInternalDocuments,
        bool showOnExternalDocuments
    )
    {
        EnsureOperationalEditable();
        var period = FindTimePeriod(periodId);
        var normalizedName = ValidateTimePeriodName(name);
        ValidateRange(start, end);

        var planning = timePeriods.Single(candidate => candidate.IsDefaultPlanning);
        var usage = timePeriods.Single(candidate => candidate.IsDefaultUsage);
        ValidateUsageWithinPlanning(
            planning == period ? start : planning.Start,
            planning == period ? end : planning.End,
            usage == period ? start : usage.Start,
            usage == period ? end : usage.End
        );
        period.Update(normalizedName, start, end, showOnInternalDocuments, showOnExternalDocuments);
    }

    /// <summary>Duplicates a period immediately after its source without its default designations.</summary>
    public ProjectTimePeriod DuplicateTimePeriod(Guid periodId)
    {
        EnsureOperationalEditable();
        var source = FindTimePeriod(periodId);
        var insertAt = source.Position + 1;
        foreach (var period in timePeriods.Where(period => period.Position >= insertAt))
            period.SetPosition(period.Position + 1);
        var duplicate = source.Duplicate(insertAt);
        timePeriods.Add(duplicate);
        return duplicate;
    }

    /// <summary>Sets the schedule order using every project period exactly once.</summary>
    public void ReorderTimePeriods(IReadOnlyList<Guid> orderedPeriodIds)
    {
        EnsureOperationalEditable();
        ValidateTimePeriodOrder(orderedPeriodIds);

        for (var position = 0; position < orderedPeriodIds.Count; position++)
            FindTimePeriod(orderedPeriodIds[position]).SetPosition(position);
    }

    /// <summary>Validates that an order contains every project period exactly once.</summary>
    public void ValidateTimePeriodOrder(IReadOnlyList<Guid> orderedPeriodIds)
    {
        EnsureOperationalEditable();
        ArgumentNullException.ThrowIfNull(orderedPeriodIds);
        if (
            orderedPeriodIds.Count != timePeriods.Count
            || orderedPeriodIds.Distinct().Count() != timePeriods.Count
            || orderedPeriodIds.Any(id => timePeriods.All(period => period.Id != id))
        )
            throw new BusinessRuleViolationException(
                "projects.time-period-order-invalid",
                "The order must include every project time period exactly once."
            );
    }

    /// <summary>Moves the requested default planning or usage role to a period.</summary>
    public void DesignateTimePeriod(Guid periodId, bool isDefaultPlanning, bool isDefaultUsage)
    {
        EnsureOperationalEditable();
        var selected = FindTimePeriod(periodId);
        if (!isDefaultPlanning && !isDefaultUsage)
            throw new BusinessRuleViolationException(
                "projects.time-period-designation-required",
                "Choose the planning period, usage period or both."
            );

        var planning = isDefaultPlanning
            ? selected
            : timePeriods.Single(period => period.IsDefaultPlanning);
        var usage = isDefaultUsage ? selected : timePeriods.Single(period => period.IsDefaultUsage);
        ValidateUsageWithinPlanning(planning.Start, planning.End, usage.Start, usage.End);

        if (isDefaultPlanning)
        {
            foreach (var period in timePeriods)
                period.SetDesignation(false, period.IsDefaultUsage);
        }
        if (isDefaultUsage)
        {
            foreach (var period in timePeriods)
                period.SetDesignation(period.IsDefaultPlanning, false);
        }
        selected.SetDesignation(
            isDefaultPlanning || selected.IsDefaultPlanning,
            isDefaultUsage || selected.IsDefaultUsage
        );
    }

    private ProjectTimePeriod FindTimePeriod(Guid periodId) =>
        timePeriods.Find(candidate => candidate.Id == periodId)
        ?? throw new BusinessRuleViolationException(
            "projects.time-period-not-found",
            "The time period does not belong to this project."
        );

    private static string ValidateTimePeriodName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Trim().Length > 120)
            throw new BusinessRuleViolationException(
                "projects.time-period-name-invalid",
                "A time period needs a name of at most 120 characters."
            );
        return name.Trim();
    }

    private static void ValidateRange(DateTimeOffset? start, DateTimeOffset? end)
    {
        if (start.HasValue != end.HasValue || (start.HasValue && end <= start))
            throw new BusinessRuleViolationException(
                "projects.time-period-invalid",
                "A time period needs both boundaries, and its end must follow its start."
            );
    }

    private static void ValidateUsageWithinPlanning(
        DateTimeOffset? planningStart,
        DateTimeOffset? planningEnd,
        DateTimeOffset? usageStart,
        DateTimeOffset? usageEnd
    )
    {
        if (
            planningStart.HasValue
            && usageStart.HasValue
            && (
                usageStart < planningStart
                || usageEnd > planningEnd
                || (usageStart == planningStart && usageEnd == planningEnd)
            )
        )
            throw new BusinessRuleViolationException(
                "projects.usage-period-outside-planning",
                "The usage period must be shorter than and contained by the planning period."
            );
    }

    private void EnsureOperationalEditable()
    {
        if (Status == ProjectStatus.Complete)
            throw new BusinessRuleViolationException(
                "projects.complete-read-only",
                "Reopen a completed project before editing its operational details."
            );
    }

    /// <summary>Adds a quantity of equipment to the project's packing list.</summary>
    public PackingLine AddPackingLine(
        Guid equipmentId,
        int quantity,
        PackingLineRole role = PackingLineRole.Required
    )
    {
        EnsureOperationalEditable();
        var line = PackingLine.Create(equipmentId, quantity, role);
        packingLines.Add(line);
        return line;
    }

    /// <summary>Adjusts an unlocked packing line's quantity and role.</summary>
    public PackingLine AdjustPackingLine(Guid lineId, int quantity, PackingLineRole role)
    {
        EnsureOperationalEditable();
        var line = FindPackingLine(lineId);
        line.Adjust(quantity, role);
        return line;
    }

    /// <summary>Marks a packing line as prepared.</summary>
    public PackingLine Pack(Guid lineId, string actor, DateTimeOffset at)
    {
        EnsureOperationalEditable();
        var line = FindPackingLine(lineId);
        line.Pack(actor, at);
        return line;
    }

    /// <summary>Marks a prepared line as dispatched.</summary>
    public PackingLine BookOut(Guid lineId, string actor, DateTimeOffset at)
    {
        EnsureOperationalEditable();
        var line = FindPackingLine(lineId);
        line.BookOut(actor, at);
        return line;
    }

    /// <summary>Marks a dispatched line as received.</summary>
    public PackingLine Receive(Guid lineId, string actor, DateTimeOffset at)
    {
        EnsureOperationalEditable();
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
        EnsureOperationalEditable();
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
