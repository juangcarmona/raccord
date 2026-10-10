using System.Globalization;
using Raccord.Application.Documents;
using Raccord.Domain.Billing;
using Raccord.Domain.Parties;
using Raccord.Infrastructure.Documents.Pdf;
using Format = Raccord.Infrastructure.Documents.SpanishInvoiceFormat;

namespace Raccord.Infrastructure.Documents;

/// <summary>
/// Renders an issued invoice as an A4 PDF with Spanish labels. The document is a presentation of the issued
/// representation; it never recalculates or changes the invoice.
/// </summary>
internal sealed class PdfInvoiceRenderer : IInvoiceDocumentRenderer
{
    private const double Left = 50;
    private const double Right = 545;
    private const double Top = 790;
    private const double Bottom = 70;
    private const double RowHeight = 13;
    private const double BodySize = 9;
    private const double DescriptionWidth = 235;

    private static readonly (string Label, double Right)[] Columns =
    [
        ("Cantidad", 345), ("Precio", 410), ("Dto.", 450), ("IVA", 490), ("Importe", Right),
    ];

    public string ContentType => "application/pdf";

    public string FileExtension => "pdf";

    public byte[] Render(IssuedInvoice invoice)
    {
        ArgumentNullException.ThrowIfNull(invoice);
        var pages = new List<PdfPage>();
        var page = NewPage(pages);
        var y = DrawHeader(page, invoice);
        y = DrawTableHeader(page, y);

        foreach (var line in invoice.Lines)
        {
            var descriptionLines = Wrap(line.Description, DescriptionWidth, BodySize);
            var needed = descriptionLines.Count * RowHeight;
            if (y - needed < Bottom)
            {
                page = NewPage(pages);
                y = DrawTableHeader(page, ContinuationHeader(page, invoice));
            }

            var pricing = line.Pricing;
            page.TextRight(Columns[0].Right, y, Format.Quantity(pricing.Quantity), BodySize);
            page.TextRight(Columns[1].Right, y, Format.UnitPrice(pricing.UnitPrice), BodySize);
            page.TextRight(Columns[2].Right, y, pricing.DiscountPercent == 0 ? string.Empty : Format.Percent(pricing.DiscountPercent), BodySize);
            page.TextRight(Columns[3].Right, y, Format.Percent(pricing.VatRate.Percentage), BodySize);
            page.TextRight(Columns[4].Right, y, Format.Money(pricing.NetAmount.Amount, invoice.Currency.Code), BodySize);
            foreach (var text in descriptionLines)
            {
                page.Text(Left, y, text, BodySize);
                y -= RowHeight;
            }
        }

        var totalsHeight = ((invoice.Totals.VatBreakdown.Count * 2) + 3) * RowHeight;
        if (y - totalsHeight < Bottom)
        {
            page = NewPage(pages);
            y = ContinuationHeader(page, invoice);
        }

        DrawTotals(page, invoice, y - 6);
        DrawFooters(pages, invoice);
        return PdfDocumentWriter.Write(pages, "Factura " + invoice.Number.Value);
    }

    private static PdfPage NewPage(List<PdfPage> pages)
    {
        var page = new PdfPage();
        pages.Add(page);
        return page;
    }

    private static double DrawHeader(PdfPage page, IssuedInvoice invoice)
    {
        var y = DrawParty(page, Left, Top, invoice.Issuer, 14);

        page.TextRight(Right, Top, "FACTURA", 18, bold: true);
        var metaY = Top - 24;
        DrawMeta(page, ref metaY, "Número", invoice.Number.Value);
        DrawMeta(page, ref metaY, "Fecha de expedición", Format.Date(invoice.IssueDate));
        if (invoice.OperationDate is { } operationDate)
        {
            DrawMeta(page, ref metaY, "Fecha de operación", Format.Date(operationDate));
        }

        y = Math.Min(y, metaY) - 24;
        page.Text(Left, y, "Facturar a", 8, bold: true);
        return DrawParty(page, Left, y - 14, invoice.Recipient, 11) - 20;
    }

    private static void DrawMeta(PdfPage page, ref double y, string label, string value)
    {
        page.TextRight(Right - 90, y, label, BodySize);
        page.TextRight(Right, y, value, BodySize, bold: true);
        y -= RowHeight;
    }

    private static double DrawParty(PdfPage page, double x, double y, LegalParty party, double nameSize)
    {
        page.Text(x, y, party.LegalName, nameSize, bold: true);
        y -= nameSize + 3;
        page.Text(x, y, "NIF: " + party.TaxId, BodySize);
        y -= RowHeight;
        if (party.Address is { } address)
        {
            page.Text(x, y, address.Street, BodySize);
            y -= RowHeight;
            var locality = string.Join(' ', new[] { address.PostalCode, address.City }.Where(part => part.Length > 0));
            var region = string.Join(", ", new[] { locality, address.Province, address.CountryCode }.Where(part => part.Length > 0));
            page.Text(x, y, region, BodySize);
            y -= RowHeight;
        }

        return y;
    }

    private static double ContinuationHeader(PdfPage page, IssuedInvoice invoice)
    {
        page.Text(Left, Top, invoice.Issuer.LegalName, 10, bold: true);
        page.TextRight(Right, Top, "Factura " + invoice.Number.Value + " (continuación)", 10, bold: true);
        return Top - 30;
    }

    private static double DrawTableHeader(PdfPage page, double y)
    {
        page.Band(Left - 4, y - 4, Right - Left + 8, RowHeight + 2);
        page.Text(Left, y, "Descripción", 8, bold: true);
        foreach (var (label, right) in Columns)
        {
            page.TextRight(right, y, label, 8, bold: true);
        }

        return y - RowHeight - 6;
    }

    private static void DrawTotals(PdfPage page, IssuedInvoice invoice, double y)
    {
        const double LabelRight = 450;
        var currency = invoice.Currency.Code;
        page.Rule(330, Right, y + RowHeight - 2);
        foreach (var group in invoice.Totals.VatBreakdown)
        {
            var rate = Format.Percent(group.Rate.Percentage);
            page.TextRight(LabelRight, y, "Base imponible (" + rate + ")", BodySize);
            page.TextRight(Right, y, Format.Money(group.TaxableBase.Amount, currency), BodySize);
            y -= RowHeight;
            page.TextRight(LabelRight, y, "Cuota IVA " + rate, BodySize);
            page.TextRight(Right, y, Format.Money(group.VatAmount.Amount, currency), BodySize);
            y -= RowHeight;
        }

        page.Rule(330, Right, y + RowHeight - 2);
        page.TextRight(LabelRight, y - 4, "Total factura", 11, bold: true);
        page.TextRight(Right, y - 4, Format.Money(invoice.Totals.Total.Amount, currency), 11, bold: true);
    }

    private static void DrawFooters(List<PdfPage> pages, IssuedInvoice invoice)
    {
        var issued = invoice.IssuedAt.UtcDateTime.ToString("yyyy-MM-dd HH:mm:ss 'UTC'", CultureInfo.InvariantCulture);
        for (var index = 0; index < pages.Count; index++)
        {
            var page = pages[index];
            page.Rule(Left, Right, 52, 0.3);
            page.Text(Left, 40, "Emitida el " + issued + " · Representación emitida verificable por su huella SHA-256", 7);
            page.TextRight(Right, 40, string.Create(CultureInfo.InvariantCulture, $"{invoice.Number.Value} · Página {index + 1} de {pages.Count}"), 7);
        }
    }

    private static List<string> Wrap(string text, double width, double size)
    {
        var lines = new List<string>();
        var current = string.Empty;
        foreach (var word in text.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            var candidate = current.Length == 0 ? word : current + " " + word;
            if (WinAnsi.Measure(candidate, bold: false, size) <= width)
            {
                current = candidate;
                continue;
            }

            if (current.Length > 0)
            {
                lines.Add(current);
            }

            current = word;
            while (WinAnsi.Measure(current, bold: false, size) > width && current.Length > 1)
            {
                var cut = current.Length - 1;
                while (cut > 1 && WinAnsi.Measure(current[..cut], bold: false, size) > width)
                {
                    cut--;
                }

                lines.Add(current[..cut]);
                current = current[cut..];
            }
        }

        lines.Add(current);
        return lines;
    }
}
