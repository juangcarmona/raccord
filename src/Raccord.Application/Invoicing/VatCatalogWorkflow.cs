using System.Globalization;
using Raccord.Application.Abstractions;
using Raccord.Application.Auditing;
using Raccord.Contracts.Invoicing;
using Raccord.Domain.Billing;
using Raccord.SharedKernel.Abstractions;
using Raccord.SharedKernel.Auditing;
using Raccord.SharedKernel.Common;

namespace Raccord.Application.Invoicing;

/// <summary>Configures VAT classes and schemes and records their audit history.</summary>
public sealed class VatCatalogWorkflow(
    IVatCatalogRepository catalog,
    IUnitOfWork unitOfWork,
    IAuditTrail audit,
    ICurrentUser currentUser,
    IClock clock
)
{
    /// <summary>Returns all configured VAT classes.</summary>
    public async Task<IReadOnlyList<VatClassResponse>> ListClassesAsync(
        CancellationToken cancellationToken
    ) =>
        [
            .. (await catalog.ListClassesAsync(cancellationToken).ConfigureAwait(false)).Select(
                ToResponse
            ),
        ];

    /// <summary>Returns all configured VAT schemes.</summary>
    public async Task<IReadOnlyList<VatSchemeResponse>> ListSchemesAsync(
        CancellationToken cancellationToken
    ) =>
        [
            .. (await catalog.ListSchemesAsync(cancellationToken).ConfigureAwait(false)).Select(
                ToResponse
            ),
        ];

    /// <summary>Creates a VAT class.</summary>
    public async Task<VatClassResponse> CreateClassAsync(
        CreateVatClassRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var vatClass = VatClass.Create(request.Name);
        catalog.Add(vatClass);
        AppendAudit(
            "vat-class.created",
            "VatClass",
            vatClass.Id,
            new Dictionary<string, string>(StringComparer.Ordinal) { ["name"] = vatClass.Name }
        );
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(vatClass);
    }

    /// <summary>Changes a VAT class display name without changing its identity.</summary>
    public async Task<VatClassResponse?> RenameClassAsync(
        Guid id,
        RenameVatClassRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var vatClass = await catalog.FindClassAsync(id, cancellationToken).ConfigureAwait(false);
        if (vatClass is null)
        {
            return null;
        }

        var previousName = vatClass.Name;
        vatClass.Rename(request.Name);
        AppendAudit(
            "vat-class.renamed",
            "VatClass",
            id,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["previousName"] = previousName,
                ["newName"] = vatClass.Name,
            }
        );
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(vatClass);
    }

    /// <summary>Creates a VAT scheme after validating all referenced classes.</summary>
    public async Task<VatSchemeResponse> CreateSchemeAsync(
        CreateVatSchemeRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var kind = ParseKind(request.Kind);
        var rates = await ValidateRatesAsync(request.ClassRates, cancellationToken)
            .ConfigureAwait(false);
        var scheme = kind switch
        {
            VatSchemeKind.PerClass => VatScheme.CreatePerClass(request.Name, rates),
            VatSchemeKind.FixedRate => VatScheme.CreateFixedRate(
                request.Name,
                request.FixedPercentage
                    ?? throw new BusinessRuleViolationException(
                        "invoicing.vat-rate-required",
                        "A fixed-rate scheme requires a percentage."
                    )
            ),
            VatSchemeKind.ReverseCharge => VatScheme.CreateReverseCharge(request.Name),
            _ => throw new BusinessRuleViolationException(
                "invoicing.vat-scheme-kind-invalid",
                "The VAT scheme kind is unsupported."
            ),
        };
        catalog.Add(scheme);
        AppendAudit("vat-scheme.created", "VatScheme", scheme.Id, SchemeAuditDetails(scheme));
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(scheme);
    }

    /// <summary>Changes an unused scheme's rate while preserving its identity.</summary>
    public async Task<VatSchemeResponse?> UpdateSchemeAsync(
        Guid id,
        UpdateVatSchemeRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var scheme = await catalog.FindSchemeAsync(id, cancellationToken).ConfigureAwait(false);
        if (scheme is null)
        {
            return null;
        }

        var previousDetails = SchemeAuditDetails(scheme);
        scheme.Rename(request.Name);
        switch (scheme.Kind)
        {
            case VatSchemeKind.PerClass:
                scheme.UpdatePerClassRates(
                    await ValidateRatesAsync(request.ClassRates, cancellationToken)
                        .ConfigureAwait(false)
                );
                break;
            case VatSchemeKind.FixedRate:
                scheme.UpdateFixedRate(
                    request.FixedPercentage
                        ?? throw new BusinessRuleViolationException(
                            "invoicing.vat-rate-required",
                            "A fixed-rate scheme requires a percentage."
                        )
                );
                break;
            case VatSchemeKind.ReverseCharge:
                if (request.FixedPercentage is not null || request.ClassRates is { Count: > 0 })
                {
                    throw new BusinessRuleViolationException(
                        "invoicing.vat-scheme-kind-mismatch",
                        "A reverse-charge scheme has a zero rate and does not accept mappings."
                    );
                }
                break;
        }

        AppendAudit(
            "vat-scheme.updated",
            "VatScheme",
            id,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["previousName"] = previousDetails["name"],
                ["newName"] = scheme.Name,
                ["previousKind"] = previousDetails["kind"],
                ["newKind"] = SchemeKindName(scheme.Kind),
                ["previousRates"] = previousDetails["rates"],
                ["newRates"] = FormatSchemeRates(scheme),
            }
        );
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(scheme);
    }

    /// <summary>Selects one scheme as the default for future commercial records.</summary>
    public async Task<VatSchemeResponse?> SetDefaultAsync(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        var scheme = await catalog.FindSchemeAsync(id, cancellationToken).ConfigureAwait(false);
        if (scheme is null)
        {
            return null;
        }

        var schemes = await catalog.ListSchemesAsync(cancellationToken).ConfigureAwait(false);
        var previousDefaultId = schemes.FirstOrDefault(current => current.IsDefault)?.Id;
        foreach (var current in schemes)
        {
            current.SetDefault(current.Id == id);
        }

        AppendAudit(
            "vat-scheme.default-set",
            "VatScheme",
            id,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["previousDefaultSchemeId"] = previousDefaultId?.ToString("D") ?? string.Empty,
                ["newDefaultSchemeId"] = id.ToString("D"),
            }
        );
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(scheme);
    }

    private async Task<IReadOnlyList<VatClassRate>> ValidateRatesAsync(
        IReadOnlyList<VatClassRateRequest>? requested,
        CancellationToken cancellationToken
    )
    {
        var rates = requested ?? [];
        var available = (await catalog.ListClassesAsync(cancellationToken).ConfigureAwait(false))
            .Select(vatClass => vatClass.Id)
            .ToHashSet();
        if (rates.Any(rate => !available.Contains(rate.VatClassId)))
        {
            throw new BusinessRuleViolationException(
                "invoicing.vat-class-unmapped",
                "Every VAT scheme mapping must reference an existing VAT class."
            );
        }

        return [.. rates.Select(rate => new VatClassRate(rate.VatClassId, rate.Percentage))];
    }

    private void AppendAudit(
        string action,
        string entityType,
        Guid entityId,
        IReadOnlyDictionary<string, string> details
    ) =>
        audit.Append(
            new AuditEntry(clock.UtcNow, currentUser.Name, action, entityType, entityId, details)
        );

    private static VatSchemeKind ParseKind(string kind) =>
        kind?.Trim().ToLowerInvariant() switch
        {
            "per-class" => VatSchemeKind.PerClass,
            "fixed-rate" => VatSchemeKind.FixedRate,
            "reverse-charge" => VatSchemeKind.ReverseCharge,
            _ => throw new BusinessRuleViolationException(
                "invoicing.vat-scheme-kind-invalid",
                "The VAT scheme kind must be per-class, fixed-rate or reverse-charge."
            ),
        };

    private static Dictionary<string, string> SchemeAuditDetails(VatScheme scheme) =>
        new(StringComparer.Ordinal)
        {
            ["name"] = scheme.Name,
            ["kind"] = SchemeKindName(scheme.Kind),
            ["rates"] = FormatSchemeRates(scheme),
        };

    private static string SchemeKindName(VatSchemeKind kind) =>
        kind switch
        {
            VatSchemeKind.PerClass => "per-class",
            VatSchemeKind.FixedRate => "fixed-rate",
            VatSchemeKind.ReverseCharge => "reverse-charge",
            _ => "unknown",
        };

    private static string FormatSchemeRates(VatScheme scheme) =>
        scheme.Kind switch
        {
            VatSchemeKind.PerClass => string.Join(
                ";",
                scheme
                    .ClassRates.OrderBy(rate => rate.VatClassId)
                    .Select(rate =>
                        $"{rate.VatClassId:D}={rate.Percentage.ToString("G29", CultureInfo.InvariantCulture)}"
                    )
            ),
            VatSchemeKind.FixedRate => scheme.FixedPercentage?.ToString(
                "G29",
                CultureInfo.InvariantCulture
            ) ?? string.Empty,
            VatSchemeKind.ReverseCharge => "0",
            _ => string.Empty,
        };

    private static VatClassResponse ToResponse(VatClass vatClass) =>
        new(vatClass.Id, vatClass.Name);

    private static VatSchemeResponse ToResponse(VatScheme scheme) =>
        new(
            scheme.Id,
            scheme.Name,
            scheme.Kind switch
            {
                VatSchemeKind.PerClass => "per-class",
                VatSchemeKind.FixedRate => "fixed-rate",
                VatSchemeKind.ReverseCharge => "reverse-charge",
                _ => "unknown",
            },
            scheme.FixedPercentage,
            scheme.IsDefault,
            scheme.IsUsed,
            scheme.Territory,
            scheme.SourceUrl,
            scheme.VerifiedOn,
            [
                .. scheme.ClassRates.Select(rate => new VatClassRateResponse(
                    rate.VatClassId,
                    rate.Percentage
                )),
            ]
        );
}
