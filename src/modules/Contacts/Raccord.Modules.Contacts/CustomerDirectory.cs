using Raccord.Contracts.Common;
using Raccord.Contracts.Contacts;
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
            ContactType.Customer,
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
            customer.Email,
            customer.Type
        );
        writer.Add(party);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(party);
    }

    /// <summary>Registers a contact of any supported operational type.</summary>
    public async Task<ContactResponse> RegisterContactAsync(
        RegisterContactRequest request,
        CancellationToken cancellationToken
    )
    {
        var party = await SaveContactAsync(
                request.LegalName,
                request.Type,
                request.TaxId,
                request.Address,
                request.Email,
                cancellationToken
            )
            .ConfigureAwait(false);
        return ToContactResponse(party);
    }

    /// <summary>Updates a contact without changing issued invoice snapshots.</summary>
    public async Task<ContactResponse?> UpdateContactAsync(
        Guid id,
        UpdateContactRequest request,
        CancellationToken cancellationToken
    )
    {
        var existing = await parties.FindAsync(id, cancellationToken).ConfigureAwait(false);
        if (existing is null)
            return null;
        var address = request.Address is null
            ? null
            : new PostalAddress(
                request.Address.Street,
                request.Address.PostalCode,
                request.Address.City,
                request.Address.Province,
                request.Address.CountryCode
            );
        var updated = existing with
        {
            LegalName = request.LegalName,
            TaxId = Raccord.SharedKernel.Parties.TaxId.Normalize(request.TaxId),
            Address = address,
            Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim(),
            Type = request.Type,
        };
        writer.Update(updated);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToContactResponse(updated);
    }

    /// <summary>Lists contacts using one-based pagination.</summary>
    public async Task<ContactPage> ListContactsAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken
    )
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);
        var all = await parties.ListAsync(cancellationToken).ConfigureAwait(false);
        return new ContactPage(
            [.. all.Skip((page - 1) * pageSize).Take(pageSize).Select(ToContactResponse)],
            page,
            pageSize,
            all.Count
        );
    }

    /// <summary>Reads a contact.</summary>
    public async Task<ContactResponse?> GetContactAsync(
        Guid id,
        CancellationToken cancellationToken
    ) =>
        (await parties.FindAsync(id, cancellationToken).ConfigureAwait(false)) is { } party
            ? ToContactResponse(party)
            : null;

    private async Task<PartySnapshot> SaveContactAsync(
        string legalName,
        ContactType type,
        string? taxId,
        AddressDto? address,
        string? email,
        CancellationToken cancellationToken
    )
    {
        var contact = Customer.Register(
            legalName ?? string.Empty,
            type,
            taxId,
            address is null
                ? null
                : new PostalAddress(
                    address.Street,
                    address.PostalCode,
                    address.City,
                    address.Province,
                    address.CountryCode
                ),
            email
        );
        var party = new PartySnapshot(
            contact.Id,
            contact.LegalName,
            contact.TaxId,
            contact.BillingAddress.ToAddress(),
            contact.Email,
            contact.Type
        );
        writer.Add(party);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return party;
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

    private static ContactResponse ToContactResponse(PartySnapshot party) =>
        new(
            party.Id,
            party.LegalName,
            party.Type,
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
