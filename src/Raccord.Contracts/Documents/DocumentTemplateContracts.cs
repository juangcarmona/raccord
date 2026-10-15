#pragma warning disable CS1591
namespace Raccord.Contracts.Documents;

public sealed record CreateDocumentTemplateRequest(
    string Name,
    string DocumentType,
    string Letterhead,
    string BodyTemplate,
    string Footer,
    string FontFamily,
    string AccentColor,
    string? LogoStorageKey,
    bool IsDefault
);

public sealed record UpdateDocumentTemplateRequest(
    string Name,
    string Letterhead,
    string BodyTemplate,
    string Footer,
    string FontFamily,
    string AccentColor,
    string? LogoStorageKey,
    bool IsDefault
);

public sealed record DocumentTemplateResponse(
    Guid Id,
    string Name,
    string DocumentType,
    string Letterhead,
    string BodyTemplate,
    string Footer,
    string FontFamily,
    string AccentColor,
    string? LogoStorageKey,
    bool IsDefault,
    DateTimeOffset UpdatedAt
);

public sealed record DocumentTemplatePreviewResponse(Guid Id, string DocumentType, string Html);
#pragma warning restore CS1591
