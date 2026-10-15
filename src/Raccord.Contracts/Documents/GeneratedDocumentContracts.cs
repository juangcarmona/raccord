#pragma warning disable CS1591
namespace Raccord.Contracts.Documents;

public sealed record GeneratedProjectDocumentResponse(
    Guid ProjectId,
    string DocumentType,
    string FileName,
    string ContentType,
    string Html
);

public sealed record EquipmentLabelResponse(
    Guid EquipmentId,
    string Code,
    string Name,
    string? ScanCode,
    string FileName,
    string ContentType,
    string Html
);
#pragma warning restore CS1591
