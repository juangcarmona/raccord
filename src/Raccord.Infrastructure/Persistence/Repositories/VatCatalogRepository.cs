using Microsoft.EntityFrameworkCore;
using Raccord.Application.Invoicing;
using Raccord.Domain.Billing;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class VatCatalogRepository(RaccordDbContext db) : IVatCatalogRepository
{
    public async Task<IReadOnlyList<VatClass>> ListClassesAsync(
        CancellationToken cancellationToken
    ) =>
        await db
            .VatClasses.AsNoTracking()
            .OrderBy(item => item.Name)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    public Task<VatClass?> FindClassAsync(Guid id, CancellationToken cancellationToken) =>
        db.VatClasses.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

    public void Add(VatClass vatClass) => db.VatClasses.Add(vatClass);

    public async Task<IReadOnlyList<VatScheme>> ListSchemesAsync(
        CancellationToken cancellationToken
    ) =>
        await db
            .VatSchemes.Include(item => item.ClassRates)
            .OrderByDescending(item => item.IsDefault)
            .ThenBy(item => item.Name)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    public Task<VatScheme?> FindSchemeAsync(Guid id, CancellationToken cancellationToken) =>
        db
            .VatSchemes.Include(item => item.ClassRates)
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

    public void Add(VatScheme scheme) => db.VatSchemes.Add(scheme);
}
