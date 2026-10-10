using Raccord.Domain.Customers;

namespace Raccord.Application.Customers;

/// <summary>Persistence of customers.</summary>
public interface ICustomerRepository
{
    /// <summary>Finds a customer.</summary>
    Task<Customer?> FindAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>Lists customers by legal name.</summary>
    Task<IReadOnlyList<Customer>> ListAsync(CancellationToken cancellationToken);

    /// <summary>Adds a new customer.</summary>
    void Add(Customer customer);
}
