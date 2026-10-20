using Raccord.Contracts.Common;
using Raccord.Contracts.Contacts;
using Raccord.Modules.Contacts.Contracts;
using Raccord.SharedKernel.Abstractions;
using Raccord.SharedKernel.Auditing;

namespace Raccord.Modules.Contacts;

/// <summary>Previews and imports bounded batches of new contacts from spreadsheets.</summary>
public sealed class ContactImportDirectory(
    ContactsDirectory contacts,
    IContactsUnitOfWork unitOfWork,
    IAuditAppender audit,
    ICurrentUser currentUser,
    IClock clock
)
{
    /// <summary>Reads the column labels and a small sample before field mapping.</summary>
    public static ContactImportInspectionResponse Inspect(
        byte[] content,
        string fileName,
        bool hasHeader
    ) => ContactSpreadsheetParser.Inspect(content, fileName, hasHeader);

    /// <summary>Parses a spreadsheet and validates all rows without persisting contact data.</summary>
    public async Task<ContactImportPreviewResponse> PreviewAsync(
        byte[] content,
        string fileName,
        ContactImportMapping mapping,
        CancellationToken cancellationToken
    )
    {
        var parsed = ContactSpreadsheetParser.Parse(content, fileName, mapping);
        var rows = await ValidateRowsAsync(parsed.Rows, cancellationToken).ConfigureAwait(false);
        return new ContactImportPreviewResponse(
            parsed.Sha256,
            parsed.FileType,
            rows.Length,
            rows.Count(row => row.Errors.Count == 0),
            rows.Count(row => row.Errors.Count > 0),
            parsed.Headers,
            rows.Take(10).ToArray()
        );
    }

    /// <summary>Imports valid rows after verifying the uploaded file matches its preview.</summary>
    public async Task<ContactImportResultResponse> ImportAsync(
        byte[] content,
        string fileName,
        ContactImportMapping mapping,
        string expectedSha256,
        CancellationToken cancellationToken
    )
    {
        var parsed = ContactSpreadsheetParser.Parse(content, fileName, mapping);
        if (!string.Equals(parsed.Sha256, expectedSha256, StringComparison.OrdinalIgnoreCase))
        {
            throw new ContactImportFormatException(
                "contacts.import.preview-mismatch",
                "The file changed after preview. Upload it again and review the new preview."
            );
        }

        var validated = await ValidateRowsAsync(parsed.Rows, cancellationToken)
            .ConfigureAwait(false);
        var importId = Guid.CreateVersion7();
        AppendAudit(importId, "contacts.import.started", parsed.FileType, null, null);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        var rejected = new List<ContactImportRowResponse>();
        var accepted = 0;
        for (var index = 0; index < parsed.Rows.Count; index++)
        {
            var parsedRow = parsed.Rows[index];
            var validation = validated[index];
            if (validation.Errors.Count > 0)
            {
                rejected.Add(validation);
                continue;
            }

            await contacts
                .CreateAsync(ToSaveRequest(parsedRow.Values), cancellationToken)
                .ConfigureAwait(false);
            accepted++;
        }

        AppendAudit(
            importId,
            "contacts.import.completed",
            parsed.FileType,
            accepted,
            rejected.Count
        );
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return new ContactImportResultResponse(importId, accepted, rejected.Count, rejected);
    }

    private async Task<ContactImportRowResponse[]> ValidateRowsAsync(
        IReadOnlyList<ContactImportParsedRow> rows,
        CancellationToken cancellationToken
    )
    {
        var result = new List<ContactImportRowResponse>(rows.Count);
        var seenNumbers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var row in rows)
        {
            var request = ToSaveRequest(row.Values);
            var error = ContactsDirectory.ValidateImportRow(request);
            if (error is null && request.ContactNumber is { } number)
            {
                if (!seenNumbers.Add(number))
                {
                    error = "The contact number is repeated in this file.";
                }
                else if (
                    await contacts
                        .ContactNumberExistsAsync(number, cancellationToken)
                        .ConfigureAwait(false)
                )
                {
                    error = "The contact number is already assigned to another contact.";
                }
            }

            result.Add(
                new ContactImportRowResponse(
                    row.RowNumber,
                    row.Values,
                    error is null ? [] : [error]
                )
            );
        }

        return [.. result];
    }

    private static SaveContactRequest ToSaveRequest(IReadOnlyDictionary<string, string> values)
    {
        string Get(string name) => values.GetValueOrDefault(name, string.Empty);

        var addressValues = new[]
        {
            Get("street"),
            Get("postalCode"),
            Get("city"),
            Get("province"),
            Get("countryCode"),
        };
        AddressDto? address = addressValues.All(string.IsNullOrWhiteSpace)
            ? null
            : new AddressDto(
                addressValues[0],
                addressValues[1],
                addressValues[2],
                addressValues[3],
                addressValues[4]
            );
        var roles = Get("roles")
            .Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        return new SaveContactRequest(
            Get("type"),
            Get("legalName"),
            EmptyToNull(Get("contactNumber")),
            EmptyToNull(Get("taxId")),
            address,
            EmptyToNull(Get("email")),
            roles
        );
    }

    private void AppendAudit(
        Guid importId,
        string action,
        string fileType,
        int? accepted,
        int? rejected
    )
    {
        var details = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["fileType"] = fileType,
        };
        if (accepted is not null)
        {
            details["acceptedRowCount"] = accepted.Value.ToString(
                System.Globalization.CultureInfo.InvariantCulture
            );
            details["rejectedRowCount"] = rejected!.Value.ToString(
                System.Globalization.CultureInfo.InvariantCulture
            );
        }

        audit.Append(
            new AuditEntry(
                clock.UtcNow,
                currentUser.Name,
                action,
                "contact-import",
                importId,
                details
            )
        );
    }

    private static string? EmptyToNull(string value) =>
        string.IsNullOrWhiteSpace(value) ? null : value;
}
