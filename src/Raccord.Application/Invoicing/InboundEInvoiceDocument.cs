using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using Raccord.SharedKernel.Common;

namespace Raccord.Application.Invoicing;

internal sealed record InboundEInvoiceDocument(
    string InvoiceNumber,
    string SupplierName,
    string SupplierTaxId,
    DateOnly IssueDate,
    decimal PayableAmount,
    string Currency
)
{
#pragma warning disable MA0051
    public static InboundEInvoiceDocument Parse(string xml)
    {
        if (string.IsNullOrWhiteSpace(xml))
            throw new BusinessRuleViolationException(
                "inbound-e-invoice.xml-missing",
                "The structured invoice XML is required."
            );
        XDocument document;
        try
        {
            document = XDocument.Parse(xml, LoadOptions.PreserveWhitespace);
        }
        catch (XmlException)
        {
            throw new BusinessRuleViolationException(
                "inbound-e-invoice.xml-invalid",
                "The structured invoice XML is invalid."
            );
        }
        if (!string.Equals(document.Root?.Name.LocalName, "Invoice", StringComparison.Ordinal))
            throw new BusinessRuleViolationException(
                "inbound-e-invoice.document-unsupported",
                "Only UBL Invoice documents are supported."
            );

        var number = Value(document, "ID");
        var supplier =
            document
                .Descendants()
                .FirstOrDefault(item =>
                    string.Equals(
                        item.Name.LocalName,
                        "AccountingSupplierParty",
                        StringComparison.Ordinal
                    )
                )
            ?? throw new BusinessRuleViolationException(
                "inbound-e-invoice.supplier-missing",
                "The supplier is required."
            );
        var supplierName = Value(supplier, "Name");
        var supplierTaxId = Value(supplier, "CompanyID");
        var issueDate = DateOnly.ParseExact(
            Value(document, "IssueDate"),
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture
        );
        var payable =
            document
                .Descendants()
                .FirstOrDefault(item =>
                    string.Equals(item.Name.LocalName, "PayableAmount", StringComparison.Ordinal)
                )
            ?? throw new BusinessRuleViolationException(
                "inbound-e-invoice.total-missing",
                "The payable amount is required."
            );
        var currency =
            payable.Attribute("currencyID")?.Value ?? Value(document, "DocumentCurrencyCode");
        return new(
            number,
            supplierName,
            supplierTaxId,
            issueDate,
            decimal.Parse(payable.Value, CultureInfo.InvariantCulture),
            currency
        );
    }
#pragma warning restore MA0051

    private static string Value(XContainer container, string localName) =>
        container
            .Descendants()
            .FirstOrDefault(item =>
                string.Equals(item.Name.LocalName, localName, StringComparison.Ordinal)
            )
            ?.Value.Trim()
        ?? throw new BusinessRuleViolationException(
            "inbound-e-invoice.field-missing",
            $"The structured invoice field '{localName}' is required."
        );
}
