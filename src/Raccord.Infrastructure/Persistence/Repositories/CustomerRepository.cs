using System.Text.Json;
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
            Type = party.Type,
            Email = party.Email,
            DefaultDiscountPercent = party.DefaultDiscountPercent,
            DefaultPaymentTerms = party.DefaultPaymentTerms,
        };
        row.BillingAddress.Set(party.Address);
        db.Customers.Add(row);
    }

    public void Update(PartySnapshot party)
    {
        var row = db.Customers.Local.FirstOrDefault(customer => customer.Id == party.Id);
        if (row is null)
        {
            row = db.Customers.First(customer => customer.Id == party.Id);
        }

        row.LegalName = party.LegalName;
        row.Type = party.Type;
        row.TaxId = party.TaxId;
        row.Email = party.Email;
        row.DefaultDiscountPercent = party.DefaultDiscountPercent;
        row.DefaultPaymentTerms = party.DefaultPaymentTerms;
        row.BillingAddress.Set(party.Address);
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
            customer.Type,
            customer.DefaultDiscountPercent,
            customer.DefaultPaymentTerms,
            ParseTags(customer.Tags),
            customer.Folder
        );
    }

    private static string[] ParseTags(string? tags) =>
        string.IsNullOrWhiteSpace(tags) ? [] : JsonSerializer.Deserialize<string[]>(tags) ?? [];
}
