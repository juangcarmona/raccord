using Raccord.Contracts.Common;
using Raccord.Contracts.Invoicing;
using Raccord.Contracts.Projects;
using Raccord.Domain.Billing;
using Raccord.Domain.Parties;
using Raccord.Domain.Pricing;
using Raccord.Domain.Projects;

namespace Raccord.Application;

/// <summary>Maps domain objects to public contracts. Contracts never leak domain types.</summary>
internal static class ContractMapping
{
    public static AddressDto? ToDto(this PostalAddress? address) =>
        address is null
            ? null
            : new AddressDto(
                address.Street,
                address.PostalCode,
                address.City,
                address.Province,
                address.CountryCode
            );

    public static PostalAddress? ToDomain(this AddressDto? address) =>
        address is null
            ? null
            : new PostalAddress(
                address.Street ?? string.Empty,
                address.PostalCode ?? string.Empty,
                address.City ?? string.Empty,
                address.Province ?? string.Empty,
                address.CountryCode ?? string.Empty
            );

    public static PartyResponse ToResponse(this LegalParty party) =>
        new(party.LegalName, party.TaxId, party.Address.ToDto());

    public static InvoiceTotalsResponse ToResponse(this InvoiceTotals totals) =>
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

    public static InvoiceLineResponse ToResponse(this InvoiceDraftLine line)
    {
        var pricing = line.Pricing;
        return new(
            line.Id,
            line.Position,
            line.Description,
            pricing.Quantity,
            pricing.UnitPrice,
            pricing.DiscountPercent,
            pricing.VatRate.Percentage,
            pricing.NetAmount.Amount
        );
    }

    public static InvoiceLineResponse ToResponse(this IssuedInvoiceLine line) =>
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

    public static ValidationIssueResponse ToResponse(this ValidationIssue issue) =>
        new(issue.Code, issue.Message);

    public static BillableLineResponse ToResponse(this BillableLine line) =>
        new(
            line.Id,
            line.Kind.ToString().ToLowerInvariant(),
            line.Description,
            line.Quantity,
            line.UnitPrice,
            line.DiscountPercent,
            line.VatPercentage,
            line.Pricing.NetAmount.Amount,
            line.State switch
            {
                BillingState.Unbilled => "unbilled",
                BillingState.InDraft => "in-draft",
                _ => "invoiced",
            },
            line.DraftId,
            line.InvoiceId
        );

    public static IssuedInvoiceResponse ToResponse(
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
