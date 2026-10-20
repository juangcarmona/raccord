using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Raccord.Modules.Contacts.Contracts;
using Raccord.SharedKernel.Parties;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class CustomerRepository(RaccordDbContext db)
    : IPartyDirectory,
        IPartyWriter,
        IContactDirectory
{
    public async Task<PartySnapshot?> FindAsync(Guid id, CancellationToken cancellationToken) =>
        ToPartySnapshot(
            await db
                .Customers.Include(customer => customer.Roles)
                .FirstOrDefaultAsync(
                    customer =>
                        customer.Id == id
                        && customer.Roles.Any(role => role.Role == ContactRole.Customer),
                    cancellationToken
                )
                .ConfigureAwait(false)
        );

    public async Task<IReadOnlyList<PartySnapshot>> ListAsync(
        CancellationToken cancellationToken
    ) =>
        [
            .. (
                await db
                    .Customers.Include(customer => customer.Roles)
                    .Where(customer =>
                        customer.Roles.Any(role => role.Role == ContactRole.Customer)
                    )
                    .OrderBy(customer => customer.LegalName)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false)
            ).Select(customer => ToPartySnapshot(customer)!),
        ];

    public async Task AddAsync(PartySnapshot party, CancellationToken cancellationToken)
    {
        var row = new CustomerRow
        {
            Id = party.Id,
            LegalName = party.LegalName,
            TaxId = party.TaxId,
            Email = party.Email,
            Type = ContactType.Company,
            ContactNumber = await NextContactNumberAsync(cancellationToken).ConfigureAwait(false),
            Roles = [new ContactRoleRow { Role = ContactRole.Customer }],
        };
        row.BillingAddress.Set(party.Address);
        db.Customers.Add(row);
    }

    public async Task<ContactSnapshot?> FindContactAsync(
        Guid id,
        CancellationToken cancellationToken
    ) =>
        ToContactSnapshot(
            await db
                .Customers.Include(customer => customer.Roles)
                .FirstOrDefaultAsync(customer => customer.Id == id, cancellationToken)
                .ConfigureAwait(false)
        );

    public async Task<ContactPage> ListContactsAsync(
        int page,
        int pageSize,
        ContactRole? role,
        CancellationToken cancellationToken
    )
    {
        var contacts = db.Customers.Include(customer => customer.Roles).AsQueryable();
        if (role is not null)
        {
            contacts = contacts.Where(customer =>
                customer.Roles.Any(contactRole => contactRole.Role == role)
            );
        }

        var totalCount = await contacts.CountAsync(cancellationToken).ConfigureAwait(false);
        var rows = await contacts
            .OrderBy(customer => customer.ContactNumber)
            .ThenBy(customer => customer.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return new ContactPage(
            rows.Select(row => ToContactSnapshot(row)!).ToArray(),
            page,
            pageSize,
            totalCount
        );
    }

    public async Task AddContactAsync(ContactSnapshot contact, CancellationToken cancellationToken)
    {
        var row = new CustomerRow
        {
            Id = contact.Id,
            Type = contact.Type,
            ContactNumber = string.IsNullOrWhiteSpace(contact.ContactNumber)
                ? await NextContactNumberAsync(cancellationToken).ConfigureAwait(false)
                : contact.ContactNumber.Trim(),
            LegalName = contact.LegalName,
            TaxId = contact.TaxId,
            Email = contact.Email,
            Roles = [.. contact.Roles.Select(role => new ContactRoleRow { Role = role })],
        };
        row.BillingAddress.Set(contact.Address);
        db.Customers.Add(row);
    }

    public async Task UpdateContactAsync(
        ContactSnapshot contact,
        CancellationToken cancellationToken
    )
    {
        var row = await db
            .Customers.Include(customer => customer.Roles)
            .FirstOrDefaultAsync(customer => customer.Id == contact.Id, cancellationToken)
            .ConfigureAwait(false);
        if (row is null)
        {
            return;
        }

        row.Type = contact.Type;
        row.ContactNumber = string.IsNullOrWhiteSpace(contact.ContactNumber)
            ? await NextContactNumberAsync(cancellationToken).ConfigureAwait(false)
            : contact.ContactNumber.Trim();
        row.LegalName = contact.LegalName;
        row.TaxId = contact.TaxId;
        row.Email = contact.Email;
        row.BillingAddress.Set(contact.Address);
        row.Roles.RemoveAll(role => !contact.Roles.Contains(role.Role));
        foreach (var role in contact.Roles.Where(role => row.Roles.All(item => item.Role != role)))
        {
            row.Roles.Add(new ContactRoleRow { ContactId = row.Id, Role = role });
        }
    }

    private async Task<string> NextContactNumberAsync(CancellationToken cancellationToken)
    {
        while (true)
        {
            var value = await db
                .Database.SqlQueryRaw<long>(
                    "SELECT nextval('contacts.contact_number_seq') AS \"Value\""
                )
                .SingleAsync(cancellationToken)
                .ConfigureAwait(false);
            var candidate = value.ToString(CultureInfo.InvariantCulture);
            if (
                !await db
                    .Customers.AnyAsync(
                        customer => customer.ContactNumber == candidate,
                        cancellationToken
                    )
                    .ConfigureAwait(false)
            )
            {
                return candidate;
            }
        }
    }

    private static PartySnapshot? ToPartySnapshot(CustomerRow? customer)
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
            customer.Email
        );
    }

    private static ContactSnapshot? ToContactSnapshot(CustomerRow? customer) =>
        customer is null
            ? null
            : new ContactSnapshot(
                customer.Id,
                customer.Type,
                customer.ContactNumber,
                customer.LegalName,
                customer.TaxId,
                customer.BillingAddress.ToAddress(),
                customer.Email,
                customer.Roles.Select(role => role.Role).ToHashSet()
            );
}
