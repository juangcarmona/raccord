using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Parties;
using Raccord.SharedKernel.Pricing;

namespace Raccord.Domain.Billing;

/// <summary>
/// The canonical, immutable representation of an issued invoice: deterministic JSON plus its SHA-256 hash.
/// It is what Raccord stores, renders documents from and later feeds to compliance adapters.
/// </summary>
/// <param name="Json">Canonical JSON. Numbers are fixed-point strings so no reader can reinterpret them.</param>
/// <param name="ContentHash">Lowercase hexadecimal SHA-256 of the UTF-8 JSON.</param>
public sealed record IssuedInvoiceRepresentation(string Json, string ContentHash)
{
    /// <summary>Schema identifier written into every representation.</summary>
    public const string Schema = "raccord.issued-invoice/1";

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
    };

    /// <summary>Creates the representation of an invoice.</summary>
    public static IssuedInvoiceRepresentation From(IssuedInvoice invoice)
    {
        ArgumentNullException.ThrowIfNull(invoice);
        var json = JsonSerializer.Serialize(ToDocument(invoice), SerializerOptions);
        return new IssuedInvoiceRepresentation(json, Hash(json));
    }

    /// <summary>Restores an invoice after verifying that the stored JSON still matches its hash.</summary>
    public static IssuedInvoice Restore(string json, string expectedHash)
    {
        ArgumentNullException.ThrowIfNull(json);
        if (!string.Equals(Hash(json), expectedHash, StringComparison.Ordinal))
        {
            throw new BusinessRuleViolationException(
                "issued-invoice.integrity-violation",
                "The stored invoice does not match its content hash."
            );
        }

        var document =
            JsonSerializer.Deserialize<InvoiceDocument>(json, SerializerOptions)
            ?? throw new BusinessRuleViolationException(
                "issued-invoice.integrity-violation",
                "The stored invoice is empty."
            );
        if (!string.Equals(document.Schema, Schema, StringComparison.Ordinal))
        {
            throw new BusinessRuleViolationException(
                "issued-invoice.schema-unsupported",
                $"Unsupported invoice schema '{document.Schema}'."
            );
        }

        return FromDocument(document);
    }

    private static string Hash(string json) =>
        Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(json)));

    private static InvoiceDocument ToDocument(IssuedInvoice invoice)
    {
        var minorUnits = invoice.Currency.MinorUnits;
        return new InvoiceDocument(
            Schema,
            invoice.Id,
            invoice.Source.DraftId,
            invoice.Source.ProjectId,
            invoice.Source.CustomerId,
            invoice.Number.SeriesCode,
            invoice.Number.Year,
            invoice.Number.Sequence,
            invoice.Number.Value,
            Date(invoice.IssueDate),
            invoice.OperationDate is { } operationDate ? Date(operationDate) : null,
            invoice.IssuedAt.UtcDateTime.ToString(
                "yyyy-MM-ddTHH:mm:ss.fffffffZ",
                CultureInfo.InvariantCulture
            ),
            invoice.Currency.Code,
            Party(invoice.Issuer),
            Party(invoice.Recipient),
            [
                .. invoice.Lines.Select(line => new LineDocument(
                    line.Position,
                    line.Description,
                    Number(line.Pricing.Quantity),
                    Number(line.Pricing.UnitPrice),
                    Number(line.Pricing.DiscountPercent),
                    Number(line.Pricing.VatRate.Percentage),
                    Amount(line.Pricing.NetAmount.Amount, minorUnits)
                )),
            ],
            [
                .. invoice.Totals.VatBreakdown.Select(group => new VatDocument(
                    Number(group.Rate.Percentage),
                    Amount(group.TaxableBase.Amount, minorUnits),
                    Amount(group.VatAmount.Amount, minorUnits)
                )),
            ],
            Amount(invoice.Totals.TaxableBase.Amount, minorUnits),
            Amount(invoice.Totals.VatAmount.Amount, minorUnits),
            Amount(invoice.Totals.Total.Amount, minorUnits)
        );
    }

    private static IssuedInvoice FromDocument(InvoiceDocument document)
    {
        var currency = Currency.FromCode(document.Currency);
        var lines = document
            .Lines.Select(line => new IssuedInvoiceLine(
                line.Position,
                line.Description,
                LinePricing.Create(
                    Parse(line.Quantity),
                    Parse(line.UnitPrice),
                    Parse(line.DiscountPercent),
                    VatRate.Of(Parse(line.VatRate))
                )
            ))
            .ToList();

        return new IssuedInvoice(
            document.Id,
            new IssuedInvoiceSource(document.DraftId, document.ProjectId, document.CustomerId),
            new InvoiceNumber(document.Series, document.Year, document.Sequence, document.Number),
            DateOnly.ParseExact(document.IssueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
            document.OperationDate is { } operationDate
                ? DateOnly.ParseExact(operationDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                : null,
            DateTimeOffset.Parse(
                document.IssuedAt,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
            ),
            Party(document.Issuer),
            Party(document.Recipient),
            lines,
            currency
        );
    }

    private static PartyDocument Party(LegalParty party) =>
        new(
            party.LegalName,
            party.TaxId,
            party.Address is { } a
                ? new AddressDocument(a.Street, a.PostalCode, a.City, a.Province, a.CountryCode)
                : null
        );

    private static LegalParty Party(PartyDocument party) =>
        new(
            party.LegalName,
            party.TaxId,
            party.Address is { } a
                ? new PostalAddress(a.Street, a.PostalCode, a.City, a.Province, a.CountryCode)
                : null
        );

    private static string Date(DateOnly date) =>
        date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

    private static string Number(decimal value) =>
        value.ToString("0.############################", CultureInfo.InvariantCulture);

    private static string Amount(decimal value, int minorUnits) =>
        value.ToString(
            "F" + minorUnits.ToString(CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture
        );

    private static decimal Parse(string value) =>
        decimal.Parse(
            value,
            NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture
        );

    private sealed record InvoiceDocument(
        string Schema,
        Guid Id,
        Guid DraftId,
        Guid ProjectId,
        Guid CustomerId,
        string Series,
        int Year,
        int Sequence,
        string Number,
        string IssueDate,
        string? OperationDate,
        string IssuedAt,
        string Currency,
        PartyDocument Issuer,
        PartyDocument Recipient,
        IReadOnlyList<LineDocument> Lines,
        IReadOnlyList<VatDocument> VatBreakdown,
        string TaxableBase,
        string VatAmount,
        string Total
    );

    private sealed record PartyDocument(string LegalName, string? TaxId, AddressDocument? Address);

    private sealed record AddressDocument(
        string Street,
        string PostalCode,
        string City,
        string Province,
        string CountryCode
    );

    private sealed record LineDocument(
        int Position,
        string Description,
        string Quantity,
        string UnitPrice,
        string DiscountPercent,
        string VatRate,
        string NetAmount
    );

    private sealed record VatDocument(string Rate, string TaxableBase, string VatAmount);
}
