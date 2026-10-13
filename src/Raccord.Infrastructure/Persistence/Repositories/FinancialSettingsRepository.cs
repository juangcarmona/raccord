using Microsoft.EntityFrameworkCore;
using Raccord.Application.Organization;
using Raccord.Domain.Organization;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class FinancialSettingsRepository(RaccordDbContext db)
    : IFinancialSettingsRepository
{
    public Task<FinancialSettings?> FindAsync(CancellationToken cancellationToken) =>
        db
            .FinancialSettings.OrderBy(settings => settings.Id)
            .FirstOrDefaultAsync(cancellationToken);

    public void Add(FinancialSettings settings) => db.FinancialSettings.Add(settings);
}
