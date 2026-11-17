namespace Raccord.Modules.Contacts;

internal sealed record ContactImportParseResult(
    string FileType,
    string Sha256,
    IReadOnlyList<string> Headers,
    IReadOnlyList<ContactImportParsedRow> Rows
);
