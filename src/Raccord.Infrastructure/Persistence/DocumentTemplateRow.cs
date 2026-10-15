namespace Raccord.Infrastructure.Persistence;

internal sealed class DocumentTemplateRow
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty;
    public string Language { get; set; } = "en";
    public string Letterhead { get; set; } = string.Empty;
    public string BodyTemplate { get; set; } = string.Empty;
    public string Footer { get; set; } = string.Empty;
    public string FontFamily { get; set; } = string.Empty;
    public string AccentColor { get; set; } = string.Empty;
    public string? LogoStorageKey { get; set; }
    public bool IsDefault { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
