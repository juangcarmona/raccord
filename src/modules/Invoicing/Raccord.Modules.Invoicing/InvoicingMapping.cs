using Raccord.Contracts.Common;
using Raccord.Contracts.Invoicing;
using Raccord.Domain.Billing;
using Raccord.SharedKernel.Parties;
using Raccord.SharedKernel.Pricing;

namespace Raccord.Modules.Invoicing;

internal static class InvoicingMapping
{
    internal static PartyResponse ToResponse(this LegalParty party) =>
        new(
            party.LegalName,
            party.TaxId,
            party.Address is null
                ? null
                : new AddressDto(
                    party.Address.Street,
                    party.Address.PostalCode,
                    party.Address.City,
                    party.Address.Province,
                    party.Address.CountryCode
                )
        );

    internal static InvoiceTotalsResponse ToResponse(this InvoiceTotals totals) =>
        new(
            totals.Total.Currency.Code,
            totals.TaxableBase.Amount,
            totals.VatAmount.Amount,
            totals.Total.Amount,
            [
                .. totals.VatBreakdown.Select(group => new VatGroupResponse(
                    group.Rate.Percentage,
                    group.TaxableBase.Amount,
                    group.VatAmount.Amount
                )),
            ]
        );

    internal static InvoiceLineResponse ToResponse(this InvoiceDraftLine line) =>
        new(
            line.Id,
            line.Position,
            line.Description,
            line.Pricing.Quantity,
            line.Pricing.UnitPrice,
            line.Pricing.DiscountPercent,
            line.Pricing.VatRate.Percentage,
            line.Pricing.NetAmount.Amount
        );

    internal static InvoiceLineResponse ToResponse(this IssuedInvoiceLine line) =>
        new(
            null,
            line.Position,
            line.Description,
            line.Pricing.Quantity,
            line.Pricing.UnitPrice,
            line.Pricing.DiscountPercent,
            line.Pricing.VatRate.Percentage,
            line.Pricing.NetAmount.Amount
        );

    internal static ValidationIssueResponse ToResponse(this ValidationIssue issue) =>
        new(issue.Code, issue.Message);

    internal static IssuedInvoiceResponse ToResponse(
        this IssuedInvoice invoice,
        string contentHash
    ) =>
        new(
            invoice.Id,
            invoice.Number.Value,
            invoice.Number.SeriesCode,
            invoice.IssueDate,
            invoice.OperationDate,
            invoice.IssuedAt,
            invoice.Issuer.ToResponse(),
            invoice.Recipient.ToResponse(),
            [.. invoice.Lines.Select(ToResponse)],
            invoice.Totals.ToResponse(),
            contentHash,
            invoice.Source.ProjectId,
            invoice.Source.CustomerId,
            invoice.Source.DraftId
        );
}
