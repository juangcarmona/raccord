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
    public void The_document_Should_be_a_structurally_valid_pdf()
    {
        var pdf = renderer.Render(SampleInvoices.Issue());
        var text = Latin1.GetString(pdf);

        text.ShouldStartWith("%PDF-1.7", Case.Sensitive);
        text.ShouldEndWith("%%EOF\n", Case.Sensitive);

        var startXref = int.Parse(
            Regex.Match(text, @"startxref\n(\d+)\n%%EOF\n$").Groups[1].Value,
            CultureInfo.InvariantCulture
        );
        text[startXref..].ShouldStartWith("xref", Case.Sensitive);

        var offsets = Regex
            .Matches(
                text[startXref..],
                @"^(?<offset>\d{10}) 00000 n ?$",
                RegexOptions.Multiline | RegexOptions.ExplicitCapture
            )
            .Select(match => int.Parse(match.Groups["offset"].Value, CultureInfo.InvariantCulture))
            .ToList();
        offsets.ShouldNotBeEmpty();
        for (var objectNumber = 1; objectNumber <= offsets.Count; objectNumber++)
        {
            text[offsets[objectNumber - 1]..]
                .ShouldStartWith($"{objectNumber} 0 obj", Case.Sensitive);
        }
    }

    [Fact]
    public void The_document_Should_show_number_parties_and_spanish_formatted_totals()
    {
        var text = Latin1.GetString(renderer.Render(SampleInvoices.Issue()));

        text.ShouldContain("(F2026-00001)", Case.Sensitive);
        text.ShouldContain("(Productora Ejemplo S.A.)", Case.Sensitive);
        text.ShouldContain("(NIF: A00000000)", Case.Sensitive);
        text.Replace("\u0080", "€", StringComparison.Ordinal)
            .ShouldContain("(1.548,80 €)", Case.Sensitive);
        text.ShouldContain("(11/05/2026)", Case.Sensitive);
    }

    [Fact]
    public void Non_ascii_text_Should_be_encoded_as_single_win_ansi_bytes()
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
        pdf.AsSpan().IndexOf(expected).ShouldBeGreaterThanOrEqualTo(0);
        pdf.AsSpan()
            .IndexOf(new byte[] { (byte)' ', 0x96, (byte)' ' })
            .ShouldBeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public void Rendering_Should_be_deterministic()
    {
        var invoice = SampleInvoices.Issue();

        renderer.Render(invoice).ShouldBe(renderer.Render(invoice));
    }

    [Fact]
    public void Long_invoices_Should_continue_on_further_pages()
    {
        var text = Latin1.GetString(renderer.Render(SampleInvoices.Issue(lineCount: 90)));

        var pages = Regex.Count(text, @"/Type /Page\b(?!s)");
        pages.ShouldBeGreaterThanOrEqualTo(3);
        text.ShouldContain("(Line 90)", Case.Sensitive);
    }
}
