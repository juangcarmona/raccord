using Microsoft.EntityFrameworkCore;
using Raccord.Domain.Projects;
using Raccord.Modules.Projects.Contracts;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class ProjectTimeRepository(RaccordDbContext db) : IProjectTimeStore
{
    public async Task<IReadOnlyList<ProjectTimeEntry>> ListAsync(
        Guid projectId,
        CancellationToken cancellationToken
    ) =>
        await db
            .ProjectTimeEntries.AsNoTracking()
            .Where(entry => entry.ProjectId == projectId)
            .OrderBy(entry => entry.RecordedAt)
            .ThenBy(entry => entry.Id)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    public void Add(ProjectTimeEntry entry) => db.ProjectTimeEntries.Add(entry);
}
