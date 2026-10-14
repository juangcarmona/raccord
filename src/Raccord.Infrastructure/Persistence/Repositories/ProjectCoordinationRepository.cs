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
}
