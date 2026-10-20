using System.Globalization;
using System.IO.Compression;
using System.Xml;
using System.Xml.Linq;

namespace Raccord.Modules.Contacts;

internal static class ContactXlsxParser
{
    private const long MaximumExpandedBytes = 40 * 1024 * 1024;
    private static readonly XNamespace SpreadsheetNs =
        "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
    private static readonly XNamespace RelationshipNs =
        "http://schemas.openxmlformats.org/officeDocument/2006/relationships";
    private static readonly XNamespace PackageRelationshipNs =
        "http://schemas.openxmlformats.org/package/2006/relationships";

    internal static List<List<string>> Parse(byte[] content)
    {
        try
        {
            using var stream = new MemoryStream(content, writable: false);
            using var archive = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: false);
            ValidateArchive(archive);
            var worksheetPath = GetWorksheetPath(archive);
            var sharedStrings = ReadSharedStrings(archive);
            return ReadRows(LoadXml(archive, worksheetPath), sharedStrings);
        }
        catch (ContactImportFormatException)
        {
            throw;
        }
        catch (Exception exception)
            when (exception
                    is InvalidDataException
                        or XmlException
                        or IOException
                        or ArgumentException
                        or InvalidOperationException
            )
        {
            throw new ContactImportFormatException(
                "contacts.import.xlsx-malformed",
                "The XLSX workbook is malformed or unsupported."
            );
        }
    }

    private static void ValidateArchive(ZipArchive archive)
    {
        if (archive.Entries.Sum(entry => entry.Length) > MaximumExpandedBytes)
        {
            throw new ContactImportFormatException(
                "contacts.import.xlsx-expanded-too-large",
                "The expanded spreadsheet exceeds the supported size."
            );
        }

        var hasUnsupportedContent = archive.Entries.Any(entry =>
            entry.FullName.StartsWith("xl/media/", StringComparison.OrdinalIgnoreCase)
            || entry.FullName.EndsWith("vbaProject.bin", StringComparison.OrdinalIgnoreCase)
        );
        if (hasUnsupportedContent)
        {
            throw new ContactImportFormatException(
                "contacts.import.xlsx-content-unsupported",
                "The workbook cannot contain images or macros."
            );
        }
    }

    private static string GetWorksheetPath(ZipArchive archive)
    {
        var workbook = LoadXml(archive, "xl/workbook.xml");
        var sheets =
            workbook
                .Root?.Element(SpreadsheetNs + "sheets")
                ?.Elements(SpreadsheetNs + "sheet")
                .ToArray()
            ?? [];
        if (sheets.Length != 1)
        {
            throw new ContactImportFormatException(
                "contacts.import.worksheet-count-invalid",
                "The workbook must contain exactly one worksheet."
            );
        }

        var relationshipId = (string?)sheets[0].Attribute(RelationshipNs + "id");
        var relationships = LoadXml(archive, "xl/_rels/workbook.xml.rels");
        var relationship = relationships
            .Root?.Elements(PackageRelationshipNs + "Relationship")
            .SingleOrDefault(item =>
                string.Equals(
                    (string?)item.Attribute("Id"),
                    relationshipId,
                    StringComparison.Ordinal
                )
            );
        if (
            relationship is null
            || string.Equals(
                (string?)relationship.Attribute("TargetMode"),
                "External",
                StringComparison.Ordinal
            )
        )
        {
            throw new ContactImportFormatException(
                "contacts.import.xlsx-content-unsupported",
                "The workbook contains an unsupported worksheet reference."
            );
        }

        var target = (string?)relationship.Attribute("Target") ?? string.Empty;
        var path = target.StartsWith('/')
            ? target.TrimStart('/')
            : $"xl/{target.TrimStart('.', '/')}";
        return NormalizeWorksheetPath(path);
    }

    private static List<List<string>> ReadRows(
        XDocument worksheet,
        IReadOnlyList<string> sharedStrings
    )
    {
        var rowElements =
            worksheet
                .Root?.Element(SpreadsheetNs + "sheetData")
                ?.Elements(SpreadsheetNs + "row")
                .ToArray()
            ?? [];
        var result = new List<List<string>>(rowElements.Length);
        foreach (var row in rowElements)
        {
            var rowNumber = GetRowNumber(row, result.Count + 1);
            while (result.Count < rowNumber - 1)
            {
                result.Add([]);
            }

            result.Add(ReadCells(row, sharedStrings));
        }

        return result;
    }

    private static List<string> ReadCells(XElement row, IReadOnlyList<string> sharedStrings)
    {
        var cells = new List<string>();
        foreach (var cell in row.Elements(SpreadsheetNs + "c"))
        {
            var column = ColumnIndex((string?)cell.Attribute("r"));
            if (column >= 64)
            {
                throw new ContactImportFormatException(
                    "contacts.import.columns-exceeded",
                    "The spreadsheet contains more than 64 columns."
                );
            }

            while (cells.Count <= column)
            {
                cells.Add(string.Empty);
            }

            if (cell.Element(SpreadsheetNs + "f") is not null)
            {
                throw new ContactImportFormatException(
                    "contacts.import.xlsx-content-unsupported",
                    "Formula cells are not supported. Replace them with plain text values."
                );
            }

            cells[column] = CellText(cell, sharedStrings);
        }

        return cells;
    }

    private static int GetRowNumber(XElement row, int fallback) =>
        int.TryParse(
            (string?)row.Attribute("r"),
            NumberStyles.None,
            CultureInfo.InvariantCulture,
            out var value
        )
        && value > 0
            ? value
            : fallback;

    private static XDocument LoadXml(ZipArchive archive, string path)
    {
        var entry =
            archive.GetEntry(path)
            ?? throw new ContactImportFormatException(
                "contacts.import.xlsx-malformed",
                "The workbook is missing a required part."
            );
        using var stream = entry.Open();
        using var reader = XmlReader.Create(stream, SecureReaderSettings());
        return XDocument.Load(reader, LoadOptions.None);
    }

    private static string[] ReadSharedStrings(ZipArchive archive)
    {
        var entry = archive.GetEntry("xl/sharedStrings.xml");
        if (entry is null)
        {
            return [];
        }

        using var stream = entry.Open();
        using var reader = XmlReader.Create(stream, SecureReaderSettings());
        return XDocument
                .Load(reader)
                .Root?.Elements(SpreadsheetNs + "si")
                .Select(item =>
                    string.Concat(item.Descendants(SpreadsheetNs + "t").Select(text => text.Value))
                )
                .ToArray()
            ?? [];
    }

    private static XmlReaderSettings SecureReaderSettings() =>
        new()
        {
            DtdProcessing = DtdProcessing.Prohibit,
            XmlResolver = null,
            MaxCharactersInDocument = MaximumExpandedBytes,
            MaxCharactersFromEntities = 0,
        };

    private static string CellText(XElement cell, IReadOnlyList<string> sharedStrings)
    {
        var type = (string?)cell.Attribute("t");
        var value = (string?)cell.Element(SpreadsheetNs + "v");
        if (string.Equals(type, "s", StringComparison.Ordinal))
        {
            return
                int.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out var index)
                && index >= 0
                && index < sharedStrings.Count
                ? sharedStrings[index]
                : throw new ContactImportFormatException(
                    "contacts.import.xlsx-malformed",
                    "The workbook contains an invalid shared string reference."
                );
        }

        if (string.Equals(type, "inlineStr", StringComparison.Ordinal))
        {
            return string.Concat(cell.Descendants(SpreadsheetNs + "t").Select(text => text.Value));
        }

        if (string.Equals(type, "str", StringComparison.Ordinal))
        {
            return value ?? string.Empty;
        }

        if (type is null && string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        throw new ContactImportFormatException(
            "contacts.import.xlsx-content-unsupported",
            "Cells must contain plain text values."
        );
    }

    private static int ColumnIndex(string? cellReference)
    {
        if (string.IsNullOrWhiteSpace(cellReference))
        {
            throw new ContactImportFormatException(
                "contacts.import.xlsx-malformed",
                "The workbook contains a cell without a reference."
            );
        }

        var value = 0;
        foreach (var character in cellReference.TakeWhile(char.IsLetter))
        {
            value = checked(value * 26 + char.ToUpperInvariant(character) - 'A' + 1);
        }

        return value - 1;
    }

    private static string NormalizeWorksheetPath(string path)
    {
        var segments = new List<string>();
        foreach (var segment in path.Replace('\\', '/').Split('/'))
        {
            if (segment is "" or ".")
            {
                continue;
            }

            if (string.Equals(segment, "..", StringComparison.Ordinal))
            {
                if (segments.Count == 0)
                {
                    throw new ContactImportFormatException(
                        "contacts.import.xlsx-content-unsupported",
                        "The workbook references a worksheet outside its package."
                    );
                }

                segments.RemoveAt(segments.Count - 1);
            }
            else
            {
                segments.Add(segment);
            }
        }

        return string.Join('/', segments);
    }
}
