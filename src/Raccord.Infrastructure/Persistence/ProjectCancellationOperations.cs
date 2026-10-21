using Microsoft.EntityFrameworkCore;
using Raccord.Contracts.Crew;
using Raccord.Modules.Projects.Contracts;

namespace Raccord.Infrastructure.Persistence;

internal sealed class ProjectCancellationOperations(RaccordDbContext db)
    : IProjectCancellationOperations
{
    public async Task<ProjectCancellationOperationsSummary> PreviewAsync(
        Guid projectId,
        CancellationToken cancellationToken
    )
    {
        var functions = await LoadFunctionsAsync(projectId, cancellationToken)
            .ConfigureAwait(false);
        var invitations = await ActiveInvitations(projectId)
            .CountAsync(cancellationToken)
            .ConfigureAwait(false);
        return new(
            functions.Sum(function => function.Crew.Count),
            functions.Sum(function => function.Vehicles.Count),
            invitations
        );
    }

    public async Task ApplyAsync(
        Guid projectId,
        bool removePlannedResources,
        bool withdrawInvitations,
        CancellationToken cancellationToken
    )
    {
        if (removePlannedResources)
        {
            var functions = await LoadFunctionsAsync(projectId, cancellationToken)
                .ConfigureAwait(false);
            foreach (var function in functions)
            {
                db.RemoveRange(function.Crew);
                db.RemoveRange(function.Vehicles);
            }
        }

        if (withdrawInvitations)
        {
            var invitations = await ActiveInvitations(projectId)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            foreach (var invitation in invitations)
                invitation.Status = CrewInvitationStatus.Withdrawn;
        }
    }

    private Task<List<ProjectFunctionRow>> LoadFunctionsAsync(
        Guid projectId,
        CancellationToken cancellationToken
    ) =>
        db
            .ProjectFunctions.Include(function => function.Crew)
            .Include(function => function.Vehicles)
            .Where(function => function.ProjectId == projectId)
            .ToListAsync(cancellationToken);

    private IQueryable<CrewInvitationRow> ActiveInvitations(Guid projectId) =>
        db.CrewInvitations.Where(invitation =>
            db.ProjectFunctions.Any(function =>
                function.Id == invitation.FunctionId && function.ProjectId == projectId
            )
            && invitation.Status != CrewInvitationStatus.Declined
            && invitation.Status != CrewInvitationStatus.Withdrawn
        );
}
