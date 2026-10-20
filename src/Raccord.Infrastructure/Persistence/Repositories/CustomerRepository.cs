using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Raccord.Modules.Contacts.Contracts;
using Raccord.SharedKernel.Parties;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class CustomerRepository(RaccordDbContext db)
    : IPartyDirectory,
        IPartyWriter,
        IContactDirectory,
        IContactPeopleDirectory,
        IContactOrganizationDirectory
{
    public async Task<bool> LockContactAsync(Guid contactId, CancellationToken cancellationToken) =>
        await db
            .Database.SqlQuery<Guid>(
                $"SELECT id AS \"Value\" FROM contacts.customers WHERE id = {contactId} FOR UPDATE"
            )
            .SingleOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false) != Guid.Empty;

    public async Task<IReadOnlyList<ContactPersonSnapshot>> ListAsync(
        Guid contactId,
        CancellationToken cancellationToken
    ) =>
        [
            .. (
                await db
                    .ContactPersons.AsNoTracking()
                    .Where(person => person.ContactId == contactId)
                    .OrderBy(person => person.Name)
                    .ThenBy(person => person.Id)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false)
            ).Select(ToContactPersonSnapshot),
        ];

    public async Task<ContactPersonSnapshot?> FindAsync(
        Guid contactId,
        Guid personId,
        CancellationToken cancellationToken
    )
    {
        var person = await db
            .ContactPersons.AsNoTracking()
            .FirstOrDefaultAsync(
                candidate => candidate.ContactId == contactId && candidate.Id == personId,
                cancellationToken
            )
            .ConfigureAwait(false);
        return person is null ? null : ToContactPersonSnapshot(person);
    }

    public Task AddAsync(ContactPersonSnapshot person, CancellationToken cancellationToken)
    {
        db.ContactPersons.Add(
            new ContactPersonRow
            {
                Id = person.Id,
                ContactId = person.ContactId,
                Name = person.Name,
                Email = person.Email,
                Phone = person.Phone,
                IsDefault = person.IsDefault,
                IsAdministrative = person.IsAdministrative,
            }
        );
        return Task.CompletedTask;
    }

    public async Task UpdateAsync(ContactPersonSnapshot person, CancellationToken cancellationToken)
    {
        var row = await db
            .ContactPersons.FirstOrDefaultAsync(
                candidate => candidate.ContactId == person.ContactId && candidate.Id == person.Id,
                cancellationToken
            )
            .ConfigureAwait(false);
        if (row is null)
        {
            return;
        }

        row.Name = person.Name;
        row.Email = person.Email;
        row.Phone = person.Phone;
        row.IsDefault = person.IsDefault;
        row.IsAdministrative = person.IsAdministrative;
    }

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

    public Task<bool> ContactNumberExistsAsync(
        string contactNumber,
        CancellationToken cancellationToken
    ) =>
        db.Customers.AnyAsync(
            customer => customer.ContactNumber == contactNumber,
            cancellationToken
        );

    public async Task<ContactPage> ListContactsAsync(
        ContactDirectoryFilter filter,
        CancellationToken cancellationToken
    )
    {
        var contacts = db.Customers.Include(customer => customer.Roles).AsQueryable();
        if (filter.Role is not null)
        {
            contacts = contacts.Where(customer =>
                customer.Roles.Any(contactRole => contactRole.Role == filter.Role)
            );
        }

        if (filter.FolderId is not null)
        {
            contacts = contacts.Where(customer => customer.FolderId == filter.FolderId);
        }

        if (filter.TagIds.Count > 0)
        {
            contacts = filter.RequireAllTags
                ? contacts.Where(customer =>
                    db.ContactTagAssignments.Count(assignment =>
                        assignment.ContactId == customer.Id
                        && filter.TagIds.Contains(assignment.TagId)
                    ) == filter.TagIds.Count
                )
                : contacts.Where(customer =>
                    db.ContactTagAssignments.Any(assignment =>
                        assignment.ContactId == customer.Id
                        && filter.TagIds.Contains(assignment.TagId)
                    )
                );
        }

        var totalCount = await contacts.CountAsync(cancellationToken).ConfigureAwait(false);
        var rows = await contacts
            .OrderBy(customer => customer.ContactNumber)
            .ThenBy(customer => customer.Id)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return new ContactPage(
            rows.Select(row => ToContactSnapshot(row)!).ToArray(),
            filter.Page,
            filter.PageSize,
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

    public async Task<IReadOnlyList<ContactTagSnapshot>> ListTagsAsync(
        CancellationToken cancellationToken
    ) =>
        [
            .. (
                await db
                    .ContactTags.AsNoTracking()
                    .OrderBy(tag => tag.Name)
                    .ThenBy(tag => tag.Id)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false)
            ).Select(tag => new ContactTagSnapshot(tag.Id, tag.Name)),
        ];

    public async Task<ContactTagSnapshot?> CreateTagAsync(
        string name,
        CancellationToken cancellationToken
    )
    {
        var key = TagNameKey(name);
        if (
            await db
                .ContactTags.AnyAsync(tag => tag.NameKey == key, cancellationToken)
                .ConfigureAwait(false)
        )
        {
            return null;
        }

        var tag = new ContactTagRow
        {
            Id = Guid.CreateVersion7(),
            Name = name,
            NameKey = key,
        };
        db.ContactTags.Add(tag);
        return new ContactTagSnapshot(tag.Id, tag.Name);
    }

    public async Task<bool> DeleteTagAsync(Guid tagId, CancellationToken cancellationToken)
    {
        var tag = await db
            .ContactTags.Include(candidate => candidate.Assignments)
            .FirstOrDefaultAsync(candidate => candidate.Id == tagId, cancellationToken)
            .ConfigureAwait(false);
        if (tag is null || tag.Assignments.Count > 0)
        {
            return false;
        }

        db.ContactTags.Remove(tag);
        return true;
    }

    public async Task<IReadOnlyList<ContactFolderSnapshot>> ListFoldersAsync(
        CancellationToken cancellationToken
    ) =>
        [
            .. (
                await db
                    .ContactFolders.AsNoTracking()
                    .OrderBy(folder => folder.ParentId)
                    .ThenBy(folder => folder.Position)
                    .ThenBy(folder => folder.Name)
                    .ThenBy(folder => folder.Id)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false)
            ).Select(ToFolderSnapshot),
        ];

    public async Task<ContactFolderSnapshot?> CreateFolderAsync(
        string name,
        Guid? parentId,
        CancellationToken cancellationToken
    )
    {
        var parentExists =
            parentId is null
            || await db
                .ContactFolders.AnyAsync(folder => folder.Id == parentId, cancellationToken)
                .ConfigureAwait(false);
        if (!parentExists)
        {
            return null;
        }

        var position =
            await db
                .ContactFolders.Where(folder => folder.ParentId == parentId)
                .Select(folder => (int?)folder.Position)
                .MaxAsync(cancellationToken)
                .ConfigureAwait(false)
            ?? -1;
        var folder = new ContactFolderRow
        {
            Id = Guid.CreateVersion7(),
            Name = name,
            ParentId = parentId,
            Position = position + 1,
        };
        db.ContactFolders.Add(folder);
        return ToFolderSnapshot(folder);
    }

    public async Task<ContactFolderSnapshot?> UpdateFolderAsync(
        Guid folderId,
        string name,
        Guid? parentId,
        int position,
        CancellationToken cancellationToken
    )
    {
        var folder = await db
            .ContactFolders.FirstOrDefaultAsync(
                candidate => candidate.Id == folderId,
                cancellationToken
            )
            .ConfigureAwait(false);
        var parentExists =
            parentId is null
            || await db
                .ContactFolders.AnyAsync(candidate => candidate.Id == parentId, cancellationToken)
                .ConfigureAwait(false);
        var createsCycle =
            folder is not null
            && await WouldCreateFolderCycleAsync(folderId, parentId, cancellationToken)
                .ConfigureAwait(false);
        if (folder is null || position < 0 || !parentExists || createsCycle)
        {
            return null;
        }

        var previousParentId = folder.ParentId;
        var previousSiblings = await db
            .ContactFolders.Where(candidate => candidate.ParentId == previousParentId)
            .Where(candidate => candidate.Id != folderId)
            .OrderBy(candidate => candidate.Position)
            .ThenBy(candidate => candidate.Name)
            .ThenBy(candidate => candidate.Id)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        for (var index = 0; index < previousSiblings.Count; index++)
        {
            previousSiblings[index].Position = index;
        }

        var targetSiblings =
            previousParentId == parentId
                ? previousSiblings
                : await db
                    .ContactFolders.Where(candidate => candidate.ParentId == parentId)
                    .Where(candidate => candidate.Id != folderId)
                    .OrderBy(candidate => candidate.Position)
                    .ThenBy(candidate => candidate.Name)
                    .ThenBy(candidate => candidate.Id)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);
        var insertionIndex = Math.Min(position, targetSiblings.Count);
        targetSiblings.Insert(insertionIndex, folder);
        for (var index = 0; index < targetSiblings.Count; index++)
        {
            targetSiblings[index].Position = index;
        }

        folder.Name = name;
        folder.ParentId = parentId;
        return ToFolderSnapshot(folder);
    }

    public async Task<bool> DeleteFolderAsync(Guid folderId, CancellationToken cancellationToken)
    {
        var folder = await db
            .ContactFolders.FirstOrDefaultAsync(
                candidate => candidate.Id == folderId,
                cancellationToken
            )
            .ConfigureAwait(false);
        var containsContacts = await db
            .Customers.AnyAsync(customer => customer.FolderId == folderId, cancellationToken)
            .ConfigureAwait(false);
        var containsFolders = await db
            .ContactFolders.AnyAsync(child => child.ParentId == folderId, cancellationToken)
            .ConfigureAwait(false);
        if (folder is null || containsContacts || containsFolders)
        {
            return false;
        }

        db.ContactFolders.Remove(folder);
        return true;
    }

    public async Task<ContactOrganizationSnapshot?> GetContactOrganizationAsync(
        Guid contactId,
        CancellationToken cancellationToken
    )
    {
        var contact = await db
            .Customers.AsNoTracking()
            .Where(customer => customer.Id == contactId)
            .Select(customer => new { customer.Id, customer.FolderId })
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
        if (contact is null)
        {
            return null;
        }

        var tags = await db
            .ContactTagAssignments.AsNoTracking()
            .Where(assignment => assignment.ContactId == contactId)
            .OrderBy(assignment => assignment.Tag.Name)
            .Select(assignment => new ContactTagSnapshot(assignment.Tag.Id, assignment.Tag.Name))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return new ContactOrganizationSnapshot(contact.Id, contact.FolderId, tags);
    }

    public async Task<bool> SetContactTagAsync(
        Guid contactId,
        Guid tagId,
        bool assigned,
        CancellationToken cancellationToken
    )
    {
        var contactExists = await db
            .Customers.AnyAsync(contact => contact.Id == contactId, cancellationToken)
            .ConfigureAwait(false);
        var tagExists = await db
            .ContactTags.AnyAsync(tag => tag.Id == tagId, cancellationToken)
            .ConfigureAwait(false);
        if (!contactExists || !tagExists)
        {
            return false;
        }

        var assignment = await db
            .ContactTagAssignments.FirstOrDefaultAsync(
                candidate => candidate.ContactId == contactId && candidate.TagId == tagId,
                cancellationToken
            )
            .ConfigureAwait(false);
        if (assigned && assignment is null)
        {
            db.ContactTagAssignments.Add(
                new ContactTagAssignmentRow { ContactId = contactId, TagId = tagId }
            );
        }
        else if (!assigned && assignment is not null)
        {
            db.ContactTagAssignments.Remove(assignment);
        }

        return true;
    }

    public async Task<bool> SetContactFolderAsync(
        Guid contactId,
        Guid? folderId,
        CancellationToken cancellationToken
    )
    {
        var contact = await db
            .Customers.FirstOrDefaultAsync(
                candidate => candidate.Id == contactId,
                cancellationToken
            )
            .ConfigureAwait(false);
        if (contact is null)
        {
            return false;
        }

        var folderExists =
            folderId is null
            || await db
                .ContactFolders.AnyAsync(folder => folder.Id == folderId, cancellationToken)
                .ConfigureAwait(false);
        if (!folderExists)
        {
            return false;
        }

        contact.FolderId = folderId;
        return true;
    }

    private async Task<bool> WouldCreateFolderCycleAsync(
        Guid folderId,
        Guid? parentId,
        CancellationToken cancellationToken
    )
    {
        var ancestors = await db
            .ContactFolders.AsNoTracking()
            .ToDictionaryAsync(folder => folder.Id, folder => folder.ParentId, cancellationToken)
            .ConfigureAwait(false);
        var current = parentId;
        while (current is not null)
        {
            if (current == folderId)
            {
                return true;
            }

            if (!ancestors.TryGetValue(current.Value, out current))
            {
                return true;
            }
        }

        return false;
    }

    private static string TagNameKey(string name) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(name.ToUpperInvariant())));

    private static ContactFolderSnapshot ToFolderSnapshot(ContactFolderRow folder) =>
        new(folder.Id, folder.Name, folder.ParentId, folder.Position);

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

    private static ContactPersonSnapshot ToContactPersonSnapshot(ContactPersonRow person) =>
        new(
            person.Id,
            person.ContactId,
            person.Name,
            person.Email,
            person.Phone,
            person.IsDefault,
            person.IsAdministrative
        );
}
