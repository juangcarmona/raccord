using Microsoft.EntityFrameworkCore;
using Raccord.Application.Organization;
using Raccord.Application.Projects;
using Raccord.Domain.Organization;
using Raccord.Domain.Projects;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class IssuerProfileRepository(RaccordDbContext db) : IIssuerProfileRepository
{
    public Task<IssuerProfile?> FindAsync(CancellationToken cancellationToken) =>
        db.IssuerProfiles.OrderBy(profile => profile.Id).FirstOrDefaultAsync(cancellationToken);

    public void Add(IssuerProfile profile) => db.IssuerProfiles.Add(profile);
}
