using Raccord.Application.Abstractions;
using Raccord.Modules.Projects.Contracts;

namespace Raccord.Infrastructure.Persistence;

internal sealed class ProjectsUnitOfWork(IUnitOfWork unitOfWork) : IProjectsUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        unitOfWork.SaveChangesAsync(cancellationToken);
}
