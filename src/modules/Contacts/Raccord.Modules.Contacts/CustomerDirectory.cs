using Raccord.Contracts.Common;
using Raccord.Contracts.Customers;
using Raccord.Modules.Contacts.Contracts;
using Raccord.Modules.Contacts.Domain;
using Raccord.SharedKernel.Parties;

#pragma warning disable MA0051

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
            customer.Email,
            request.DefaultDiscountPercent,
            request.DefaultPaymentTerms
        );
        writer.Add(party);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(party);
    }

    /// <summary>Lists customers.</summary>
    public async Task<IReadOnlyList<CustomerResponse>> ListAsync(
        CancellationToken cancellationToken
    ) => [.. (await parties.ListAsync(cancellationToken).ConfigureAwait(false)).Select(ToResponse)];

    /// <summary>Imports valid CSV contact rows atomically and reports rejected rows.</summary>
    public async Task<ContactImportResponse> ImportCsvAsync(
        Stream content,
        CancellationToken cancellationToken
    )
    {
        using var reader = new StreamReader(content);
        var header = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false);
        if (!string.Equals(header, "legalName,taxId,email", StringComparison.OrdinalIgnoreCase))
        {
            return new ContactImportResponse(
                0,
                1,
                [
                    new ContactImportRowResponse(
                        1,
                        false,
                        "Expected header legalName,taxId,email.",
                        null
                    ),
                ]
            );
        }

        var rows = new List<(int Number, RegisterCustomerRequest? Request, string? Error)>();
        var number = 1;
        while (await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false) is { } line)
        {
            number++;
            var fields = line.Split(',', StringSplitOptions.None);
            if (fields.Length != 3 || string.IsNullOrWhiteSpace(fields[0]))
            {
                rows.Add(
                    (number, null, "Legal name is required and rows must have three columns.")
                );
                continue;
            }
            rows.Add(
                (
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

        if (rows.Any(row => row.Error is not null))
        {
            return new ContactImportResponse(
                0,
                rows.Count,
                rows.Select(row => new ContactImportRowResponse(row.Number, false, row.Error, null))
                    .ToArray()
            );
        }

        var imported = new List<ContactImportRowResponse>();
        foreach (var row in rows)
        {
            var request = row.Request!;
            var customer = Customer.Register(request.LegalName, request.TaxId, null, request.Email);
            var party = new PartySnapshot(
                customer.Id,
                customer.LegalName,
                customer.TaxId,
                null,
                customer.Email
            );
            writer.Add(party);
            imported.Add(new ContactImportRowResponse(row.Number, true, null, ToResponse(party)));
        }
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return new ContactImportResponse(imported.Count, 0, imported);
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
}

#pragma warning restore MA0051
