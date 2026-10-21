using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Raccord.Modules.Projects.Contracts;

namespace Raccord.Infrastructure.Persistence;

internal sealed class ProjectNumberSeriesRepository(RaccordDbContext db) : IProjectNumberSeries
{
    private const long AdvisoryLockKey = 7_303_416;

    public async Task LockAsync(CancellationToken cancellationToken)
    {
        var transaction =
            db.Database.CurrentTransaction
            ?? throw new InvalidOperationException(
                "Project number assignment requires an active transaction."
            );
        using var command = db.Database.GetDbConnection().CreateCommand();
        command.Transaction = transaction.GetDbTransaction();
        command.CommandText = "SELECT pg_advisory_xact_lock(@lock_key)";
        var parameter = command.CreateParameter();
        parameter.ParameterName = "lock_key";
        parameter.Value = AdvisoryLockKey;
        command.Parameters.Add(parameter);
        await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<string> NextAsync(CancellationToken cancellationToken)
    {
        while (true)
        {
            var next = await db
                .Database.SqlQueryRaw<long>(
                    "SELECT nextval('projects.project_number_seq') AS \"Value\""
                )
                .SingleAsync(cancellationToken)
                .ConfigureAwait(false);
            var candidate = next.ToString(CultureInfo.InvariantCulture);
            if (!await IsAssignedAsync(candidate, null, cancellationToken).ConfigureAwait(false))
                return candidate;
        }
    }

    public Task<bool> IsAssignedAsync(
        string number,
        Guid? excludingProjectId,
        CancellationToken cancellationToken
    ) =>
        db.Projects.AnyAsync(
            project =>
                project.ProjectNumber == number
                && (!excludingProjectId.HasValue || project.Id != excludingProjectId.Value),
            cancellationToken
        );
}
