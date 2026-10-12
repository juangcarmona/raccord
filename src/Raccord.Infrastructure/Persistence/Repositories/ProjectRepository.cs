using Microsoft.EntityFrameworkCore;
using Raccord.Domain.Projects;
using Raccord.Modules.Projects.Contracts;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class ProjectRepository(RaccordDbContext db) : IBillableWork
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
