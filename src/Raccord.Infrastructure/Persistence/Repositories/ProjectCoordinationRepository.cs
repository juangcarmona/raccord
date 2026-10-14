using Microsoft.EntityFrameworkCore;
using Raccord.Domain.Projects;
using Raccord.Modules.Projects.Contracts;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class ProjectCoordinationRepository(RaccordDbContext db) : IProjectCoordinationStore
{
    public async Task<IReadOnlyList<ProjectRequest>> ListRequestsAsync(
        CancellationToken cancellationToken
    ) =>
        await db
            .ProjectRequests.OrderByDescending(request => request.ReceivedAt)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    public Task<ProjectRequest?> FindRequestAsync(Guid id, CancellationToken cancellationToken) =>
        db.ProjectRequests.FirstOrDefaultAsync(request => request.Id == id, cancellationToken);

    public async Task<IReadOnlyList<ProjectType>> ListProjectTypesAsync(
        CancellationToken cancellationToken
    ) =>
        await db
            .ProjectTypes.OrderBy(type => type.Name)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    public async Task<IReadOnlyList<ProjectTeam>> ListTeamsAsync(
        CancellationToken cancellationToken
    ) =>
        await db
            .ProjectTeams.OrderBy(team => team.Name)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    public void Add(ProjectRequest request) => db.ProjectRequests.Add(request);

    public void Add(ProjectType projectType) => db.ProjectTypes.Add(projectType);

    public void Add(ProjectTeam team) => db.ProjectTeams.Add(team);

    public async Task<IReadOnlyList<ProjectTaskStatus>> ListTaskStatusesAsync(
        CancellationToken cancellationToken
    ) =>
        await db
            .ProjectTaskStatuses.OrderBy(status => status.Position)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    public Task<ProjectTaskStatus?> FindTaskStatusAsync(
        Guid id,
        CancellationToken cancellationToken
    ) => db.ProjectTaskStatuses.FirstOrDefaultAsync(status => status.Id == id, cancellationToken);

    public async Task<IReadOnlyList<ProjectTask>> ListTasksAsync(
        Guid projectId,
        CancellationToken cancellationToken
    ) =>
        await db
            .ProjectTasks.Where(task => task.ProjectId == projectId)
            .OrderBy(task => task.StatusId)
            .ThenBy(task => task.Position)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    public Task<ProjectTask?> FindTaskAsync(Guid id, CancellationToken cancellationToken) =>
        db.ProjectTasks.FirstOrDefaultAsync(task => task.Id == id, cancellationToken);

    public void Add(ProjectTaskStatus status) => db.ProjectTaskStatuses.Add(status);

    public void Add(ProjectTask task) => db.ProjectTasks.Add(task);
}
