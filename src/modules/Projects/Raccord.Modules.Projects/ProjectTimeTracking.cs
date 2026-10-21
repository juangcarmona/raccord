using Raccord.Contracts.Projects;
using Raccord.Domain.Projects;
using Raccord.Modules.Compliance.Contracts;
using Raccord.Modules.Projects.Contracts;
using Raccord.SharedKernel.Auditing;

namespace Raccord.Modules.Projects;

#pragma warning disable CS1591

/// <summary>Records project time and derives plan-versus-actual reports.</summary>
public sealed class ProjectTimeTracking(
    IBillableWork projects,
    IProjectTimeStore time,
    IAuditTrail audit,
    IProjectsUnitOfWork unitOfWork
)
{
    public async Task<ProjectTimeEntryResponse?> RecordAsync(
        Guid projectId,
        RecordProjectTimeRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var project = await projects.FindAsync(projectId, cancellationToken).ConfigureAwait(false);
        if (project is null)
            return null;
        if (project.Status == ProjectStatus.Complete)
            throw new Raccord.SharedKernel.Common.BusinessRuleViolationException(
                "projects.complete-read-only",
                "Reopen a completed project before recording operational time."
            );
        var entry = ProjectTimeEntry.Record(
            projectId,
            request.TaskId,
            request.Service,
            request.PlannedHours,
            request.ActualHours,
            request.Actor
        );
        time.Add(entry);
        audit.Append(
            new AuditEntry(
                entry.RecordedAt,
                entry.Actor,
                "project.time.recorded",
                "project",
                projectId,
                new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    ["entryId"] = entry.Id.ToString(),
                    ["service"] = entry.Service,
                }
            )
        );
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(entry);
    }

    public async Task<ProjectTimeReportResponse?> GetReportAsync(
        Guid projectId,
        CancellationToken cancellationToken
    )
    {
        if (await projects.FindAsync(projectId, cancellationToken).ConfigureAwait(false) is null)
            return null;
        var entries = await time.ListAsync(projectId, cancellationToken).ConfigureAwait(false);
        var planned = entries.Sum(entry => entry.PlannedHours);
        var actual = entries.Sum(entry => entry.ActualHours);
        return new(
            projectId,
            planned,
            actual,
            actual - planned,
            entries.Select(ToResponse).ToList()
        );
    }

    private static ProjectTimeEntryResponse ToResponse(ProjectTimeEntry entry) =>
        new(
            entry.Id,
            entry.TaskId,
            entry.Service,
            entry.PlannedHours,
            entry.ActualHours,
            entry.ActualHours - entry.PlannedHours,
            entry.Actor,
            entry.RecordedAt
        );
}

#pragma warning restore CS1591
