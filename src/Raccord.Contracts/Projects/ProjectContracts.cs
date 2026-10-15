namespace Raccord.Contracts.Projects;

/// <summary>Creates a project.</summary>
/// <param name="Name">Project name.</param>
/// <param name="CustomerId">Customer billed for the project.</param>
/// <param name="StartDate">First day of the job.</param>
/// <param name="EndDate">Last day of the job.</param>
/// <param name="ProjectTypeId">Optional reusable project type.</param>
/// <param name="TeamId">Optional assigned team.</param>
public sealed record CreateProjectRequest(
    string Name,
    Guid CustomerId,
    DateOnly StartDate,
    DateOnly EndDate,
    Guid? ProjectTypeId = null,
    Guid? TeamId = null
);

/// <summary>Records incoming project work.</summary>
public sealed record ReceiveProjectRequest(
    string Name,
    string RequesterName,
    string RequesterEmail,
    Guid? CustomerId,
    string? Description,
    DateOnly StartDate,
    DateOnly EndDate,
    Guid? ProjectTypeId,
    Guid? TeamId
);

/// <summary>Declines an incoming project request.</summary>
public sealed record DeclineProjectRequest(string Reason);

/// <summary>Creates a reusable project type or team.</summary>
public sealed record CreateProjectClassificationRequest(string Name, string? Description);

/// <summary>Incoming project request.</summary>
public sealed record ProjectRequestResponse(
    Guid Id,
    string Name,
    string RequesterName,
    string RequesterEmail,
    Guid? CustomerId,
    string? Description,
    DateOnly StartDate,
    DateOnly EndDate,
    Guid? ProjectTypeId,
    Guid? TeamId,
    string Status,
    Guid? ProjectId,
    string? DeclineReason,
    DateTimeOffset ReceivedAt,
    DateTimeOffset? ResolvedAt
);

/// <summary>Project classification.</summary>
public sealed record ProjectTypeResponse(Guid Id, string Name, string? Description);

/// <summary>Project team.</summary>
public sealed record ProjectTeamResponse(Guid Id, string Name, string? Description);

/// <summary>Creates a task status.</summary>
public sealed record CreateProjectTaskStatusRequest(
    string Name,
    int Position,
    bool CompletesProject
);

/// <summary>Creates a project task.</summary>
public sealed record CreateProjectTaskRequest(
    Guid ProjectId,
    string Title,
    string? Description,
    Guid StatusId,
    int Position,
    Guid? AssigneeId
);

/// <summary>Moves a task on the board.</summary>
public sealed record ProgressProjectTaskRequest(Guid StatusId, int Position);

/// <summary>A configured task status.</summary>
public sealed record ProjectTaskStatusResponse(
    Guid Id,
    string Name,
    int Position,
    bool CompletesProject
);

/// <summary>A project task.</summary>
public sealed record ProjectTaskResponse(
    Guid Id,
    Guid ProjectId,
    string Title,
    string? Description,
    Guid StatusId,
    int Position,
    Guid? AssigneeId
);

/// <summary>A project board with automatic progress.</summary>
public sealed record ProjectBoardResponse(
    Guid ProjectId,
    decimal ProgressPercent,
    IReadOnlyList<ProjectTaskResponse> Tasks
);

/// <summary>Creates scheduled project work.</summary>
public sealed record CreateProjectScheduleRequest(
    Guid ProjectId,
    string Title,
    DateOnly StartDate,
    DateOnly EndDate,
    Guid? AssigneeId
);

/// <summary>Shifts scheduled project work.</summary>
public sealed record ShiftProjectScheduleRequest(int Days);

/// <summary>A scheduled project work item.</summary>
public sealed record ProjectScheduleResponse(
    Guid Id,
    Guid ProjectId,
    string Title,
    DateOnly StartDate,
    DateOnly EndDate,
    Guid? AssigneeId
);

/// <summary>Adds billable work to a project.</summary>
/// <param name="Kind">One of <c>equipment</c>, <c>labour</c>, <c>transport</c>, <c>consumable</c>, <c>service</c>, <c>other</c>.</param>
/// <param name="Description">Text printed on the invoice.</param>
/// <param name="Quantity">Units, days or hours; greater than zero.</param>
/// <param name="UnitPrice">Net unit price with up to four decimals.</param>
/// <param name="DiscountPercent">Line discount, 0–100.</param>
/// <param name="VatRate">VAT percentage, 0–100.</param>
public sealed record AddBillableLineRequest(
    string Kind,
    string Description,
    decimal Quantity,
    decimal UnitPrice,
    decimal DiscountPercent,
    decimal VatRate
);

#pragma warning disable CS1591

public sealed record PlanEquipmentRequest(Guid EquipmentId, int Quantity);

public sealed record PlannedEquipmentResponse(Guid EquipmentId, int Quantity);

public sealed record PackingLineResponse(
    Guid Id,
    Guid EquipmentId,
    int Quantity,
    bool IsPacked,
    bool IsBookedOut,
    bool IsReceived,
    string? PackedBy,
    string? BookedOutBy,
    string? ReceivedBy
);

public sealed record PackLineRequest(string Actor);

public sealed record BookOutLineRequest(string Actor);

public sealed record ReceiveLineRequest(string Actor);

#pragma warning restore CS1591

/// <summary>A project in a list.</summary>
/// <param name="Id">Project identifier.</param>
/// <param name="Name">Project name.</param>
/// <param name="CustomerId">Customer identifier.</param>
/// <param name="CustomerName">Customer legal name.</param>
/// <param name="StartDate">First day of the job.</param>
/// <param name="EndDate">Last day of the job.</param>
/// <param name="UnbilledLineCount">Lines not yet in a draft or invoice.</param>
/// <param name="UnbilledNetAmount">Net amount of the unbilled lines.</param>
/// <param name="Currency">ISO currency code.</param>
/// <param name="ProjectTypeId">Assigned reusable project type.</param>
/// <param name="TeamId">Assigned team.</param>
public sealed record ProjectSummaryResponse(
    Guid Id,
    string Name,
    Guid CustomerId,
    string CustomerName,
    DateOnly StartDate,
    DateOnly EndDate,
    int UnbilledLineCount,
    decimal UnbilledNetAmount,
    string Currency,
    Guid? ProjectTypeId = null,
    Guid? TeamId = null
);

/// <summary>A project with its billable lines.</summary>
/// <param name="Id">Project identifier.</param>
/// <param name="Name">Project name.</param>
/// <param name="CustomerId">Customer identifier.</param>
/// <param name="CustomerName">Customer legal name.</param>
/// <param name="StartDate">First day of the job.</param>
/// <param name="EndDate">Last day of the job.</param>
/// <param name="Currency">ISO currency code.</param>
/// <param name="BillableLines">Billable work.</param>
/// <param name="ProjectTypeId">Assigned reusable project type.</param>
/// <param name="TeamId">Assigned team.</param>
/// <param name="PackingLines">Warehouse packing lines.</param>
public sealed record ProjectResponse(
    Guid Id,
    string Name,
    Guid CustomerId,
    string CustomerName,
    DateOnly StartDate,
    DateOnly EndDate,
    string Currency,
    IReadOnlyList<BillableLineResponse> BillableLines,
    Guid? ProjectTypeId = null,
    Guid? TeamId = null,
    IReadOnlyList<PackingLineResponse>? PackingLines = null
);

/// <summary>A billable line.</summary>
/// <param name="Id">Line identifier.</param>
/// <param name="Kind">Line kind.</param>
/// <param name="Description">Text printed on the invoice.</param>
/// <param name="Quantity">Units, days or hours.</param>
/// <param name="UnitPrice">Net unit price.</param>
/// <param name="DiscountPercent">Line discount percentage.</param>
/// <param name="VatRate">VAT percentage.</param>
/// <param name="NetAmount">Net amount calculated by the server.</param>
/// <param name="BillingState">One of <c>unbilled</c>, <c>in-draft</c>, <c>invoiced</c>.</param>
/// <param name="DraftId">Draft holding the line.</param>
/// <param name="InvoiceId">Invoice that billed the line.</param>
public sealed record BillableLineResponse(
    Guid Id,
    string Kind,
    string Description,
    decimal Quantity,
    decimal UnitPrice,
    decimal DiscountPercent,
    decimal VatRate,
    decimal NetAmount,
    string BillingState,
    Guid? DraftId,
    Guid? InvoiceId
);
