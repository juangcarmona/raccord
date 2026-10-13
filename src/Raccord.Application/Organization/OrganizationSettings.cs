using System.Globalization;
using Raccord.Application.Abstractions;
using Raccord.Application.Auditing;
using Raccord.Application.Invoicing;
using Raccord.Contracts.Invoicing;
using Raccord.Contracts.Organization;
using Raccord.Domain.Organization;
using Raccord.SharedKernel.Abstractions;
using Raccord.SharedKernel.Auditing;
using Raccord.SharedKernel.Common;

namespace Raccord.Application.Organization;

/// <summary>Use cases for the issuing organization's legal details and invoice series.</summary>
public sealed class OrganizationSettings(
    IIssuerProfileRepository issuers,
    IFinancialSettingsRepository financialSettings,
    IInvoiceSeriesRepository series,
    IAuditTrail audit,
    IClock clock,
    ICurrentUser actor,
    IUnitOfWork unitOfWork
)
{
    /// <summary>Reads financial settings, returning safe defaults before first configuration.</summary>
    public async Task<FinancialSettingsResponse> GetFinancialSettingsAsync(
        CancellationToken cancellationToken
    )
    {
        var settings =
            await financialSettings.FindAsync(cancellationToken).ConfigureAwait(false)
            ?? FinancialSettings.Configure(30, 30, false);
        return new(
            settings.QuoteValidityDays,
            settings.ContractValidityDays,
            settings.AllowSigningAfterExpiry
        );
    }

    /// <summary>Creates or updates company-wide financial settings.</summary>
    public async Task<FinancialSettingsResponse> UpdateFinancialSettingsAsync(
        UpdateFinancialSettingsRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var settings = await financialSettings.FindAsync(cancellationToken).ConfigureAwait(false);
        if (settings is null)
        {
            settings = FinancialSettings.Configure(
                request.QuoteValidityDays,
                request.ContractValidityDays,
                request.AllowSigningAfterExpiry
            );
            financialSettings.Add(settings);
        }
        else
        {
            settings.Update(
                request.QuoteValidityDays,
                request.ContractValidityDays,
                request.AllowSigningAfterExpiry
            );
        }

        audit.Append(
            new AuditEntry(
                clock.UtcNow,
                actor.Name,
                "financial-settings.updated",
                "financial-settings",
                settings.Id,
                new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    ["quoteValidityDays"] = settings.QuoteValidityDays.ToString(
                        CultureInfo.InvariantCulture
                    ),
                    ["contractValidityDays"] = settings.ContractValidityDays.ToString(
                        CultureInfo.InvariantCulture
                    ),
                }
            )
        );
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return new(
            settings.QuoteValidityDays,
            settings.ContractValidityDays,
            settings.AllowSigningAfterExpiry
        );
    }

    /// <summary>Reads the issuer profile, if defined.</summary>
    public async Task<IssuerProfileResponse?> GetIssuerAsync(CancellationToken cancellationToken)
    {
        var profile = await issuers.FindAsync(cancellationToken).ConfigureAwait(false);
        return profile is null ? null : ToResponse(profile);
    }

    /// <summary>Defines or updates the issuer profile used by future invoices.</summary>
    public async Task<IssuerProfileResponse> UpdateIssuerAsync(
        UpdateIssuerProfileRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var address =
            request.Address.ToDomain()
            ?? throw new BusinessRuleViolationException(
                "issuer.address-missing",
                "The issuer address is incomplete."
            );
        var profile = await issuers.FindAsync(cancellationToken).ConfigureAwait(false);
        if (profile is null)
        {
            profile = IssuerProfile.Define(
                request.LegalName ?? string.Empty,
                request.TaxId,
                address
            );
            issuers.Add(profile);
        }
        else
        {
            profile.Update(request.LegalName ?? string.Empty, request.TaxId, address);
        }

        audit.Append(
            new AuditEntry(
                clock.UtcNow,
                actor.Name,
                "issuer-profile.updated",
                "issuer-profile",
                profile.Id,
                new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    ["legalName"] = profile.LegalName,
                    ["taxId"] = profile.TaxId,
                }
            )
        );
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(profile);
    }

    /// <summary>Lists invoice series.</summary>
    public async Task<IReadOnlyList<InvoiceSeriesResponse>> ListSeriesAsync(
        CancellationToken cancellationToken
    ) =>
        [
            .. (await series.ListAsync(cancellationToken).ConfigureAwait(false)).Select(
                item => new InvoiceSeriesResponse(item.Code, item.Description, item.IsDefault)
            ),
        ];

    private static IssuerProfileResponse ToResponse(IssuerProfile profile) =>
        new(profile.LegalName, profile.TaxId, profile.Address.ToAddress().ToDto()!);
}
