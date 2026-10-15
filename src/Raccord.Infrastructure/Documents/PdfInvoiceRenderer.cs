using System.Globalization;
using QRCoder;
using Raccord.Application.Documents;
using Raccord.BuildingBlocks.Storage;
using Raccord.Domain.Billing;
using Raccord.Infrastructure.Documents.Pdf;
using Raccord.SharedKernel.Parties;
using Format = Raccord.Infrastructure.Documents.SpanishInvoiceFormat;

namespace Raccord.Infrastructure.Documents;

/// <summary>
/// Renders an issued invoice as an A4 PDF. The document is a presentation of the issued representation;
/// it never recalculates or changes the invoice.
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

    private static (string Label, double Right)[] Columns(bool english) =>
        [
            (english ? "Quantity" : "Cantidad", 345),
            (english ? "Price" : "Precio", 410),
            (english ? "Discount" : "Dto.", 450),
            ("VAT", 490),
            (english ? "Amount" : "Importe", Right),
        ];

    public string ContentType => "application/pdf";

    public string FileExtension => "pdf";

    public byte[] Render(IssuedInvoice invoice) => Render(invoice, "es");

    public byte[] Render(IssuedInvoice invoice, string language)
    {
        ArgumentNullException.ThrowIfNull(invoice);
        var english = string.Equals(language, "en", StringComparison.OrdinalIgnoreCase);
        var pages = new List<PdfPage>();
        var page = NewPage(pages);
        var y = DrawHeader(page, invoice, english);
        DrawTaxQr(page, invoice, english);
        y = DrawTableHeader(page, y, english);

        foreach (var line in invoice.Lines)
        {
            var descriptionLines = Wrap(line.Description, DescriptionWidth, BodySize);
            var needed = descriptionLines.Count * RowHeight;
            if (y - needed < Bottom)
            {
                page = NewPage(pages);
                y = DrawTableHeader(page, ContinuationHeader(page, invoice, english), english);
            }
            DrawLine(page, line, invoice.Currency.Code, y, descriptionLines, english);
            y -= needed;
        }

        var totalsHeight = ((invoice.Totals.VatBreakdown.Count * 2) + 3) * RowHeight;
        if (y - totalsHeight < Bottom)
        {
            page = NewPage(pages);
            y = ContinuationHeader(page, invoice, english);
        }

        DrawTotals(page, invoice, y - 6, english);
        DrawFooters(pages, invoice, english);
        return PdfDocumentWriter.Write(
            pages,
            (english ? "Invoice " : "Factura ") + invoice.Number.Value
        );
    }

    private static void DrawLine(
        PdfPage page,
        IssuedInvoiceLine line,
        string currency,
        double y,
        IReadOnlyList<string> descriptionLines,
        bool english
    )
    {
        var pricing = line.Pricing;
        var columns = Columns(english);
        page.TextRight(columns[0].Right, y, Quantity(pricing.Quantity, english), BodySize);
        page.TextRight(columns[1].Right, y, UnitPrice(pricing.UnitPrice, english), BodySize);
        page.TextRight(
            columns[2].Right,
            y,
            pricing.DiscountPercent == 0 ? string.Empty : Percent(pricing.DiscountPercent, english),
            BodySize
        );
        page.TextRight(columns[3].Right, y, Percent(pricing.VatRate.Percentage, english), BodySize);
        page.TextRight(
            columns[4].Right,
            y,
            Money(pricing.NetAmount.Amount, currency, english),
            BodySize
        );
        foreach (var text in descriptionLines)
        {
            page.Text(Left, y, text, BodySize);
            y -= RowHeight;
        }
    }

    private static PdfPage NewPage(List<PdfPage> pages)
    {
        var page = new PdfPage();
        pages.Add(page);
        return page;
    }

    private static double DrawHeader(PdfPage page, IssuedInvoice invoice, bool english)
    {
        var y = DrawParty(page, Left, Top, invoice.Issuer, 14, english);

        page.TextRight(Right, Top, english ? "INVOICE" : "FACTURA", 18, bold: true);
        var metaY = Top - 24;
        DrawMeta(page, ref metaY, english ? "Number" : "Número", invoice.Number.Value);
        DrawMeta(
            page,
            ref metaY,
            english ? "Issue date" : "Fecha de expedición",
            FormatDate(invoice.IssueDate, english)
        );
        if (invoice.OperationDate is { } operationDate)
        {
            DrawMeta(
                page,
                ref metaY,
                english ? "Operation date" : "Fecha de operación",
                FormatDate(operationDate, english)
            );
        }

        y = Math.Min(y, metaY) - 24;
        page.Text(Left, y, english ? "Bill to" : "Facturar a", 8, bold: true);
        return DrawParty(page, Left, y - 14, invoice.Recipient, 11, english) - 20;
    }

    private static void DrawTaxQr(PdfPage page, IssuedInvoice invoice, bool english)
    {
        const double size = 85;
        const double left = 450;
        const double bottom = 585;
        const double module = 2.5;
        const double quiet = 4;
        page.Text(
            left,
            bottom + size + 13,
            english ? "Tax QR:" : "QR tributario:",
            BodySize,
            bold: true
        );
        using var generator = new QRCodeGenerator();
        using var data = generator.CreateQrCode(
            AeatQrPayload.Build(invoice),
            QRCodeGenerator.ECCLevel.M
        );
        var matrix = data.ModuleMatrix;
        var scale = Math.Min((size - (quiet * 2)) / matrix.Count, module);
        var actual = matrix.Count * scale;
        var originX = left + ((size - actual) / 2);
        var originY = bottom + ((size - actual) / 2);
        page.FillRectangle(left, bottom, size, size, gray: 1);
        page.FillRectangle(left + quiet, bottom + quiet, size - (quiet * 2), size - (quiet * 2));
        for (var row = 0; row < matrix.Count; row++)
        {
            for (var column = 0; column < matrix[row].Count; column++)
            {
                if (matrix[row][column])
                    page.FillRectangle(
                        originX + (column * scale),
                        originY + ((matrix.Count - row - 1) * scale),
                        scale,
                        scale
                    );
            }
        }
        page.Text(left, bottom - 12, "VERI*FACTU", BodySize, bold: true);
    }

    private static void DrawMeta(PdfPage page, ref double y, string label, string value)
    {
        page.TextRight(Right - 90, y, label, BodySize);
        page.TextRight(Right, y, value, BodySize, bold: true);
        y -= RowHeight;
    }

    private static double DrawParty(
        PdfPage page,
        double x,
        double y,
        LegalParty party,
        double nameSize,
        bool english
    )
    {
        page.Text(x, y, party.LegalName, nameSize, bold: true);
        y -= nameSize + 3;
        page.Text(x, y, (english ? "Tax ID: " : "NIF: ") + party.TaxId, BodySize);
        y -= RowHeight;
        if (party.Address is { } address)
        {
            page.Text(x, y, address.Street, BodySize);
            y -= RowHeight;
            var locality = string.Join(
                ' ',
                new[] { address.PostalCode, address.City }.Where(part => part.Length > 0)
            );
            var region = string.Join(
                ", ",
                new[] { locality, address.Province, address.CountryCode }.Where(part =>
                    part.Length > 0
                )
            );
            page.Text(x, y, region, BodySize);
            y -= RowHeight;
        }

        return y;
    }

    private static double ContinuationHeader(PdfPage page, IssuedInvoice invoice, bool english)
    {
        page.Text(Left, Top, invoice.Issuer.LegalName, 10, bold: true);
        page.TextRight(
            Right,
            Top,
            (english ? "Invoice " : "Factura ")
                + invoice.Number.Value
                + (english ? " (continued)" : " (continuación)"),
            10,
            bold: true
        );
        return Top - 30;
    }

    private static double DrawTableHeader(PdfPage page, double y, bool english)
    {
        page.Band(Left - 4, y - 4, Right - Left + 8, RowHeight + 2);
        page.Text(Left, y, english ? "Description" : "Descripción", 8, bold: true);
        foreach (var (label, right) in Columns(english))
        {
            page.TextRight(right, y, label, 8, bold: true);
        }

        return y - RowHeight - 6;
    }

    private static void DrawTotals(PdfPage page, IssuedInvoice invoice, double y, bool english)
    {
        const double LabelRight = 450;
        var currency = invoice.Currency.Code;
        page.Rule(330, Right, y + RowHeight - 2);
        foreach (var group in invoice.Totals.VatBreakdown)
        {
            var rate = Percent(group.Rate.Percentage, english);
            page.TextRight(
                LabelRight,
                y,
                (english ? "Tax base (" : "Base imponible (") + rate + ")",
                BodySize
            );
            page.TextRight(Right, y, Money(group.TaxableBase.Amount, currency, english), BodySize);
            y -= RowHeight;
            page.TextRight(LabelRight, y, (english ? "VAT " : "Cuota IVA ") + rate, BodySize);
            page.TextRight(Right, y, Money(group.VatAmount.Amount, currency, english), BodySize);
            y -= RowHeight;
        }

        page.Rule(330, Right, y + RowHeight - 2);
        page.TextRight(
            LabelRight,
            y - 4,
            english ? "Invoice total" : "Total factura",
            11,
            bold: true
        );
        page.TextRight(
            Right,
            y - 4,
            Money(invoice.Totals.Total.Amount, currency, english),
            11,
            bold: true
        );
    }

    private static void DrawFooters(List<PdfPage> pages, IssuedInvoice invoice, bool english)
    {
        var issued = invoice.IssuedAt.UtcDateTime.ToString(
            "yyyy-MM-dd HH:mm:ss 'UTC'",
            CultureInfo.InvariantCulture
        );
        for (var index = 0; index < pages.Count; index++)
        {
            var page = pages[index];
            page.Rule(Left, Right, 52, 0.3);
            page.Text(
                Left,
                40,
                (english ? "Issued on " : "Emitida el ")
                    + issued
                    + (
                        english
                            ? " · Issued representation verifiable by its SHA-256 hash"
                            : " · Representación emitida verificable por su huella SHA-256"
                    ),
                7
            );
            page.TextRight(
                Right,
                40,
                english
                    ? $"{invoice.Number.Value} · Page {index + 1} of {pages.Count}"
                    : $"{invoice.Number.Value} · Página {index + 1} de {pages.Count}",
                7
            );
        }
    }

    private static string Quantity(decimal value, bool english) =>
        english ? EnglishInvoiceFormat.Quantity(value) : Format.Quantity(value);

    private static string UnitPrice(decimal value, bool english) =>
        english ? EnglishInvoiceFormat.UnitPrice(value) : Format.UnitPrice(value);

    private static string Percent(decimal value, bool english) =>
        english ? EnglishInvoiceFormat.Percent(value) : Format.Percent(value);

    private static string Money(decimal value, string currency, bool english) =>
        english ? EnglishInvoiceFormat.Money(value, currency) : Format.Money(value, currency);

    private static string FormatDate(DateOnly value, bool english) =>
        english ? EnglishInvoiceFormat.Date(value) : Format.Date(value);

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
