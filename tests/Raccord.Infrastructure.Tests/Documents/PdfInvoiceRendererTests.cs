using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Raccord.Infrastructure.Documents;

namespace Raccord.Infrastructure.Tests.Documents;

public sealed class PdfInvoiceRendererTests
{
    private static readonly Encoding Latin1 = Encoding.Latin1;
    private readonly PdfInvoiceRenderer renderer = new();

    [Fact]
    public void The_document_is_a_structurally_valid_pdf()
    {
        var pdf = renderer.Render(SampleInvoices.Issue());
        var text = Latin1.GetString(pdf);

        Assert.StartsWith("%PDF-1.7", text, StringComparison.Ordinal);
        Assert.EndsWith("%%EOF\n", text, StringComparison.Ordinal);

        var startXref = int.Parse(
            Regex.Match(text, @"startxref\n(\d+)\n%%EOF\n$").Groups[1].Value,
            CultureInfo.InvariantCulture
        );
        Assert.StartsWith("xref", text[startXref..], StringComparison.Ordinal);

        var offsets = Regex
            .Matches(
                text[startXref..],
                @"^(?<offset>\d{10}) 00000 n ?$",
                RegexOptions.Multiline | RegexOptions.ExplicitCapture
            )
            .Select(match => int.Parse(match.Groups["offset"].Value, CultureInfo.InvariantCulture))
            .ToList();
        Assert.NotEmpty(offsets);
        for (var objectNumber = 1; objectNumber <= offsets.Count; objectNumber++)
        {
            Assert.StartsWith(
                $"{objectNumber} 0 obj",
                text[offsets[objectNumber - 1]..],
                StringComparison.Ordinal
            );
        }
    }

    [Fact]
    public void The_document_shows_number_parties_and_spanish_formatted_totals()
    {
        var text = Latin1.GetString(renderer.Render(SampleInvoices.Issue()));

        Assert.Contains("(F2026-00001)", text, StringComparison.Ordinal);
        Assert.Contains("(Productora Ejemplo S.A.)", text, StringComparison.Ordinal);
        Assert.Contains("(NIF: A00000000)", text, StringComparison.Ordinal);
        Assert.Contains(
            "(1.548,80 €)",
            text.Replace("\u0080", "€", StringComparison.Ordinal),
            StringComparison.Ordinal
        );
        Assert.Contains("(11/05/2026)", text, StringComparison.Ordinal);
    }

    [Fact]
    public void Non_ascii_text_is_encoded_as_single_win_ansi_bytes()
    {
        var pdf = renderer.Render(SampleInvoices.Issue());

        var expected = new byte[]
        {
            (byte)'K',
            (byte)'i',
            (byte)'t',
            (byte)' ',
            (byte)'d',
            (byte)'e',
            (byte)' ',
            (byte)'c',
            0xE1,
            (byte)'m',
        };
        Assert.True(
            pdf.AsSpan().IndexOf(expected) >= 0,
            "Expected 'Kit de cám' encoded in WinAnsi."
        );
        Assert.True(
            pdf.AsSpan().IndexOf(new byte[] { (byte)' ', 0x96, (byte)' ' }) >= 0,
            "Expected an en dash encoded as 0x96."
        );
    }

    [Fact]
    public void Rendering_is_deterministic()
    {
        var invoice = SampleInvoices.Issue();

        Assert.Equal(renderer.Render(invoice), renderer.Render(invoice));
    }

    [Fact]
    public void Long_invoices_continue_on_further_pages()
    {
        var text = Latin1.GetString(renderer.Render(SampleInvoices.Issue(lineCount: 90)));

        var pages = Regex.Count(text, @"/Type /Page\b(?!s)");
        Assert.True(pages >= 3, $"Expected at least 3 pages, found {pages}.");
        Assert.Contains("(Line 90)", text, StringComparison.Ordinal);
    }
}
