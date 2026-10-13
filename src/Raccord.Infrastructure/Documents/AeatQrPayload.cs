using System.Globalization;
using Raccord.Domain.Billing;

namespace Raccord.Infrastructure.Documents;

internal static class AeatQrPayload
{
    private const string VerificationUrl =
        "https://www2.agenciatributaria.gob.es/wlpl/TIKE-CONT/ValidarQR";

    public static string Build(IssuedInvoice invoice)
    {
        var query = string.Join(
            '&',
            "nif=" + Uri.EscapeDataString(invoice.Issuer.TaxId ?? string.Empty),
            "numserie=" + Uri.EscapeDataString(invoice.Number.Value),
            "fecha="
                + Uri.EscapeDataString(
                    invoice.IssueDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)
                ),
            "importe="
                + Uri.EscapeDataString(
                    invoice.Totals.Total.Amount.ToString("0.00", CultureInfo.InvariantCulture)
                )
        );
        return VerificationUrl + "?" + query;
    }
}
