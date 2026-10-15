using Microsoft.EntityFrameworkCore;
using Raccord.Domain.Projects;
using Raccord.Modules.Projects.Contracts;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class ProjectContextRepository(RaccordDbContext db) : IProjectContextStore
{
    public async Task<IReadOnlyList<ProjectContextEntry>> ListAsync(
        Guid projectId,
        CancellationToken cancellationToken
    ) =>
        await db
            .ProjectContextEntries.AsNoTracking()
            .Where(entry => entry.ProjectId == projectId)
            .OrderBy(entry => entry.CreatedAt)
            .ThenBy(entry => entry.Id)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    public void Add(ProjectContextEntry entry) => db.ProjectContextEntries.Add(entry);
}
