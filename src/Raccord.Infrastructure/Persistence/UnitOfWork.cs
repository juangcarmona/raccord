using Raccord.Application.Abstractions;
using Raccord.BuildingBlocks.Persistence;
using ApplicationUnitOfWork = Raccord.Application.Abstractions.IUnitOfWork;

namespace Raccord.Infrastructure.Persistence;

internal sealed class UnitOfWork(
    RaccordDbContext db,
    SharedConnectionUnitOfWork sharedUnitOfWork,
    ParticipatingContexts participants
) : ApplicationUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        participants.Register(db);
        try
        {
            await sharedUnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (PersistenceConflictException exception)
        {
            throw new ConcurrencyConflictException(
                "Another operation created the same record first. Reload and try again.",
                exception
            );
        }
    }
}
