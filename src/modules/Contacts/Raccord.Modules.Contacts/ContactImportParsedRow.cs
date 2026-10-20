namespace Raccord.Modules.Contacts;

internal sealed record ContactImportParsedRow(
    int RowNumber,
    IReadOnlyDictionary<string, string> Values
);
