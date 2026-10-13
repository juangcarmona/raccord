using Microsoft.EntityFrameworkCore;
using Raccord.Application.Projects;
using Raccord.Domain.Projects;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class ProjectTemplateRepository(RaccordDbContext db) : IProjectTemplateRepository
{
    public Task<ProjectTemplate?> FindAsync(Guid id, CancellationToken cancellationToken) =>
        db.ProjectTemplates.FirstOrDefaultAsync(template => template.Id == id, cancellationToken);

    public void Add(ProjectTemplate projectTemplate) => db.ProjectTemplates.Add(projectTemplate);
}
