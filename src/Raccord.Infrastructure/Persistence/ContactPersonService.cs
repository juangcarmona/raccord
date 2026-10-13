#pragma warning disable CS1591

using Microsoft.EntityFrameworkCore;
using Raccord.Contracts.Customers;

namespace Raccord.Infrastructure.Persistence;

public sealed class ContactPersonService(RaccordDbContext db)
{
    public async Task<IReadOnlyList<ContactPersonResponse>> ListAsync(
        Guid customerId,
        CancellationToken cancellationToken
    ) =>
        await db
            .ContactPersons.Where(person => person.CustomerId == customerId)
            .OrderBy(person => person.Name)
            .Select(person => new ContactPersonResponse(
                person.Id,
                person.CustomerId,
                person.Name,
                person.Role,
                person.Email,
                person.Phone,
                person.IsDefault
            ))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    public async Task<ContactPersonResponse?> AddAsync(
        Guid customerId,
        CreateContactPersonRequest request,
        CancellationToken cancellationToken
    )
    {
        if (
            !await db
                .Customers.AnyAsync(customer => customer.Id == customerId, cancellationToken)
                .ConfigureAwait(false)
        )
            return null;
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Role))
            throw new ArgumentException("Name and role are required.", nameof(request));
        if (request.IsDefault)
        {
            await db
                .ContactPersons.Where(person => person.CustomerId == customerId && person.IsDefault)
                .ExecuteUpdateAsync(
                    update => update.SetProperty(person => person.IsDefault, false),
                    cancellationToken
                )
                .ConfigureAwait(false);
        }
        var person = new ContactPersonRow
        {
            Id = Guid.CreateVersion7(),
            CustomerId = customerId,
            Name = request.Name.Trim(),
            Role = request.Role.Trim(),
            Email = request.Email?.Trim(),
            Phone = request.Phone?.Trim(),
            IsDefault = request.IsDefault,
        };
        db.ContactPersons.Add(person);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return new ContactPersonResponse(
            person.Id,
            person.CustomerId,
            person.Name,
            person.Role,
            person.Email,
            person.Phone,
            person.IsDefault
        );
    }
}

#pragma warning restore CS1591
