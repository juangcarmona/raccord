using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Raccord.Contracts.Contacts;
using Raccord.Infrastructure.Persistence;
using Raccord.Modules.Contacts;
using Raccord.Web.Authorization;

namespace Raccord.Web.Api;

internal static class ContactImportEndpoints
{
    private const long MaximumRequestBytes = ContactImportLimits.MaximumFileBytes + 64 * 1024;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    internal static void MapContactImport(this RouteGroupBuilder contacts)
    {
        contacts
            .MapPost("/import/inspect", InspectAsync)
            .Accepts<ContactImportForm>("multipart/form-data")
            .WithMetadata(new RequestSizeLimitAttribute(MaximumRequestBytes))
            .RequirePermission(PermissionCatalog.ContactsCreate);
        contacts
            .MapPost("/import/preview", PreviewAsync)
            .Accepts<ContactImportForm>("multipart/form-data")
            .WithMetadata(new RequestSizeLimitAttribute(MaximumRequestBytes))
            .RequirePermission(PermissionCatalog.ContactsCreate);
        contacts
            .MapPost("/import", ImportAsync)
            .Accepts<ContactImportForm>("multipart/form-data")
            .WithMetadata(new RequestSizeLimitAttribute(MaximumRequestBytes))
            .RequirePermission(PermissionCatalog.ContactsCreate);
    }

    private static async Task<
        Results<Ok<ContactImportInspectionResponse>, BadRequest<string>>
    > InspectAsync(HttpRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var form = await request.ReadFormAsync(cancellationToken).ConfigureAwait(false);
            var file = await ReadFileAsync(form, cancellationToken).ConfigureAwait(false);
            var hasHeader = bool.TryParse(form["hasHeader"], out var selected) && selected;
            return TypedResults.Ok(
                ContactImportDirectory.Inspect(file.Content, file.FileName, hasHeader)
            );
        }
        catch (ContactImportFormatException exception)
        {
            return TypedResults.BadRequest(exception.Message);
        }
    }

    private static async Task<
        Results<Ok<ContactImportPreviewResponse>, BadRequest<string>>
    > PreviewAsync(
        HttpRequest request,
        ContactImportDirectory imports,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var input = await ReadAsync(request, includeDigest: false, cancellationToken)
                .ConfigureAwait(false);
            return TypedResults.Ok(
                await imports
                    .PreviewAsync(input.Content, input.FileName, input.Mapping, cancellationToken)
                    .ConfigureAwait(false)
            );
        }
        catch (ContactImportFormatException exception)
        {
            return TypedResults.BadRequest(exception.Message);
        }
    }

    private static async Task<
        Results<Ok<ContactImportResultResponse>, BadRequest<string>>
    > ImportAsync(
        HttpRequest request,
        ContactImportDirectory imports,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var input = await ReadAsync(request, includeDigest: true, cancellationToken)
                .ConfigureAwait(false);
            var result = await imports
                .ImportAsync(
                    input.Content,
                    input.FileName,
                    input.Mapping,
                    input.ExpectedSha256!,
                    cancellationToken
                )
                .ConfigureAwait(false);
            return TypedResults.Ok(result);
        }
        catch (ContactImportFormatException exception)
        {
            return TypedResults.BadRequest(exception.Message);
        }
    }

    private static async Task<ContactImportInput> ReadAsync(
        HttpRequest request,
        bool includeDigest,
        CancellationToken cancellationToken
    )
    {
        if (!request.HasFormContentType)
        {
            throw new ContactImportFormatException(
                "contacts.import.form-required",
                "Upload the spreadsheet as multipart form data."
            );
        }

        var form = await request.ReadFormAsync(cancellationToken).ConfigureAwait(false);
        var file = await ReadFileAsync(form, cancellationToken).ConfigureAwait(false);
        var mapping = ParseMapping(form["mapping"].ToString());
        var expectedSha256 = GetExpectedDigest(form, includeDigest);

        return new ContactImportInput(file.Content, file.FileName, mapping, expectedSha256);
    }

    private static async Task<ContactImportFile> ReadFileAsync(
        IFormCollection form,
        CancellationToken cancellationToken
    )
    {
        var file = form.Files.GetFile("file");
        if (file is null)
        {
            throw new ContactImportFormatException(
                "contacts.import.file-required",
                "Choose a CSV or XLSX file."
            );
        }

        if (file.Length > ContactImportLimits.MaximumFileBytes)
        {
            throw new ContactImportFormatException(
                "contacts.import.file-too-large",
                "The spreadsheet exceeds the 10 MiB upload limit."
            );
        }

        await using var stream = new MemoryStream((int)file.Length);
        await file.CopyToAsync(stream, cancellationToken).ConfigureAwait(false);
        return new ContactImportFile(stream.ToArray(), file.FileName);
    }

    private static ContactImportMapping ParseMapping(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<ContactImportMapping>(json, JsonOptions)
                ?? throw new JsonException("A contact import mapping is required.");
        }
        catch (JsonException)
        {
            throw new ContactImportFormatException(
                "contacts.import.mapping-invalid",
                "Map the required contact fields before continuing."
            );
        }
    }

    private static string? GetExpectedDigest(IFormCollection form, bool includeDigest)
    {
        if (!includeDigest)
        {
            return null;
        }

        var digest = form["sha256"].ToString();
        if (string.IsNullOrWhiteSpace(digest))
        {
            throw new ContactImportFormatException(
                "contacts.import.preview-required",
                "Preview the spreadsheet before importing it."
            );
        }

        return digest;
    }

    private sealed record ContactImportInput(
        byte[] Content,
        string FileName,
        ContactImportMapping Mapping,
        string? ExpectedSha256
    );

    private sealed record ContactImportFile(byte[] Content, string FileName);

    public sealed record ContactImportForm(
        IFormFile File,
        string? Mapping,
        string? Sha256,
        bool? HasHeader
    );
}
