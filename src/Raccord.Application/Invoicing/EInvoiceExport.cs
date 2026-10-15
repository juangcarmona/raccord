using System.Globalization;
using System.Xml.Linq;
using Raccord.Domain.Billing;
using Raccord.SharedKernel.Common;

namespace Raccord.Application.Invoicing;

/// <summary>Builds the supported structured representation of an issued invoice.</summary>
public static class EInvoiceExport
{
    /// <summary>The supported structured invoice format.</summary>
    public const string Format = "ubl";

    /// <summary>The media type returned for structured invoice files.</summary>
    public const string ContentType = "application/xml; charset=utf-8";

    private static readonly XNamespace Invoice =
        "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2";
    private static readonly XNamespace Basic =
        "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
    private static readonly XNamespace Aggregate =
        "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";

    /// <summary>Builds deterministic UBL 2.1 XML from an immutable invoice.</summary>
    public static byte[] Build(IssuedInvoice invoice)
    {
        ArgumentNullException.ThrowIfNull(invoice);
        EnsureIdentifiers(invoice);

        var document = new XDocument(
            new XDeclaration("1.0", "utf-8", null),
            new XElement(
                Invoice + "Invoice",
                new XAttribute(XNamespace.Xmlns + "cbc", Basic),
                new XAttribute(XNamespace.Xmlns + "cac", Aggregate),
                new XElement(Basic + "UBLVersionID", "2.1"),
                new XElement(Basic + "CustomizationID", "urn:cen.eu:en16931:2017"),
                new XElement(Basic + "ID", invoice.Number.Value),
                new XElement(
                    Basic + "IssueDate",
                    invoice.IssueDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                ),
                new XElement(Basic + "InvoiceTypeCode", "380"),
                new XElement(Basic + "DocumentCurrencyCode", invoice.Currency.Code),
                Party(Aggregate + "AccountingSupplierParty", invoice.Issuer),
                Party(Aggregate + "AccountingCustomerParty", invoice.Recipient),
                TaxTotal(invoice),
                new XElement(
                    Basic + "LegalMonetaryTotal",
                    Amount(
                        Aggregate + "TaxExclusiveAmount",
                        invoice.Totals.TaxableBase.Amount,
                        invoice.Currency.Code
                    ),
                    Amount(
                        Aggregate + "TaxInclusiveAmount",
                        invoice.Totals.Total.Amount,
                        invoice.Currency.Code
                    ),
                    Amount(
                        Aggregate + "PayableAmount",
                        invoice.Totals.Total.Amount,
                        invoice.Currency.Code
                    )
                ),
                invoice.Lines.Select(line => InvoiceLine(line, invoice.Currency.Code))
            )
        );

        return System.Text.Encoding.UTF8.GetBytes(document.ToString(SaveOptions.DisableFormatting));
    }

    private static void EnsureIdentifiers(IssuedInvoice invoice)
    {
        if (string.IsNullOrWhiteSpace(invoice.Issuer.TaxId))
            throw new BusinessRuleViolationException(
                "e-invoice.issuer-identifier-missing",
                "The issuer tax identifier is required for e-invoice export."
            );
        if (string.IsNullOrWhiteSpace(invoice.Recipient.TaxId))
            throw new BusinessRuleViolationException(
                "e-invoice.recipient-identifier-missing",
                "The customer tax identifier is required for e-invoice export."
            );
    }

    private static XElement Party(XName name, Raccord.SharedKernel.Parties.LegalParty party) =>
        new(
            name,
            new XElement(
                Aggregate + "Party",
                new XElement(
                    Aggregate + "PartyName",
                    new XElement(Basic + "Name", party.LegalName)
                ),
                new XElement(
                    Aggregate + "PartyTaxScheme",
                    new XElement(
                        Basic + "CompanyID",
                        new XAttribute("schemeID", "VAT"),
                        party.TaxId
                    ),
                    new XElement(Aggregate + "TaxScheme", new XElement(Basic + "ID", "VAT"))
                )
            )
        );

    private static XElement TaxTotal(IssuedInvoice invoice) =>
        new(
            Aggregate + "TaxTotal",
            Amount(Basic + "TaxAmount", invoice.Totals.VatAmount.Amount, invoice.Currency.Code),
            invoice.Totals.VatBreakdown.Select(group => new XElement(
                Aggregate + "TaxSubtotal",
                Amount(Basic + "TaxableAmount", group.TaxableBase.Amount, invoice.Currency.Code),
                Amount(Basic + "TaxAmount", group.VatAmount.Amount, invoice.Currency.Code),
                new XElement(
                    Aggregate + "TaxCategory",
                    new XElement(Basic + "ID", TaxCategory(group.Rate.Percentage)),
                    new XElement(
                        Basic + "Percent",
                        group.Rate.Percentage.ToString("0.##", CultureInfo.InvariantCulture)
                    ),
                    new XElement(Aggregate + "TaxScheme", new XElement(Basic + "ID", "VAT"))
                )
            ))
        );

    private static XElement InvoiceLine(IssuedInvoiceLine line, string currency) =>
        new(
            Aggregate + "InvoiceLine",
            new XElement(Basic + "ID", line.Position.ToString(CultureInfo.InvariantCulture)),
            new XElement(
                Basic + "InvoicedQuantity",
                new XAttribute("unitCode", "C62"),
                line.Pricing.Quantity.ToString("0.####", CultureInfo.InvariantCulture)
            ),
            Amount(Basic + "LineExtensionAmount", line.Pricing.NetAmount.Amount, currency),
            new XElement(
                Aggregate + "Item",
                new XElement(Basic + "Name", line.Description),
                new XElement(
                    Aggregate + "ClassifiedTaxCategory",
                    new XElement(Basic + "ID", TaxCategory(line.Pricing.VatRate.Percentage)),
                    new XElement(
                        Basic + "Percent",
                        line.Pricing.VatRate.Percentage.ToString(
                            "0.##",
                            CultureInfo.InvariantCulture
                        )
                    ),
                    new XElement(Aggregate + "TaxScheme", new XElement(Basic + "ID", "VAT"))
                )
            ),
            new XElement(
                Aggregate + "Price",
                Amount(Basic + "PriceAmount", line.Pricing.UnitPrice, currency)
            )
        );

    private static XElement Amount(XName name, decimal value, string currency) =>
        new(
            name,
            new XAttribute("currencyID", currency),
            value.ToString("0.00", CultureInfo.InvariantCulture)
        );

    private static string TaxCategory(decimal rate) => rate == 0m ? "E" : "S";
}
