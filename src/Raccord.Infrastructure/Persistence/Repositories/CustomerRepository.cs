using Microsoft.EntityFrameworkCore;
using Raccord.Application.Customers;
using Raccord.Application.Organization;
using Raccord.Application.Projects;
using Raccord.Domain.Customers;
using Raccord.Domain.Organization;
using Raccord.Domain.Projects;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class CustomerRepository(RaccordDbContext db) : ICustomerRepository
{
    public Task<Customer?> FindAsync(Guid id, CancellationToken cancellationToken) =>
        db.Customers.FirstOrDefaultAsync(customer => customer.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Customer>> ListAsync(CancellationToken cancellationToken) =>
        await db
            .Customers.OrderBy(customer => customer.LegalName)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    public void Add(Customer customer) => db.Customers.Add(customer);
}
