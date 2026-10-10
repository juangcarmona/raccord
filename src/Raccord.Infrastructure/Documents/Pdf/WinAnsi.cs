using System.Globalization;
using System.Text;

namespace Raccord.Infrastructure.Documents.Pdf;

/// <summary>WinAnsi encoding and Helvetica metrics for the PDF standard Type 1 fonts.</summary>
internal static class WinAnsi
{
    // Advance widths (1/1000 em) of characters 32..126 in Helvetica and Helvetica-Bold (Adobe core font metrics).
    private static readonly short[] RegularAscii =
    [
        278,
        278,
        355,
        556,
        556,
        889,
        667,
        191,
        333,
        333,
        389,
        584,
        278,
        333,
        278,
        278,
        556,
        556,
        556,
        556,
        556,
        556,
        556,
        556,
        556,
        556,
        278,
        278,
        584,
        584,
        584,
        556,
        1015,
        667,
        667,
        722,
        722,
        667,
        611,
        778,
        722,
        278,
        500,
        667,
        556,
        833,
        722,
        778,
        667,
        778,
        722,
        667,
        611,
        722,
        667,
        944,
        667,
        667,
        611,
        278,
        278,
        278,
        469,
        556,
        333,
        556,
        556,
        500,
        556,
        556,
        278,
        556,
        556,
        222,
        222,
        500,
        222,
        833,
        556,
        556,
        556,
        556,
        333,
        500,
        278,
        556,
        500,
        722,
        500,
        500,
        500,
        334,
        260,
        334,
        584,
    ];

    private static readonly short[] BoldAscii =
    [
        278,
        333,
        474,
        556,
        556,
        889,
        722,
        238,
        333,
        333,
        389,
        584,
        278,
        333,
        278,
        278,
        556,
        556,
        556,
        556,
        556,
        556,
        556,
        556,
        556,
        556,
        333,
        333,
        584,
        584,
        584,
        611,
        975,
        722,
        722,
        722,
        722,
        667,
        611,
        778,
        722,
        278,
        556,
        722,
        611,
        833,
        722,
        778,
        667,
        778,
        722,
        667,
        611,
        722,
        667,
        944,
        667,
        667,
        611,
        333,
        278,
        333,
        584,
        556,
        333,
        556,
        611,
        556,
        611,
        556,
        333,
        611,
        611,
        278,
        278,
        556,
        278,
        889,
        611,
        611,
        611,
        611,
        389,
        556,
        333,
        611,
        556,
        778,
        556,
        556,
        500,
        389,
        280,
        389,
        584,
    ];

    private static readonly Dictionary<char, byte> Specials = new()
    {
        ['€'] = 0x80,
        ['‚'] = 0x82,
        ['ƒ'] = 0x83,
        ['„'] = 0x84,
        ['…'] = 0x85,
        ['†'] = 0x86,
        ['‡'] = 0x87,
        ['ˆ'] = 0x88,
        ['‰'] = 0x89,
        ['Š'] = 0x8A,
        ['‹'] = 0x8B,
        ['Œ'] = 0x8C,
        ['Ž'] = 0x8E,
        ['‘'] = 0x91,
        ['’'] = 0x92,
        ['“'] = 0x93,
        ['”'] = 0x94,
        ['•'] = 0x95,
        ['–'] = 0x96,
        ['—'] = 0x97,
        ['˜'] = 0x98,
        ['™'] = 0x99,
        ['š'] = 0x9A,
        ['›'] = 0x9B,
        ['œ'] = 0x9C,
        ['ž'] = 0x9E,
        ['Ÿ'] = 0x9F,
    };

    /// <summary>Encodes text as WinAnsi bytes; characters outside the code page become '?'.</summary>
    public static byte[] Encode(string text)
    {
        var bytes = new byte[text.Length];
        for (var i = 0; i < text.Length; i++)
        {
            var c = text[i];
            bytes[i] = c switch
            {
                >= ' ' and <= '~' => (byte)c,
                >= ' ' and <= 'ÿ' => (byte)c,
                _ when Specials.TryGetValue(c, out var special) => special,
                _ => (byte)'?',
            };
        }

        return bytes;
    }

    /// <summary>Width of the text in points.</summary>
    public static double Measure(string text, bool bold, double fontSize)
    {
        var table = bold ? BoldAscii : RegularAscii;
        var units = 0;
        foreach (var c in text)
        {
            units += CharWidth(c, table);
        }

        return units * fontSize / 1000d;
    }

    private static int CharWidth(char c, short[] table)
    {
        if (c is >= ' ' and <= '~')
        {
            return table[c - ' '];
        }

        if (c == '—')
        {
            return 1000;
        }

        // Accented Latin letters share the advance width of their base letter.
        var baseLetter = c.ToString().Normalize(NormalizationForm.FormD)[0];
        return
            baseLetter is >= ' ' and <= '~'
            && CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.OtherSymbol
            ? table[baseLetter - ' ']
            : 556;
    }
}
