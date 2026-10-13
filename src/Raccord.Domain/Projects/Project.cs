using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Pricing;

namespace Raccord.Domain.Projects;

/// <summary>An audiovisual job for a customer and the billable work it produces.</summary>
public sealed class Project
{
    private readonly List<BillableLine> billableLines = [];
    private readonly List<PlannedEquipmentLine> plannedEquipmentLines = [];
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
        decimal discountPercent,
        string? paymentTerms
    )
    {
        Id = Guid.CreateVersion7();
        Name = name;
        CustomerId = customerId;
        StartDate = startDate;
        EndDate = endDate;
        DefaultDiscountPercent = discountPercent;
        PaymentTerms = paymentTerms;
        Status = ProjectStatus.Draft;
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

    /// <summary>Commercial discount copied from the contact at creation time.</summary>
    public decimal DefaultDiscountPercent { get; private set; }

    /// <summary>Payment terms copied from the contact at creation time.</summary>
    public string? PaymentTerms { get; private set; }

    /// <summary>Current planning lifecycle status.</summary>
    public ProjectStatus Status { get; private set; }

    /// <summary>Changes the project planning lifecycle status.</summary>
    public void SetStatus(ProjectStatus status) => Status = status;

    /// <summary>Moves the project and all planned equipment by the same number of days.</summary>
    public void ShiftDates(int days)
    {
        StartDate = StartDate.AddDays(days);
        EndDate = EndDate.AddDays(days);
        foreach (var line in plannedEquipmentLines)
            line.ShiftDays(days);
    }

    /// <summary>Billable work recorded for the project.</summary>
    public IReadOnlyList<BillableLine> BillableLines =>
        [.. billableLines.OrderBy(line => line.Position)];

    /// <summary>Equipment planned for this project.</summary>
    public IReadOnlyList<PlannedEquipmentLine> PlannedEquipmentLines => [.. plannedEquipmentLines];

    /// <summary>Equipment quantities prepared for warehouse fulfillment.</summary>
    public IReadOnlyList<PackingLine> PackingLines => [.. packingLines];

    /// <summary>Creates a packing list from the project's planned equipment.</summary>
    public IReadOnlyList<PackingLine> CreatePackingList()
    {
        if (packingLines.Count > 0)
            throw new BusinessRuleViolationException(
                "projects.packing-list-exists",
                "A packing list already exists for this project."
            );
        if (plannedEquipmentLines.Count == 0)
            throw new BusinessRuleViolationException(
                "projects.packing-list-empty",
                "The project has no planned equipment to pack."
            );
        packingLines.AddRange(
            plannedEquipmentLines.Select(line => new PackingLine(line.EquipmentId, line.Quantity))
        );
        return PackingLines;
    }

    /// <summary>Marks one packing line ready for dispatch.</summary>
    public PackingLine Pack(Guid lineId, string actor, DateTimeOffset packedAt)
    {
        var line =
            packingLines.Find(candidate => candidate.Id == lineId)
            ?? throw new BusinessRuleViolationException(
                "projects.packing-line-not-found",
                "The packing line does not belong to this project."
            );
        line.Pack(actor, packedAt);
        return line;
    }

    /// <summary>Books a packed line out of the warehouse.</summary>
    public PackingLine BookOut(Guid lineId, string actor, DateTimeOffset bookedOutAt)
    {
        var line =
            packingLines.Find(candidate => candidate.Id == lineId)
            ?? throw new BusinessRuleViolationException(
                "projects.packing-line-not-found",
                "The packing line does not belong to this project."
            );
        line.BookOut(actor, bookedOutAt);
        return line;
    }

    /// <summary>Receives a booked-out line back into the warehouse.</summary>
    public PackingLine Receive(Guid lineId, string actor, DateTimeOffset receivedAt)
    {
        var line =
            packingLines.Find(candidate => candidate.Id == lineId)
            ?? throw new BusinessRuleViolationException(
                "projects.packing-line-not-found",
                "The packing line does not belong to this project."
            );
        line.Receive(actor, receivedAt);
        return line;
    }

    /// <summary>Records a loss or damage exception on a received line.</summary>
    public PackingLine ReportException(
        Guid lineId,
        string kind,
        string description,
        string actor,
        DateTimeOffset reportedAt
    )
    {
        var line =
            packingLines.Find(candidate => candidate.Id == lineId)
            ?? throw new BusinessRuleViolationException(
                "projects.packing-line-not-found",
                "The packing line does not belong to this project."
            );
        line.ReportException(kind, description, actor, reportedAt);
        return line;
    }

    /// <summary>Plans equipment for a period within the project period.</summary>
    public PlannedEquipmentLine PlanEquipment(
        Guid equipmentId,
        int quantity,
        DateOnly startDate,
        DateOnly endDate
    )
    {
        if (quantity <= 0 || startDate < StartDate || endDate > EndDate || endDate < startDate)
            throw new BusinessRuleViolationException(
                "projects.equipment-period-invalid",
                "Equipment planning must use a positive quantity within the project period."
            );
        var line = new PlannedEquipmentLine(equipmentId, quantity, startDate, endDate);
        plannedEquipmentLines.Add(line);
        return line;
    }

    /// <summary>Creates a project for a customer and period.</summary>
    public static Project Create(
        string name,
        Guid customerId,
        DateOnly startDate,
        DateOnly endDate,
        decimal discountPercent = 0,
        string? paymentTerms = null
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

        if (discountPercent is < 0 or > 100)
        {
            throw new BusinessRuleViolationException(
                "projects.discount-invalid",
                "Discount must be between 0 and 100 percent."
            );
        }

        return new Project(
            name.Trim(),
            customerId,
            startDate,
            endDate,
            discountPercent,
            paymentTerms?.Trim()
        );
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
}
