using Microsoft.EntityFrameworkCore;
using Npgsql;
using Raccord.Application.Abstractions;

namespace Raccord.Infrastructure.Persistence;

internal sealed class UnitOfWork(RaccordDbContext db) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (DbUpdateConcurrencyException exception)
        {
            throw new ConcurrencyConflictException("The records were changed by another operation. Reload and try again.", exception);
        }
        catch (DbUpdateException exception) when (exception.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            throw new ConcurrencyConflictException("Another operation created the same record first. Reload and try again.", exception);
        }
    }
}
