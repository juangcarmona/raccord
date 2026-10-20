namespace Raccord.Contracts.Contacts;

/// <summary>Limits applied to contact spreadsheet uploads.</summary>
public static class ContactImportLimits
{
    /// <summary>Maximum compressed upload size, in bytes.</summary>
    public const int MaximumFileBytes = 10 * 1024 * 1024;
}

/// <summary>Maps supported contact fields to zero-based spreadsheet columns.</summary>
public sealed record ContactImportMapping(bool HasHeader, IReadOnlyDictionary<string, int> Fields);

/// <summary>A row shown during preview or returned as a rejected import row.</summary>
public sealed record ContactImportRowResponse(
    int RowNumber,
    IReadOnlyDictionary<string, string> Values,
    IReadOnlyList<string> Errors
);

/// <summary>Validated preview of a supported contact spreadsheet.</summary>
public sealed record ContactImportPreviewResponse(
    string Sha256,
    string FileType,
    int TotalRowCount,
    int ValidRowCount,
    int RejectedRowCount,
    IReadOnlyList<string> Headers,
    IReadOnlyList<ContactImportRowResponse> Rows
);

/// <summary>Column names and sample rows shown before the user maps fields.</summary>
public sealed record ContactImportInspectionResponse(
    string FileType,
    IReadOnlyList<string> Headers,
    IReadOnlyList<IReadOnlyList<string>> SampleRows
);

/// <summary>Outcome of importing valid rows from a contact spreadsheet.</summary>
public sealed record ContactImportResultResponse(
    Guid ImportId,
    int AcceptedRowCount,
    int RejectedRowCount,
    IReadOnlyList<ContactImportRowResponse> RejectedRows
);
