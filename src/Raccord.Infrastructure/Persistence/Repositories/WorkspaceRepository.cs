using Microsoft.EntityFrameworkCore;
using Raccord.Application.Invoicing;
using Raccord.Domain.Billing;
using Raccord.Modules.Workspace.Contracts;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class WorkspaceRepository(RaccordDbContext db)
    : IWorkspaceStore,
        IInvoiceNumberSequenceRepository
{
    public async Task<IReadOnlyList<InvoiceSeriesSnapshot>> ListAsync(
        CancellationToken cancellationToken
    ) =>
        [
            .. (
                await db
                    .InvoiceSeries.OrderBy(series => series.Code)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false)
            ).Select(series => new InvoiceSeriesSnapshot(
                series.Code,
                series.Description,
                series.IsDefault
            )),
        ];

    public async Task<InvoiceSeriesSnapshot?> FindAsync(
        string code,
        CancellationToken cancellationToken
    )
    {
        var series = await db
            .InvoiceSeries.FirstOrDefaultAsync(item => item.Code == code, cancellationToken)
            .ConfigureAwait(false);
        return series is null
            ? null
            : new InvoiceSeriesSnapshot(series.Code, series.Description, series.IsDefault);
    }

    public async Task<IssuerProfileSnapshot?> FindIssuerAsync(CancellationToken cancellationToken)
    {
        var issuer = await db
            .IssuerProfiles.OrderBy(profile => profile.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
        return issuer is null
            ? null
            : new IssuerProfileSnapshot(
                issuer.Id,
                issuer.LegalName,
                issuer.TaxId,
                issuer.Address.ToAddress()!
            );
    }

    public void AddIssuer(IssuerProfileSnapshot issuer)
    {
        var row = new IssuerProfileRow
        {
            Id = issuer.Id,
            LegalName = issuer.LegalName,
            TaxId = issuer.TaxId,
        };
        row.Address.Set(issuer.Address);
        db.IssuerProfiles.Add(row);
    }

    public void UpdateIssuer(IssuerProfileSnapshot issuer)
    {
        var row = db.IssuerProfiles.Local.FirstOrDefault(item => item.Id == issuer.Id);
        if (row is null)
        {
            throw new InvalidOperationException("The issuer profile was not loaded for update.");
        }

        row.LegalName = issuer.LegalName;
        row.TaxId = issuer.TaxId;
        row.Address.Set(issuer.Address);
    }

    public void AddSeries(InvoiceSeriesSnapshot series) =>
        db.InvoiceSeries.Add(
            new InvoiceSeriesRow
            {
                Code = series.Code,
                Description = series.Description,
                IsDefault = series.IsDefault,
            }
        );

    public Task<InvoiceNumberSequence?> FindSequenceAsync(
        string seriesCode,
        int year,
        CancellationToken cancellationToken
    ) =>
        db
            .InvoiceNumberSequences.AsNoTracking()
            .FirstOrDefaultAsync(
                sequence => sequence.SeriesCode == seriesCode && sequence.Year == year,
                cancellationToken
            );

    public async Task<InvoiceNumberSequence> GetOrStartSequenceAsync(
        string seriesCode,
        int year,
        CancellationToken cancellationToken
    )
    {
        var existing = await db
            .InvoiceNumberSequences.FirstOrDefaultAsync(
                sequence => sequence.SeriesCode == seriesCode && sequence.Year == year,
                cancellationToken
            )
            .ConfigureAwait(false);
        if (existing is not null)
        {
            return existing;
        }

        var started = InvoiceNumberSequence.Start(seriesCode, year);
        db.InvoiceNumberSequences.Add(started);
        return started;
    }
}
