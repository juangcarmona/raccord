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
                issuer.Address.ToAddress()!,
                issuer.IssuedInvoiceCount
            );
    }

    public void AddIssuer(IssuerProfileSnapshot issuer)
    {
        var row = new IssuerProfileRow
        {
            Id = issuer.Id,
            LegalName = issuer.LegalName,
            TaxId = issuer.TaxId,
            IssuedInvoiceCount = issuer.IssuedInvoiceCount,
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
        row.IssuedInvoiceCount = issuer.IssuedInvoiceCount;
        row.Address.Set(issuer.Address);
    }

    public void MarkInvoiceIssued()
    {
        var issuer = db.IssuerProfiles.OrderBy(profile => profile.Id).FirstOrDefault();
        if (issuer is null)
        {
            throw new InvalidOperationException(
                "An issuer profile is required before issuing an invoice."
            );
        }
        issuer.IssuedInvoiceCount++;
    }

    public async Task<IReadOnlyList<IssuerIdentityChangeSnapshot>> ListIssuerIdentityChangesAsync(
        CancellationToken cancellationToken
    ) =>
        await db
            .IssuerIdentityChanges.AsNoTracking()
            .OrderByDescending(request => request.RequestedAt)
            .Select(request => new IssuerIdentityChangeSnapshot(
                request.Id,
                request.IssuerId,
                request.OldLegalName,
                request.OldTaxId,
                request.ProposedLegalName,
                request.ProposedTaxId,
                request.Reason,
                request.RequestedById,
                request.RequestedByName,
                request.RequestedAt,
                request.Status,
                request.DecidedById,
                request.DecidedByName,
                request.DecidedAt
            ))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    public async Task<IssuerIdentityChangeSnapshot?> FindIssuerIdentityChangeAsync(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        var request = await db
            .IssuerIdentityChanges.FirstOrDefaultAsync(item => item.Id == id, cancellationToken)
            .ConfigureAwait(false);
        return request is null ? null : ToSnapshot(request);
    }

    public void AddIssuerIdentityChange(IssuerIdentityChangeSnapshot request) =>
        db.IssuerIdentityChanges.Add(ToRow(request));

    public void UpdateIssuerIdentityChange(IssuerIdentityChangeSnapshot request)
    {
        var row = db.IssuerIdentityChanges.Local.FirstOrDefault(item => item.Id == request.Id);
        if (row is null)
        {
            throw new InvalidOperationException(
                "The issuer identity request was not loaded for update."
            );
        }
        row.Status = request.Status;
        row.DecidedById = request.DecidedById;
        row.DecidedByName = request.DecidedByName;
        row.DecidedAt = request.DecidedAt;
    }

    private static IssuerIdentityChangeRow ToRow(IssuerIdentityChangeSnapshot request) =>
        new()
        {
            Id = request.Id,
            IssuerId = request.IssuerId,
            OldLegalName = request.OldLegalName,
            OldTaxId = request.OldTaxId,
            ProposedLegalName = request.ProposedLegalName,
            ProposedTaxId = request.ProposedTaxId,
            Reason = request.Reason,
            RequestedById = request.RequestedById,
            RequestedByName = request.RequestedByName,
            RequestedAt = request.RequestedAt,
            Status = request.Status,
            DecidedById = request.DecidedById,
            DecidedByName = request.DecidedByName,
            DecidedAt = request.DecidedAt,
        };

    private static IssuerIdentityChangeSnapshot ToSnapshot(IssuerIdentityChangeRow request) =>
        new(
            request.Id,
            request.IssuerId,
            request.OldLegalName,
            request.OldTaxId,
            request.ProposedLegalName,
            request.ProposedTaxId,
            request.Reason,
            request.RequestedById,
            request.RequestedByName,
            request.RequestedAt,
            request.Status,
            request.DecidedById,
            request.DecidedByName,
            request.DecidedAt
        );

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
