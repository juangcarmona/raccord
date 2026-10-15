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
    bool IsDefault,
    string Language = "en"
);

public sealed record UpdateDocumentTemplateRequest(
    string Name,
    string Letterhead,
    string BodyTemplate,
    string Footer,
    string FontFamily,
    string AccentColor,
    string? LogoStorageKey,
    bool IsDefault,
    string? Language = null
);

public sealed record DocumentTemplateResponse(
    Guid Id,
    string Name,
    string DocumentType,
    string Language,
    string Letterhead,
    string BodyTemplate,
    string Footer,
    string FontFamily,
    string AccentColor,
    string? LogoStorageKey,
    bool IsDefault,
    DateTimeOffset UpdatedAt
);

public sealed record DocumentTemplatePreviewResponse(
    Guid Id,
    string DocumentType,
    string Language,
    string Html
);
#pragma warning restore CS1591
