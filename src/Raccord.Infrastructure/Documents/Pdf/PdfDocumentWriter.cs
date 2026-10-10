using System.Globalization;
using System.Text;

namespace Raccord.Infrastructure.Documents.Pdf;

/// <summary>
/// Serializes pages into a minimal PDF 1.7 file using the standard Helvetica fonts.
/// The output contains no timestamps or random identifiers, so equal input gives equal bytes.
/// </summary>
internal static class PdfDocumentWriter
{
    private const int FirstPageObject = 6;

    public static byte[] Write(IReadOnlyList<PdfPage> pages, string title)
    {
        using var output = new MemoryStream();
        var offsets = new List<long>();

        WriteAscii(output, "%PDF-1.7\n%âãÏÓ\n");

        var kids = string.Join(
            ' ',
            Enumerable
                .Range(0, pages.Count)
                .Select(index => Invariant($"{FirstPageObject + (index * 2)} 0 R"))
        );
        WriteObject(output, offsets, "<< /Type /Catalog /Pages 2 0 R >>");
        WriteObject(
            output,
            offsets,
            Invariant($"<< /Type /Pages /Kids [{kids}] /Count {pages.Count} >>")
        );
        WriteObject(
            output,
            offsets,
            "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica /Encoding /WinAnsiEncoding >>"
        );
        WriteObject(
            output,
            offsets,
            "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica-Bold /Encoding /WinAnsiEncoding >>"
        );
        WriteInfo(output, offsets, title);

        for (var index = 0; index < pages.Count; index++)
        {
            var contentObject = FirstPageObject + (index * 2) + 1;
            WriteObject(
                output,
                offsets,
                Invariant(
                    $"<< /Type /Page /Parent 2 0 R /MediaBox [0 0 {PdfPage.Width:0.##} {PdfPage.Height:0.##}] /Resources << /Font << /F1 3 0 R /F2 4 0 R >> >> /Contents {contentObject} 0 R >>"
                )
            );
            WriteStream(output, offsets, pages[index].ToArray());
        }

        var xref = output.Position;
        var table = new StringBuilder();
        table.Append(Invariant($"xref\n0 {offsets.Count + 1}\n0000000000 65535 f \n"));
        foreach (var offset in offsets)
        {
            table.Append(Invariant($"{offset:D10} 00000 n \n"));
        }

        table.Append(
            Invariant(
                $"trailer\n<< /Size {offsets.Count + 1} /Root 1 0 R /Info 5 0 R >>\nstartxref\n{xref}\n%%EOF\n"
            )
        );
        WriteAscii(output, table.ToString());
        return output.ToArray();
    }

    private static void WriteInfo(MemoryStream output, List<long> offsets, string title)
    {
        offsets.Add(output.Position);
        WriteAscii(output, Invariant($"{offsets.Count} 0 obj\n<< /Producer (Raccord) /Title ("));
        foreach (var b in WinAnsi.Encode(title))
        {
            if (b is (byte)'(' or (byte)')' or (byte)'\\')
            {
                output.WriteByte((byte)'\\');
            }

            output.WriteByte(b);
        }

        WriteAscii(output, ") >>\nendobj\n");
    }

    private static void WriteObject(MemoryStream output, List<long> offsets, string body)
    {
        offsets.Add(output.Position);
        WriteAscii(output, Invariant($"{offsets.Count} 0 obj\n{body}\nendobj\n"));
    }

    private static void WriteStream(MemoryStream output, List<long> offsets, byte[] content)
    {
        offsets.Add(output.Position);
        WriteAscii(
            output,
            Invariant($"{offsets.Count} 0 obj\n<< /Length {content.Length} >>\nstream\n")
        );
        output.Write(content);
        WriteAscii(output, "\nendstream\nendobj\n");
    }

    private static void WriteAscii(MemoryStream output, string text) =>
        output.Write(Encoding.Latin1.GetBytes(text));

    private static string Invariant(FormattableString value) =>
        value.ToString(CultureInfo.InvariantCulture);
}
