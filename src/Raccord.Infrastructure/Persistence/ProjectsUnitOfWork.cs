using Raccord.Application.Abstractions;
using Raccord.Modules.Projects.Contracts;

namespace Raccord.Infrastructure.Persistence;

internal sealed class ProjectsUnitOfWork(
    IUnitOfWork unitOfWork,
    Raccord.BuildingBlocks.Persistence.IUnitOfWork sharedUnitOfWork
) : IProjectsUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        unitOfWork.SaveChangesAsync(cancellationToken);

    public Task<T> ExecuteAsync<T>(
        Func<CancellationToken, Task<T>> callback,
        CancellationToken cancellationToken
    ) => sharedUnitOfWork.ExecuteAsync(callback, cancellationToken);
}
