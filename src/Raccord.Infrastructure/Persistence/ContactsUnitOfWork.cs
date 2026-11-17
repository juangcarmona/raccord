using Microsoft.EntityFrameworkCore;
using Raccord.Application.Abstractions;
using Raccord.Modules.Contacts.Contracts;

namespace Raccord.Infrastructure.Persistence;

internal sealed class ContactsUnitOfWork(IUnitOfWork unitOfWork, RaccordDbContext db)
    : IContactsUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        unitOfWork.SaveChangesAsync(cancellationToken);

    public async Task<T> ExecuteAsync<T>(
        Func<CancellationToken, Task<T>> operation,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(operation);
        var strategy = db.Database.CreateExecutionStrategy();
        return await strategy
            .ExecuteAsync(async () =>
            {
                var transaction = await db
                    .Database.BeginTransactionAsync(cancellationToken)
                    .ConfigureAwait(false);
                try
                {
                    var result = await operation(cancellationToken).ConfigureAwait(false);
                    await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                    await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync(CancellationToken.None).ConfigureAwait(false);
                    throw;
                }
                finally
                {
                    await transaction.DisposeAsync().ConfigureAwait(false);
                }
            })
            .ConfigureAwait(false);
    }
}
