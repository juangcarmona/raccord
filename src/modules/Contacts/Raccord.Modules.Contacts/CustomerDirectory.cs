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
            customer.Type,
            request.DefaultDiscountPercent,
            request.DefaultPaymentTerms
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

    /// <summary>Imports valid CSV contact rows atomically and reports rejected rows.</summary>
    public async Task<ContactImportResponse> ImportCsvAsync(
        Stream content,
        CancellationToken cancellationToken
    )
    {
        using var reader = new StreamReader(content);
        var header = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false);
        if (!string.Equals(header, "legalName,taxId,email", StringComparison.OrdinalIgnoreCase))
            return InvalidImport("Expected header legalName,taxId,email.");

        var rows = await ReadImportRowsAsync(reader, cancellationToken).ConfigureAwait(false);

        if (rows.Any(row => row.Error is not null))
            return new ContactImportResponse(
                0,
                rows.Count,
                rows.Select(row => new ContactImportRowResponse(row.Number, false, row.Error, null))
                    .ToArray()
            );

        var imported = new List<ContactImportRowResponse>();
        foreach (var row in rows)
        {
            var request = row.Request!;
            var customer = Customer.Register(
                request.LegalName,
                ContactType.Customer,
                request.TaxId,
                null,
                request.Email
            );
            var party = new PartySnapshot(
                customer.Id,
                customer.LegalName,
                customer.TaxId,
                null,
                customer.Email,
                customer.Type
            );
            writer.Add(party);
            imported.Add(new ContactImportRowResponse(row.Number, true, null, ToResponse(party)));
        }
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return new ContactImportResponse(imported.Count, 0, imported);
    }

    private static ContactImportResponse InvalidImport(string error) =>
        new(0, 1, [new ContactImportRowResponse(1, false, error, null)]);

    private static async Task<
        List<(int Number, RegisterCustomerRequest? Request, string? Error)>
    > ReadImportRowsAsync(StreamReader reader, CancellationToken cancellationToken)
    {
        var rows = new List<(int Number, RegisterCustomerRequest? Request, string? Error)>();
        var number = 1;
        while (await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false) is { } line)
        {
            number++;
            var fields = line.Split(',', StringSplitOptions.None);
            rows.Add(
                fields.Length != 3 || string.IsNullOrWhiteSpace(fields[0])
                    ? (number, null, "Legal name is required and rows must have three columns.")
                    : (
                        number,
                        new RegisterCustomerRequest(
                            fields[0].Trim(),
                            string.IsNullOrWhiteSpace(fields[1]) ? null : fields[1].Trim(),
                            null,
                            string.IsNullOrWhiteSpace(fields[2]) ? null : fields[2].Trim()
                        ),
                        null
                    )
            );
        }
        return rows;
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
        CancellationToken cancellationToken,
        string? tag = null,
        string? folder = null
    )
    {
        var customers = await parties.ListAsync(cancellationToken).ConfigureAwait(false);
        return
        [
            .. customers
                .Where(customer =>
                    (
                        tag is null
                        || customer.Tags?.Contains(tag, StringComparer.OrdinalIgnoreCase) == true
                    )
                    && (
                        folder is null
                        || string.Equals(
                            customer.Folder,
                            folder,
                            StringComparison.OrdinalIgnoreCase
                        )
                    )
                )
                .Select(ToResponse),
        ];
    }

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
            party.IsInvoiceable,
            party.DefaultDiscountPercent,
            party.DefaultPaymentTerms
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
