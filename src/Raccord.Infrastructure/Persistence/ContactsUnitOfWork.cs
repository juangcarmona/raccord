using Raccord.Application.Abstractions;
using Raccord.Modules.Contacts.Contracts;

namespace Raccord.Infrastructure.Persistence;

internal sealed class ContactsUnitOfWork(IUnitOfWork unitOfWork) : IContactsUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        unitOfWork.SaveChangesAsync(cancellationToken);
}
