using Raccord.Domain.Projects;

namespace Raccord.Application.Projects;

/// <summary>Persists reusable project templates.</summary>
public interface IProjectTemplateRepository
{
    /// <summary>Finds a template by identifier.</summary>
    Task<ProjectTemplate?> FindAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>Adds a template to the unit of work.</summary>
    void Add(ProjectTemplate projectTemplate);
}
