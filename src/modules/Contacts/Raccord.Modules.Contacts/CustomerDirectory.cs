using Raccord.Contracts.Common;
using Raccord.Contracts.Customers;
using Raccord.Modules.Contacts.Contracts;
using Raccord.Modules.Contacts.Domain;
using Raccord.SharedKernel.Parties;

namespace Raccord.Modules.Contacts;

/// <summary>Use cases for registering and finding customers.</summary>
public sealed class CustomerDirectory
{
    private readonly IPartyDirectory parties;
    private readonly IPartyWriter writer;
    private readonly IContactsUnitOfWork unitOfWork;

    /// <summary>Creates the customer use-case directory.</summary>
    public CustomerDirectory(
        IPartyDirectory parties,
        IPartyWriter writer,
        IContactsUnitOfWork unitOfWork
    )
    {
        this.parties = parties;
        this.writer = writer;
        this.unitOfWork = unitOfWork;
    }

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
            request.Address is null
                ? null
                : new PostalAddress(
                    request.Address.Street,
                    request.Address.PostalCode,
                    request.Address.City,
                    request.Address.Province,
                    request.Address.CountryCode
                ),
            request.Email
        );
        var party = new PartySnapshot(
            customer.Id,
            customer.LegalName,
            customer.TaxId,
            customer.BillingAddress.ToAddress(),
            customer.Email
        );
        await writer.AddAsync(party, cancellationToken).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(party);
    }

    /// <summary>Lists customers.</summary>
    public async Task<IReadOnlyList<CustomerResponse>> ListAsync(
        CancellationToken cancellationToken
    ) => [.. (await parties.ListAsync(cancellationToken).ConfigureAwait(false)).Select(ToResponse)];

    /// <summary>Reads a customer.</summary>
    public async Task<CustomerResponse?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var party = await parties.FindAsync(id, cancellationToken).ConfigureAwait(false);
        return party is null ? null : ToResponse(party);
    }

    private static CustomerResponse ToResponse(PartySnapshot party) =>
        new(
            party.Id,
            party.LegalName,
            party.TaxId,
            party.Address is null
                ? null
                : new AddressDto(
                    party.Address.Street,
                    party.Address.PostalCode,
                    party.Address.City,
                    party.Address.Province,
                    party.Address.CountryCode
                ),
            party.Email,
            party.IsInvoiceable
        );
}
