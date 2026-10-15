using System.Text.Encodings.Web;
using Microsoft.EntityFrameworkCore;
using Raccord.Contracts.Documents;
#pragma warning disable CS1591
namespace Raccord.Infrastructure.Persistence;

public sealed class DocumentTemplateCatalog(RaccordDbContext db)
{
    public async Task<IReadOnlyList<DocumentTemplateResponse>> ListAsync(
        string? language,
        CancellationToken ct
    ) =>
        [
            .. (
                await ApplyLanguageFilter(db.DocumentTemplates.AsNoTracking(), language)
                    .OrderBy(t => t.DocumentType)
                    .ThenBy(t => t.Language)
                    .ThenBy(t => t.Name)
                    .ToListAsync(ct)
                    .ConfigureAwait(false)
            ).Select(ToResponse),
        ];

    public async Task<DocumentTemplateResponse?> FindAsync(Guid id, CancellationToken ct) =>
        (
            await db
                .DocumentTemplates.AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, ct)
                .ConfigureAwait(false)
        )
            is { } t
            ? ToResponse(t)
            : null;

    public async Task<DocumentTemplateResponse> CreateAsync(
        CreateDocumentTemplateRequest request,
        DateTimeOffset at,
        CancellationToken ct
    )
    {
        Validate(
            request.Name,
            request.DocumentType,
            request.Letterhead,
            request.BodyTemplate,
            request.FontFamily,
            request.AccentColor
        );
        var language = NormalizeLanguage(request.Language);
        if (request.IsDefault)
            await ClearDefaultAsync(request.DocumentType.Trim().ToLowerInvariant(), language, ct)
                .ConfigureAwait(false);
        var template = new DocumentTemplateRow
        {
            Id = Guid.CreateVersion7(),
            Name = request.Name.Trim(),
            DocumentType = request.DocumentType.Trim().ToLowerInvariant(),
            Language = language,
            Letterhead = request.Letterhead.Trim(),
            BodyTemplate = request.BodyTemplate.Trim(),
            Footer = request.Footer.Trim(),
            FontFamily = request.FontFamily.Trim(),
            AccentColor = request.AccentColor.Trim(),
            LogoStorageKey = request.LogoStorageKey?.Trim(),
            IsDefault = request.IsDefault,
            UpdatedAt = at,
        };
        db.DocumentTemplates.Add(template);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return ToResponse(template);
    }

    public async Task<DocumentTemplateResponse?> UpdateAsync(
        Guid id,
        UpdateDocumentTemplateRequest request,
        DateTimeOffset at,
        CancellationToken ct
    )
    {
        Validate(
            request.Name,
            null,
            request.Letterhead,
            request.BodyTemplate,
            request.FontFamily,
            request.AccentColor
        );
        var template = await db
            .DocumentTemplates.FirstOrDefaultAsync(t => t.Id == id, ct)
            .ConfigureAwait(false);
        if (template is null)
            return null;
        var language = request.Language is null
            ? template.Language
            : NormalizeLanguage(request.Language);
        if (request.IsDefault)
            await ClearDefaultAsync(template.DocumentType, language, ct, id).ConfigureAwait(false);
        template.Name = request.Name.Trim();
        template.Language = language;
        template.Letterhead = request.Letterhead.Trim();
        template.BodyTemplate = request.BodyTemplate.Trim();
        template.Footer = request.Footer.Trim();
        template.FontFamily = request.FontFamily.Trim();
        template.AccentColor = request.AccentColor.Trim();
        template.LogoStorageKey = request.LogoStorageKey?.Trim();
        template.IsDefault = request.IsDefault;
        template.UpdatedAt = at;
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return ToResponse(template);
    }

    public async Task<DocumentTemplatePreviewResponse?> PreviewAsync(Guid id, CancellationToken ct)
    {
        var template = await db
            .DocumentTemplates.AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, ct)
            .ConfigureAwait(false);
        if (template is null)
            return null;
        Func<string, string> encode = HtmlEncoder.Default.Encode;
        return new(
            template.Id,
            template.DocumentType,
            template.Language,
            $"<article style=\"font-family:{encode(template.FontFamily)};color:{encode(template.AccentColor)}\"><header>{encode(template.Letterhead)}</header><main>{encode(template.BodyTemplate)}</main><footer>{encode(template.Footer)}</footer></article>"
        );
    }

    private async Task ClearDefaultAsync(
        string type,
        string language,
        CancellationToken ct,
        Guid? except = null
    )
    {
        var defaults = await db
            .DocumentTemplates.Where(t =>
                t.DocumentType == type && t.Language == language && t.IsDefault && t.Id != except
            )
            .ToListAsync(ct)
            .ConfigureAwait(false);
        foreach (var template in defaults)
            template.IsDefault = false;
    }

    private static void Validate(
        string name,
        string? type,
        string letterhead,
        string body,
        string font,
        string color
    )
    {
        if (
            string.IsNullOrWhiteSpace(name)
            || (type is not null && string.IsNullOrWhiteSpace(type))
            || string.IsNullOrWhiteSpace(letterhead)
            || string.IsNullOrWhiteSpace(body)
            || string.IsNullOrWhiteSpace(font)
            || string.IsNullOrWhiteSpace(color)
        )
            throw new InvalidOperationException(
                "Template name, type, letterhead, body, font and accent color are required."
            );
    }

    private static string NormalizeLanguage(string language)
    {
        var normalized = language.Trim().ToLowerInvariant();
        if (
            normalized.Length is < 2 or > 16
            || normalized.Any(character => !char.IsLetterOrDigit(character) && character != '-')
        )
            throw new InvalidOperationException(
                "Language must be a valid locale such as en or fr-CA."
            );
        return normalized;
    }

    private static IQueryable<DocumentTemplateRow> ApplyLanguageFilter(
        IQueryable<DocumentTemplateRow> query,
        string? language
    ) => language is null ? query : query.Where(t => t.Language == NormalizeLanguage(language));

    private static DocumentTemplateResponse ToResponse(DocumentTemplateRow t) =>
        new(
            t.Id,
            t.Name,
            t.DocumentType,
            t.Language,
            t.Letterhead,
            t.BodyTemplate,
            t.Footer,
            t.FontFamily,
            t.AccentColor,
            t.LogoStorageKey,
            t.IsDefault,
            t.UpdatedAt
        );
}
#pragma warning restore CS1591
