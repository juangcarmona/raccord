using Microsoft.EntityFrameworkCore;
using Raccord.Application.Projects;
using Raccord.Domain.Projects;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class ProjectRepository(RaccordDbContext db) : IProjectRepository
{
    public Task<Project?> FindAsync(Guid id, CancellationToken cancellationToken) =>
        db.Projects.FirstOrDefaultAsync(project => project.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Project>> ListAsync(CancellationToken cancellationToken) =>
        await db
            .Projects.OrderByDescending(project => project.StartDate)
            .ThenBy(project => project.Name)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    public void Add(Project project) => db.Projects.Add(project);
}
