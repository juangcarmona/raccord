using Raccord.Application.Abstractions;
using Raccord.Contracts.Customers;
using Raccord.Domain.Customers;

namespace Raccord.Application.Customers;

/// <summary>Use cases for registering and finding customers.</summary>
public sealed class CustomerDirectory(ICustomerRepository customers, IUnitOfWork unitOfWork)
{
    /// <summary>Registers a customer.</summary>
    public async Task<CustomerResponse> RegisterAsync(
        RegisterCustomerRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var customer = Customer.Register(
            request.LegalName ?? string.Empty,
            request.TaxId,
            request.Address.ToDomain(),
            request.Email
        );
        customers.Add(customer);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(customer);
    }

    /// <summary>Lists customers.</summary>
    public async Task<IReadOnlyList<CustomerResponse>> ListAsync(
        CancellationToken cancellationToken
    ) =>
        [
            .. (await customers.ListAsync(cancellationToken).ConfigureAwait(false)).Select(
                ToResponse
            ),
        ];

    /// <summary>Reads a customer.</summary>
    public async Task<CustomerResponse?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var customer = await customers.FindAsync(id, cancellationToken).ConfigureAwait(false);
        return customer is null ? null : ToResponse(customer);
    }

    private static CustomerResponse ToResponse(Customer customer)
    {
        var party = customer.ToLegalParty();
        var invoiceable = party.TaxId is not null && party.Address is { IsComplete: true };
        return new CustomerResponse(
            customer.Id,
            customer.LegalName,
            customer.TaxId,
            party.Address.ToDto(),
            customer.Email,
            invoiceable
        );
    }
}
