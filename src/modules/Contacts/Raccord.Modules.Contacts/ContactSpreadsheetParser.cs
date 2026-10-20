using System.Security.Cryptography;
using System.Text;
using Raccord.Contracts.Contacts;

namespace Raccord.Modules.Contacts;

internal static class ContactSpreadsheetParser
{
    private const int MaximumColumns = 64;

    internal static ContactImportInspectionResponse Inspect(
        byte[] content,
        string fileName,
        bool hasHeader
    )
    {
        ArgumentNullException.ThrowIfNull(content);
        if (content.Length > ContactImportLimits.MaximumFileBytes)
        {
            throw new ContactImportFormatException(
                "contacts.import.file-too-large",
                "The spreadsheet exceeds the 10 MiB upload limit."
            );
        }

        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        var rows = extension switch
        {
            ".csv" => ContactCsvParser.Parse(content),
            ".xlsx" => ContactXlsxParser.Parse(content),
            _ => throw new ContactImportFormatException(
                "contacts.import.file-type-unsupported",
                "Upload a CSV or XLSX spreadsheet."
            ),
        };
        var headers = hasHeader
            ? rows.Count > 0
                ? rows[0]
                : []
            : Enumerable
                .Range(0, rows.Count == 0 ? 0 : rows.Max(row => row.Count))
                .Select(ColumnLabel)
                .ToList();
        if (hasHeader)
        {
            ValidateHeaders(headers);
            rows.RemoveAt(0);
        }

        return new ContactImportInspectionResponse(
            extension[1..],
            headers,
            rows.Take(10).Select(row => (IReadOnlyList<string>)row).ToArray()
        );
    }

    internal static ContactImportParseResult Parse(
        byte[] content,
        string fileName,
        ContactImportMapping mapping
    )
    {
        ArgumentNullException.ThrowIfNull(content);
        ArgumentNullException.ThrowIfNull(mapping);
        if (content.Length > ContactImportLimits.MaximumFileBytes)
        {
            throw new ContactImportFormatException(
                "contacts.import.file-too-large",
                "The spreadsheet exceeds the 10 MiB upload limit."
            );
        }

        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        if (extension is not ".csv" and not ".xlsx")
        {
            throw new ContactImportFormatException(
                "contacts.import.file-type-unsupported",
                "Upload a CSV or XLSX spreadsheet."
            );
        }

        ValidateMapping(mapping);
        var rows = extension switch
        {
            ".csv" => ContactCsvParser.Parse(content),
            ".xlsx" => ContactXlsxParser.Parse(content),
            _ => throw new InvalidOperationException(
                "The supported file extension was not handled."
            ),
        };

        var headers = mapping.HasHeader && rows.Count > 0 ? rows[0] : [];
        if (mapping.HasHeader)
        {
            ValidateHeaders(headers);
            if (mapping.Fields.Values.Any(column => column >= headers.Count))
            {
                throw InvalidMapping();
            }

            rows.RemoveAt(0);
        }

        var parsedRows = MapRows(rows, mapping);

        return new ContactImportParseResult(
            extension[1..],
            Convert.ToHexString(SHA256.HashData(content)).ToLowerInvariant(),
            mapping.HasHeader ? headers : [],
            parsedRows
        );
    }

    private static List<ContactImportParsedRow> MapRows(
        List<List<string>> rows,
        ContactImportMapping mapping
    )
    {
        var parsedRows = new List<ContactImportParsedRow>(rows.Count);
        for (var index = 0; index < rows.Count; index++)
        {
            var values = rows[index];
            if (values.All(string.IsNullOrWhiteSpace))
            {
                continue;
            }

            var mapped = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var field in mapping.Fields)
            {
                mapped.Add(
                    field.Key,
                    field.Value < values.Count ? values[field.Value].Trim() : string.Empty
                );
            }

            parsedRows.Add(
                new ContactImportParsedRow(mapping.HasHeader ? index + 2 : index + 1, mapped)
            );
        }

        return parsedRows;
    }

    private static void ValidateMapping(ContactImportMapping mapping)
    {
        if (mapping.Fields is null)
        {
            throw InvalidMapping();
        }

        var supported = new HashSet<string>(
            [
                "type",
                "legalName",
                "contactNumber",
                "taxId",
                "street",
                "postalCode",
                "city",
                "province",
                "countryCode",
                "email",
                "roles",
            ],
            StringComparer.Ordinal
        );
        if (
            !mapping.Fields.ContainsKey("type")
            || !mapping.Fields.ContainsKey("legalName")
            || !mapping.Fields.ContainsKey("roles")
            || mapping.Fields.Any(field =>
                !supported.Contains(field.Key) || field.Value is < 0 or >= MaximumColumns
            )
            || mapping.Fields.Values.Distinct().Count() != mapping.Fields.Count
        )
        {
            throw InvalidMapping();
        }
    }

    private static ContactImportFormatException InvalidMapping() =>
        new(
            "contacts.import.mapping-invalid",
            "Map the required contact fields to distinct spreadsheet columns."
        );

    private static string ColumnLabel(int index)
    {
        var label = new StringBuilder();
        var value = index + 1;
        while (value > 0)
        {
            value--;
            label.Insert(0, (char)('A' + value % 26));
            value /= 26;
        }

        return $"Column {label}";
    }

    private static void ValidateHeaders(List<string> headers)
    {
        if (headers.Count == 0)
        {
            throw new ContactImportFormatException(
                "contacts.import.headers-invalid",
                "The spreadsheet must contain a header row when header mapping is selected."
            );
        }

        if (headers.Count > MaximumColumns)
        {
            throw new ContactImportFormatException(
                "contacts.import.columns-exceeded",
                "The spreadsheet contains more than 64 columns."
            );
        }

        var nonBlank = headers.Select(header => header.Trim()).ToArray();
        if (
            nonBlank.Any(string.IsNullOrWhiteSpace)
            || nonBlank.Distinct(StringComparer.OrdinalIgnoreCase).Count() != nonBlank.Length
        )
        {
            throw new ContactImportFormatException(
                "contacts.import.headers-invalid",
                "Header names must be non-empty and unique."
            );
        }
    }
}
