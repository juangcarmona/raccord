using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Raccord.Infrastructure.Persistence;

namespace Raccord.Infrastructure.Compliance;

internal sealed partial class ComplianceSubmissionWorker(
    IServiceScopeFactory scopes,
    IOptions<ComplianceOptions> options,
    ILogger<ComplianceSubmissionWorker> logger
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!options.Value.Enabled)
            return;

        using var timer = new PeriodicTimer(
            TimeSpan.FromSeconds(Math.Max(1, options.Value.PollSeconds))
        );
        do
        {
            await SubmitPendingAsync(stoppingToken).ConfigureAwait(false);
        } while (await timer.WaitForNextTickAsync(stoppingToken).ConfigureAwait(false));
    }

    private async Task SubmitPendingAsync(CancellationToken cancellationToken)
    {
        using var scope = scopes.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<RaccordDbContext>();
        var authority = scope.ServiceProvider.GetRequiredService<IComplianceAuthorityClient>();
        var pending = await db
            .ComplianceRecords.Where(record =>
                record.Status == PendingComplianceRecorder.PendingStatus
            )
            .OrderBy(record => record.CreatedAt)
            .Take(20)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        foreach (var record in pending)
        {
            try
            {
                var invoice = await db
                    .IssuedInvoices.AsNoTracking()
                    .SingleAsync(row => row.Id == record.InvoiceId, cancellationToken)
                    .ConfigureAwait(false);
                var result = await authority
                    .SubmitAsync(
                        record.RecordHash,
                        record.InvoiceNumber,
                        invoice.Representation,
                        cancellationToken
                    )
                    .ConfigureAwait(false);
                record.Attempts++;
                record.LastAttemptAt = DateTimeOffset.UtcNow;
                record.Status = result.Accepted ? "accepted" : "rejected";
                record.AuthorityReference = result.Reference;
                record.LastError = result.Error;
                await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception)
                when (exception is HttpRequestException or InvalidOperationException)
            {
                record.Attempts++;
                record.LastAttemptAt = DateTimeOffset.UtcNow;
                record.LastError = exception.Message;
                await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                LogSubmissionFailure(logger, exception, record.Id);
            }
        }
    }

    [LoggerMessage(
        EventId = 6501,
        Level = LogLevel.Warning,
        Message = "Compliance submission attempt failed for record {RecordId}."
    )]
    private static partial void LogSubmissionFailure(
        ILogger logger,
        Exception exception,
        Guid recordId
    );
}
