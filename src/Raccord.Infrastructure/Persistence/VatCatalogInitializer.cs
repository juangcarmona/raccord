using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Raccord.Domain.Billing;
using Raccord.SharedKernel.Abstractions;
using Raccord.SharedKernel.Auditing;

namespace Raccord.Infrastructure.Persistence;

/// <summary>Adds the source-dated starter catalog only when an installation has none.</summary>
public sealed class VatCatalogInitializer(RaccordDbContext db, IAuditAppender audit, IClock clock)
{
    /// <summary>Creates the general class and standard-rate scheme when both catalogs are empty.</summary>
    public async Task EnsureInitializedAsync(string actor, CancellationToken cancellationToken)
    {
        if (
            await db.VatClasses.AnyAsync(cancellationToken).ConfigureAwait(false)
            || await db.VatSchemes.AnyAsync(cancellationToken).ConfigureAwait(false)
        )
        {
            return;
        }

        var vatClass = VatClass.Create("General");
        var bootstrap = SpanishVatDefaults.StandardRate;
        var scheme = VatScheme.CreatePerClass(
            "General",
            [new VatClassRate(vatClass.Id, bootstrap.Percentage)]
        );
        scheme.SetDefault(true);
        scheme.SetBootstrapSource(bootstrap);
        db.VatClasses.Add(vatClass);
        db.VatSchemes.Add(scheme);
        audit.Append(
            new AuditEntry(
                clock.UtcNow,
                actor,
                "vat-catalog.initialized",
                "VatScheme",
                scheme.Id,
                new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    ["vatClassId"] = vatClass.Id.ToString("D"),
                    ["percentage"] = bootstrap.Percentage.ToString(CultureInfo.InvariantCulture),
                    ["territory"] = bootstrap.Territory,
                    ["sourceUrl"] = bootstrap.SourceUrl,
                    ["verifiedOn"] = bootstrap.VerifiedOn.ToString(
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture
                    ),
                }
            )
        );
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
