using Microsoft.EntityFrameworkCore;
using Raccord.Modules.Contacts.Contracts;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class CustomerRepository(RaccordDbContext db) : IPartyDirectory, IPartyWriter
{
    public async Task<PartySnapshot?> FindAsync(Guid id, CancellationToken cancellationToken) =>
        ToSnapshot(
            await db
                .Customers.FirstOrDefaultAsync(customer => customer.Id == id, cancellationToken)
                .ConfigureAwait(false)
        );

    public async Task<IReadOnlyList<PartySnapshot>> ListAsync(
        CancellationToken cancellationToken
    ) =>
        [
            .. (
                await db
                    .Customers.OrderBy(customer => customer.LegalName)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false)
            ).Select(customer => ToSnapshot(customer)!),
        ];

    public void Add(PartySnapshot party)
    {
        var row = new CustomerRow
        {
            Id = party.Id,
            LegalName = party.LegalName,
            TaxId = party.TaxId,
            Email = party.Email,
            DefaultDiscountPercent = party.DefaultDiscountPercent,
            DefaultPaymentTerms = party.DefaultPaymentTerms,
        };
        row.BillingAddress.Set(party.Address);
        db.Customers.Add(row);
    }

    private static PartySnapshot? ToSnapshot(CustomerRow? customer)
    {
        if (customer is null)
        {
            return null;
        }

        return new PartySnapshot(
            customer.Id,
            customer.LegalName,
            customer.TaxId,
            customer.BillingAddress.ToAddress(),
            customer.Email,
            customer.DefaultDiscountPercent,
            customer.DefaultPaymentTerms
        );
    }
}
