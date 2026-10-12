using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Raccord.BuildingBlocks.Persistence;

/// <summary>EF transaction coordinator using one externally owned Npgsql connection.</summary>
public sealed class SharedConnectionUnitOfWork(
    NpgsqlConnection connection,
    ParticipatingContexts participants
) : IUnitOfWork
{
    private bool executing;

    /// <inheritdoc />
    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        ExecuteAsync(_ => Task.FromResult(true), cancellationToken);

    /// <inheritdoc />
    public async Task<T> ExecuteAsync<T>(
        Func<CancellationToken, Task<T>> callback,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(callback);
        if (executing)
        {
            throw new InvalidOperationException(
                "Nested shared unit-of-work execution is not supported."
            );
        }

        var openedHere = connection.State != System.Data.ConnectionState.Open;
        executing = true;
#pragma warning disable MA0004
        await using var transaction = await OpenTransactionAsync(cancellationToken)
            .ConfigureAwait(false);
#pragma warning restore MA0004
        try
        {
            await EnlistAsync(transaction, cancellationToken).ConfigureAwait(false);
            var result = await callback(cancellationToken).ConfigureAwait(false);
            await EnlistAsync(transaction, cancellationToken).ConfigureAwait(false);
            foreach (var context in participants.Items)
            {
                await SaveContextAsync(context, cancellationToken).ConfigureAwait(false);
            }

            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            return result;
        }
        catch
        {
            try
            {
                await transaction.RollbackAsync(CancellationToken.None).ConfigureAwait(false);
            }
            finally
            {
                ClearTrackedState();
            }

            throw;
        }
        finally
        {
            await DetachAsync().ConfigureAwait(false);

            executing = false;
            if (openedHere)
            {
                await connection.CloseAsync().ConfigureAwait(false);
            }
        }
    }

    private async Task DetachAsync()
    {
        foreach (var context in participants.Items)
        {
            await context
                .Database.UseTransactionAsync(
                    (System.Data.Common.DbTransaction?)null,
                    CancellationToken.None
                )
                .ConfigureAwait(false);
        }
    }

    private void ClearTrackedState()
    {
        foreach (var context in participants.Items)
        {
            context.ChangeTracker.Clear();
        }
    }

    private async Task<NpgsqlTransaction> OpenTransactionAsync(CancellationToken cancellationToken)
    {
        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        }

        if (connection.FullState != System.Data.ConnectionState.Open)
        {
            throw new InvalidOperationException(
                "The shared connection must be open without an external transaction."
            );
        }

        return await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
    }

    private async Task EnlistAsync(
        NpgsqlTransaction transaction,
        CancellationToken cancellationToken
    )
    {
        foreach (var context in participants.Items)
        {
            if (!ReferenceEquals(context.Database.GetDbConnection(), connection))
            {
                throw new InvalidOperationException(
                    "Every participating context must use the shared connection object."
                );
            }

            await context
                .Database.UseTransactionAsync(transaction, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    private static async Task SaveContextAsync(
        DbContext context,
        CancellationToken cancellationToken
    )
    {
        try
        {
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (DbUpdateConcurrencyException exception)
        {
            throw new PersistenceConflictException(
                "A concurrent persistence conflict occurred.",
                exception
            );
        }
        catch (DbUpdateException exception)
            when (exception.InnerException
                    is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation }
            )
        {
            throw new PersistenceConflictException(
                "A unique persistence conflict occurred.",
                exception
            );
        }
    }
}
