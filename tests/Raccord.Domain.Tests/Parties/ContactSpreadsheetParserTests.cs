using System.IO.Compression;
using System.Text;
using Raccord.Contracts.Contacts;
using Raccord.Modules.Contacts;
using Shouldly;
using Xunit;

namespace Raccord.Domain.Tests.Parties;

public sealed class ContactSpreadsheetParserTests
{
    [Fact]
    public void Contact_import_Should_ParseQuotedCsvAndOptionalHeader()
    {
        var bytes = Encoding.UTF8.GetBytes(
            "type,name,roles,email\r\ncompany,\"Northlight, Films\",customer,hello@example.test\r\n"
        );
        var mapping = new ContactImportMapping(
            HasHeader: true,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["type"] = 0,
                ["legalName"] = 1,
                ["roles"] = 2,
                ["email"] = 3,
            }
        );

        var result = ContactSpreadsheetParser.Parse(bytes, "contacts.csv", mapping);

        result.FileType.ShouldBe("csv");
        result.Rows.Count.ShouldBe(1);
        result.Rows[0].RowNumber.ShouldBe(2);
        result.Rows[0].Values["legalName"].ShouldBe("Northlight, Films");
        result.Sha256.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public void Contact_import_Should_ParseCsvWithoutAHeader()
    {
        var bytes = Encoding.UTF8.GetBytes("company,Northlight Films,customer\n");
        var mapping = new ContactImportMapping(
            HasHeader: false,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["type"] = 0,
                ["legalName"] = 1,
                ["roles"] = 2,
            }
        );

        var result = ContactSpreadsheetParser.Parse(bytes, "contacts.csv", mapping);

        result.Rows.Count.ShouldBe(1);
        result.Rows[0].RowNumber.ShouldBe(1);
        result.Rows[0].Values["legalName"].ShouldBe("Northlight Films");
    }

    [Fact]
    public void Contact_import_Should_RejectMalformedCsvBeforeCreatingRows()
    {
        var bytes = Encoding.UTF8.GetBytes("company,\"unterminated,customer\n");
        var mapping = new ContactImportMapping(
            HasHeader: false,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["type"] = 0,
                ["legalName"] = 1,
                ["roles"] = 2,
            }
        );

        var error = Should.Throw<ContactImportFormatException>(() =>
            ContactSpreadsheetParser.Parse(bytes, "contacts.csv", mapping)
        );

        error.Code.ShouldBe("contacts.import.csv-malformed");
    }

    [Theory]
    [InlineData("type,,roles\ncompany,name,customer\n")]
    [InlineData("type,type,roles\ncompany,name,customer\n")]
    public void Contact_import_Should_RejectBlankOrDuplicateHeaders(string csv)
    {
        var mapping = new ContactImportMapping(
            HasHeader: true,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["type"] = 0,
                ["legalName"] = 1,
                ["roles"] = 2,
            }
        );

        var error = Should.Throw<ContactImportFormatException>(() =>
            ContactSpreadsheetParser.Parse(Encoding.UTF8.GetBytes(csv), "contacts.csv", mapping)
        );

        error.Code.ShouldBe("contacts.import.headers-invalid");
    }

    [Fact]
    public void Contact_import_Should_RejectMoreThanSixtyFourColumns()
    {
        var bytes = Encoding.UTF8.GetBytes(
            string.Join(',', Enumerable.Range(0, 65).Select(i => $"c{i}"))
        );
        var mapping = new ContactImportMapping(
            HasHeader: false,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["type"] = 0,
                ["legalName"] = 1,
                ["roles"] = 2,
            }
        );

        var error = Should.Throw<ContactImportFormatException>(() =>
            ContactSpreadsheetParser.Parse(bytes, "contacts.csv", mapping)
        );

        error.Code.ShouldBe("contacts.import.columns-exceeded");
    }

    [Fact]
    public void Contact_import_Should_RejectUnsupportedFileType()
    {
        var error = Should.Throw<ContactImportFormatException>(() =>
            ContactSpreadsheetParser.Parse(
                [1, 2, 3],
                "contacts.xls",
                new(false, new Dictionary<string, int>(StringComparer.Ordinal))
            )
        );

        error.Code.ShouldBe("contacts.import.file-type-unsupported");
    }

    [Fact]
    public void Contact_import_Should_RejectXlsxWithMoreThanOneWorksheet()
    {
        var bytes = CreateXlsx(sheetCount: 2);
        var mapping = new ContactImportMapping(
            HasHeader: false,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["type"] = 0,
                ["legalName"] = 1,
                ["roles"] = 2,
            }
        );

        var error = Should.Throw<ContactImportFormatException>(() =>
            ContactSpreadsheetParser.Parse(bytes, "contacts.xlsx", mapping)
        );

        error.Code.ShouldBe("contacts.import.worksheet-count-invalid");
    }

    [Fact]
    public void Contact_import_Should_ParsePlainTextXlsxRows()
    {
        var bytes = CreateXlsx(
            sheetCount: 1,
            worksheetRows: InlineTextRow("Company", "Northlight", "Customer")
        );
        var mapping = new ContactImportMapping(
            HasHeader: false,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["type"] = 0,
                ["legalName"] = 1,
                ["roles"] = 2,
            }
        );

        var result = ContactSpreadsheetParser.Parse(bytes, "contacts.xlsx", mapping);

        result.FileType.ShouldBe("xlsx");
        result.Rows.Single().Values["legalName"].ShouldBe("Northlight");
    }

    [Fact]
    public void Contact_import_Should_RejectFormulaCellsInXlsx()
    {
        var bytes = CreateXlsx(
            sheetCount: 1,
            worksheetRows: "<row r=\"1\"><c r=\"A1\"><f>1+1</f><v>2</v></c></row>"
        );
        var mapping = new ContactImportMapping(
            HasHeader: false,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["type"] = 0,
                ["legalName"] = 1,
                ["roles"] = 2,
            }
        );

        var error = Should.Throw<ContactImportFormatException>(() =>
            ContactSpreadsheetParser.Parse(bytes, "contacts.xlsx", mapping)
        );

        error.Code.ShouldBe("contacts.import.xlsx-content-unsupported");
    }

    [Fact]
    public void Contact_import_Should_RejectNumericXlsxCells()
    {
        var bytes = CreateXlsx(
            sheetCount: 1,
            worksheetRows: "<row r=\"1\"><c r=\"A1\"><v>42</v></c></row>"
        );
        var mapping = new ContactImportMapping(
            HasHeader: false,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["type"] = 0,
                ["legalName"] = 1,
                ["roles"] = 2,
            }
        );

        var error = Should.Throw<ContactImportFormatException>(() =>
            ContactSpreadsheetParser.Parse(bytes, "contacts.xlsx", mapping)
        );

        error.Code.ShouldBe("contacts.import.xlsx-content-unsupported");
    }

    [Theory]
    [InlineData("xl/media/image1.png")]
    [InlineData("xl/vbaProject.bin")]
    public void Contact_import_Should_RejectImagesAndMacrosInXlsx(string entryName)
    {
        var bytes = CreateXlsx(sheetCount: 1, unsupportedEntry: entryName);
        var mapping = new ContactImportMapping(
            HasHeader: false,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["type"] = 0,
                ["legalName"] = 1,
                ["roles"] = 2,
            }
        );

        var error = Should.Throw<ContactImportFormatException>(() =>
            ContactSpreadsheetParser.Parse(bytes, "contacts.xlsx", mapping)
        );

        error.Code.ShouldBe("contacts.import.xlsx-content-unsupported");
    }

    [Fact]
    public void Contact_import_Should_RejectMalformedXlsxArchive()
    {
        var error = Should.Throw<ContactImportFormatException>(() =>
            ContactSpreadsheetParser.Parse(
                [1, 2, 3],
                "contacts.xlsx",
                new(
                    false,
                    new Dictionary<string, int>(StringComparer.Ordinal)
                    {
                        ["type"] = 0,
                        ["legalName"] = 1,
                        ["roles"] = 2,
                    }
                )
            )
        );

        error.Code.ShouldBe("contacts.import.xlsx-malformed");
    }

    [Fact]
    public void Contact_import_Should_RejectFilesOverTenMebibytes()
    {
        var error = Should.Throw<ContactImportFormatException>(() =>
            ContactSpreadsheetParser.Parse(
                new byte[ContactImportLimits.MaximumFileBytes + 1],
                "contacts.csv",
                new(false, new Dictionary<string, int>(StringComparer.Ordinal))
            )
        );

        error.Code.ShouldBe("contacts.import.file-too-large");
    }

    private static byte[] CreateXlsx(
        int sheetCount,
        string worksheetRows = "",
        string? unsupportedEntry = null
    )
    {
        using var output = new MemoryStream();
        using (var archive = new ZipArchive(output, ZipArchiveMode.Create, leaveOpen: true))
        {
            Write(
                archive,
                "xl/workbook.xml",
                $"<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\"><sheets>{string.Concat(Enumerable.Range(1, sheetCount).Select(i => $"<sheet name=\"Sheet{i}\" sheetId=\"{i}\" r:id=\"rId{i}\"/>"))}</sheets></workbook>"
            );
            Write(
                archive,
                "xl/_rels/workbook.xml.rels",
                $"<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">{string.Concat(Enumerable.Range(1, sheetCount).Select(i => $"<Relationship Id=\"rId{i}\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" Target=\"worksheets/sheet{i}.xml\"/>"))}</Relationships>"
            );
            Write(
                archive,
                "xl/worksheets/sheet1.xml",
                $"<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\"><sheetData>{worksheetRows}</sheetData></worksheet>"
            );
            if (sheetCount > 1)
            {
                Write(
                    archive,
                    "xl/worksheets/sheet2.xml",
                    "<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\"><sheetData/></worksheet>"
                );
            }

            if (unsupportedEntry is not null)
            {
                Write(archive, unsupportedEntry, "unsupported");
            }
        }

        return output.ToArray();

        static void Write(ZipArchive archive, string path, string contents)
        {
            using var writer = new StreamWriter(archive.CreateEntry(path).Open());
            writer.Write(contents);
        }
    }

    private static string InlineTextRow(params string[] values) =>
        $"<row r=\"1\">{string.Concat(values.Select((value, index) => $"<c r=\"{(char)('A' + index)}1\" t=\"inlineStr\"><is><t>{value}</t></is></c>"))}</row>";
}
