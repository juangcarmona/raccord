using Raccord.SharedKernel.Common;

namespace Raccord.Domain.Projects;

/// <summary>Incoming work request and its conversion history.</summary>
public sealed class ProjectRequest
{
    private ProjectRequest() { }

    private ProjectRequest(
        string name,
        string requesterName,
        string requesterEmail,
        Guid? customerId,
        string? description,
        DateOnly startDate,
        DateOnly endDate,
        Guid? projectTypeId,
        Guid? teamId
    )
    {
        Id = Guid.CreateVersion7();
        Name = name;
        RequesterName = requesterName;
        RequesterEmail = requesterEmail;
        CustomerId = customerId;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        ProjectTypeId = projectTypeId;
        TeamId = teamId;
        Status = ProjectRequestStatus.Received;
        ReceivedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>Request identifier.</summary>
    public Guid Id { get; private set; }

    /// <summary>Requested project name.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Name supplied by the requester.</summary>
    public string RequesterName { get; private set; } = string.Empty;

    /// <summary>Email supplied by the requester.</summary>
    public string RequesterEmail { get; private set; } = string.Empty;

    /// <summary>Existing customer linked to the request.</summary>
    public Guid? CustomerId { get; private set; }

    /// <summary>Additional request context.</summary>
    public string? Description { get; private set; }

    /// <summary>Requested first day.</summary>
    public DateOnly StartDate { get; private set; }

    /// <summary>Requested final day.</summary>
    public DateOnly EndDate { get; private set; }

    /// <summary>Requested project type.</summary>
    public Guid? ProjectTypeId { get; private set; }

    /// <summary>Requested project team.</summary>
    public Guid? TeamId { get; private set; }

    /// <summary>Current lifecycle status.</summary>
    public ProjectRequestStatus Status { get; private set; }

    /// <summary>Created project after conversion.</summary>
    public Guid? ProjectId { get; private set; }

    /// <summary>Reason supplied when declined.</summary>
    public string? DeclineReason { get; private set; }

    /// <summary>When the request was received.</summary>
    public DateTimeOffset ReceivedAt { get; private set; }

    /// <summary>When the request was converted or declined.</summary>
    public DateTimeOffset? ResolvedAt { get; private set; }

    /// <summary>Receives and validates an incoming request.</summary>
    public static ProjectRequest Receive(
        string name,
        string requesterName,
        string requesterEmail,
        Guid? customerId,
        string? description,
        DateOnly startDate,
        DateOnly endDate,
        Guid? projectTypeId,
        Guid? teamId
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessRuleViolationException(
                "projects.request-name-required",
                "Request name is required."
            );
        if (string.IsNullOrWhiteSpace(requesterName))
            throw new BusinessRuleViolationException(
                "projects.requester-name-required",
                "Requester name is required."
            );
        if (string.IsNullOrWhiteSpace(requesterEmail))
            throw new BusinessRuleViolationException(
                "projects.requester-email-required",
                "Requester email is required."
            );
        if (endDate < startDate)
            throw new BusinessRuleViolationException(
                "projects.request-period-invalid",
                "A request cannot end before it starts."
            );

        return new ProjectRequest(
            name.Trim(),
            requesterName.Trim(),
            requesterEmail.Trim(),
            customerId,
            string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            startDate,
            endDate,
            projectTypeId,
            teamId
        );
    }

    /// <summary>Marks the request as converted to a project.</summary>
    public void Convert(Guid projectId)
    {
        if (Status != ProjectRequestStatus.Received)
            throw new BusinessRuleViolationException(
                "projects.request-already-resolved",
                "The request is already resolved."
            );
        ProjectId = projectId;
        Status = ProjectRequestStatus.Converted;
        ResolvedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>Declines the request with an auditable reason.</summary>
    public void Decline(string reason)
    {
        if (Status != ProjectRequestStatus.Received)
            throw new BusinessRuleViolationException(
                "projects.request-already-resolved",
                "The request is already resolved."
            );
        if (string.IsNullOrWhiteSpace(reason))
            throw new BusinessRuleViolationException(
                "projects.decline-reason-required",
                "A decline reason is required."
            );
        DeclineReason = reason.Trim();
        Status = ProjectRequestStatus.Declined;
        ResolvedAt = DateTimeOffset.UtcNow;
    }
}
