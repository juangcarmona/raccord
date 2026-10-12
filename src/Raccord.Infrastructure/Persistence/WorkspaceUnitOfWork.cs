using Raccord.Application.Abstractions;
using Raccord.Modules.Workspace.Contracts;

namespace Raccord.Infrastructure.Persistence;

internal sealed class WorkspaceUnitOfWork(IUnitOfWork unitOfWork) : IWorkspaceUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        unitOfWork.SaveChangesAsync(cancellationToken);
}
