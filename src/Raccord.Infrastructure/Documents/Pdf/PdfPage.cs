using System.Buffers;
using System.Globalization;
using System.Text;

namespace Raccord.Infrastructure.Documents.Pdf;

/// <summary>Drawing operations of one A4 page, in PDF points from the bottom-left corner.</summary>
internal sealed class PdfPage
{
    public const double Width = 595.28;
    public const double Height = 841.89;

    private readonly ArrayBufferWriter<byte> content = new();

    /// <summary>Draws left-aligned text.</summary>
    public void Text(double x, double y, string text, double size, bool bold = false)
    {
        Write(string.Create(CultureInfo.InvariantCulture, $"BT /{(bold ? "F2" : "F1")} {Number(size)} Tf {Number(x)} {Number(y)} Td ("));
        WriteEscaped(text);
        Write(") Tj ET\n");
    }

    /// <summary>Draws text whose right edge is at <paramref name="right"/>.</summary>
    public void TextRight(double right, double y, string text, double size, bool bold = false) =>
        Text(right - WinAnsi.Measure(text, bold, size), y, text, size, bold);

    /// <summary>Draws a horizontal rule.</summary>
    public void Rule(double x1, double x2, double y, double thickness = 0.5) =>
        Write(string.Create(CultureInfo.InvariantCulture, $"{Number(thickness)} w {Number(x1)} {Number(y)} m {Number(x2)} {Number(y)} l S\n"));

    /// <summary>Fills a light grey band.</summary>
    public void Band(double x, double y, double width, double height) =>
        Write(string.Create(CultureInfo.InvariantCulture, $"0.94 g {Number(x)} {Number(y)} {Number(width)} {Number(height)} re f 0 g\n"));

    /// <summary>The content stream bytes.</summary>
    public byte[] ToArray() => content.WrittenSpan.ToArray();

    private static string Number(double value) => value.ToString("0.##", CultureInfo.InvariantCulture);

    private void Write(string ascii) => content.Write(Encoding.ASCII.GetBytes(ascii));

    private void WriteEscaped(string text)
    {
        foreach (var b in WinAnsi.Encode(text))
        {
            if (b is (byte)'(' or (byte)')' or (byte)'\\')
            {
                content.Write("\\"u8);
            }

            content.Write([b]);
        }
    }
}
