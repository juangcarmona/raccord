using Microsoft.EntityFrameworkCore;
using Raccord.Domain.Billing;
using Raccord.Modules.Invoicing.Contracts;
using Raccord.SharedKernel.Common;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class VatSchemePricing(RaccordDbContext db) : IVatSchemePricing
{
    public async Task<VatRateSelection> ResolveRateAsync(
        Guid vatClassId,
        Guid vatSchemeId,
        CancellationToken cancellationToken
    )
    {
        if (vatClassId == Guid.Empty || vatSchemeId == Guid.Empty)
        {
            throw new BusinessRuleViolationException(
                "invoicing.vat-reference-incomplete",
                "Select both an existing VAT class and scheme."
            );
        }

        if (
            !await db
                .VatClasses.AnyAsync(item => item.Id == vatClassId, cancellationToken)
                .ConfigureAwait(false)
        )
        {
            throw new BusinessRuleViolationException(
                "invoicing.vat-class-not-found",
                "The selected VAT class does not exist."
            );
        }

        var scheme = await db
            .VatSchemes.Include(item => item.ClassRates)
            .FirstOrDefaultAsync(item => item.Id == vatSchemeId, cancellationToken)
            .ConfigureAwait(false);
        if (scheme is null)
        {
            throw new BusinessRuleViolationException(
                "invoicing.vat-scheme-not-found",
                "The selected VAT scheme does not exist."
            );
        }

        return new VatRateSelection(vatClassId, vatSchemeId, scheme.RateFor(vatClassId).Percentage);
    }

    public void MarkUsed(Guid vatSchemeId)
    {
        var scheme =
            db.VatSchemes.Local.SingleOrDefault(item => item.Id == vatSchemeId)
            ?? throw new InvalidOperationException(
                "Resolve the VAT scheme before marking it used."
            );
        scheme.MarkUsed();
    }
}
