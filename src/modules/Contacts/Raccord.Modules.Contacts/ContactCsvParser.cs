using System.Text;

namespace Raccord.Modules.Contacts;

internal static class ContactCsvParser
{
    internal static List<List<string>> Parse(byte[] content)
    {
        var text = Decode(content);
        if (text.StartsWith('\uFEFF'))
        {
            text = text[1..];
        }

        var parser = new CsvReader();
        for (var index = 0; index < text.Length; index++)
        {
            if (parser.Read(text[index], index + 1 < text.Length ? text[index + 1] : null))
            {
                index++;
            }
        }

        return parser.Finish();
    }

    private static string Decode(byte[] content)
    {
        try
        {
            return new UTF8Encoding(false, true).GetString(content);
        }
        catch (DecoderFallbackException)
        {
            throw new ContactImportFormatException(
                "contacts.import.csv-encoding-invalid",
                "The CSV file must use UTF-8 text."
            );
        }
    }

    private sealed class CsvReader
    {
        private readonly List<List<string>> rows = [];
        private List<string> row = [];
        private readonly StringBuilder field = new();
        private bool quoted;
        private bool afterQuote;

        internal bool Read(char current, char? next)
        {
            if (quoted)
            {
                return ReadQuoted(current, next);
            }

            if (current == '"')
            {
                if (field.Length > 0 || afterQuote)
                {
                    throw Malformed();
                }

                quoted = true;
                return false;
            }

            if (afterQuote && current is not ',' and not '\r' and not '\n')
            {
                throw Malformed();
            }

            if (current == ',')
            {
                CommitField();
                afterQuote = false;
                return false;
            }

            if (current is '\r' or '\n')
            {
                CommitRow();
                return current == '\r' && next == '\n';
            }

            field.Append(current);
            return false;
        }

        private bool ReadQuoted(char current, char? next)
        {
            if (current != '"')
            {
                field.Append(current);
                return false;
            }

            if (next == '"')
            {
                field.Append('"');
                return true;
            }

            quoted = false;
            afterQuote = true;
            return false;
        }

        internal List<List<string>> Finish()
        {
            if (quoted)
            {
                throw Malformed();
            }

            if (field.Length > 0 || row.Count > 0 || afterQuote)
            {
                CommitRow();
            }

            if (rows.Count > 0 && rows[^1].All(string.IsNullOrWhiteSpace))
            {
                rows.RemoveAt(rows.Count - 1);
            }

            return rows;
        }

        private void CommitField()
        {
            row.Add(field.ToString());
            field.Clear();
            if (row.Count > 64)
            {
                throw TooManyColumns();
            }
        }

        private void CommitRow()
        {
            CommitField();
            rows.Add(row);
            row = [];
            afterQuote = false;
        }

        private static ContactImportFormatException Malformed() =>
            new("contacts.import.csv-malformed", "The CSV contains malformed quoted fields.");

        private static ContactImportFormatException TooManyColumns() =>
            new(
                "contacts.import.columns-exceeded",
                "The spreadsheet contains more than 64 columns."
            );
    }
}
