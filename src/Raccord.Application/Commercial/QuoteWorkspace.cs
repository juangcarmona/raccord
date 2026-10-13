using System.Globalization;
using System.Text;
using Raccord.Application.Abstractions;
using Raccord.Application.Organization;
using Raccord.Application.Projects;
using Raccord.Contracts.Commercial;
using Raccord.Domain.Commercial;
using Raccord.Modules.Contacts.Contracts;
using Raccord.SharedKernel.Abstractions;
using Raccord.SharedKernel.Common;

namespace Raccord.Application.Commercial;

/// <summary>Generates immutable quote snapshots from project plans.</summary>
public sealed class QuoteWorkspace(
    IQuoteRepository quotes,
    IProjectRepository projects,
    IFinancialSettingsRepository settings,
    IPartyDirectory parties,
    IClock clock,
    IUnitOfWork unitOfWork
)
{
    /// <summary>Generates a numbered quote from all current priced project lines.</summary>
    public async Task<QuoteResponse?> GenerateAsync(Guid projectId, CancellationToken ct)
    {
        var project = await projects.FindAsync(projectId, ct).ConfigureAwait(false);
        if (project is null)
            return null;
        var customer =
            await parties.FindAsync(project.CustomerId, ct).ConfigureAwait(false)
            ?? throw new BusinessRuleViolationException(
                "customers.not-found",
                "The project's customer no longer exists."
            );
        var financial =
            await settings.FindAsync(ct).ConfigureAwait(false)
            ?? Domain.Organization.FinancialSettings.Configure(30, 30, false);
        var today = clock.Today;
        var sequence = await quotes.FindSequenceAsync("Q", today.Year, ct).ConfigureAwait(false);
        if (sequence is null)
        {
            sequence = QuoteNumberSequence.Start("Q", today.Year);
            quotes.AddSequence(sequence);
        }
        var number = sequence.Allocate();
        var lines = project.BillableLines.Select(line =>
            QuoteLine.FromProjectLine(
                line.Position,
                line.Description,
                line.Quantity,
                line.UnitPrice,
                line.DiscountPercent,
                line.VatPercentage,
                line.CurrencyCode
            )
        );
        var quote = Quote.Generate(
            project.Id,
            project.CustomerId,
            customer.LegalName,
            project.Name,
            number,
            today,
            financial.QuoteValidityDays,
            project.PaymentTerms,
            lines
        );
        quotes.Add(quote);
        await unitOfWork.SaveChangesAsync(ct).ConfigureAwait(false);
        return ToResponse(quote);
    }

    /// <summary>Reads an immutable quote snapshot.</summary>
    public async Task<QuoteResponse?> GetAsync(Guid id, CancellationToken ct) =>
        await quotes.FindAsync(id, ct).ConfigureAwait(false) is { } quote
            ? ToResponse(quote)
            : null;

    /// <summary>Marks a draft quote ready for customer signature.</summary>
    public async Task<QuoteResponse?> UpdateReviewAsync(
        Guid id,
        UpdateQuoteReviewRequest request,
        CancellationToken ct
    )
    {
        var quote = await quotes.FindAsync(id, ct).ConfigureAwait(false);
        if (quote is null)
            return null;
        if (!string.Equals(request.Status, "ReadyForSignature", StringComparison.Ordinal))
            throw new BusinessRuleViolationException(
                "quotes.review-status-invalid",
                "The only supported review transition is ReadyForSignature."
            );
        quote.MarkReadyForSignature();
        await unitOfWork.SaveChangesAsync(ct).ConfigureAwait(false);
        return ToResponse(quote);
    }

    /// <summary>Records acceptance using the trusted application clock.</summary>
    public async Task<QuoteResponse?> AcceptAsync(Guid id, CancellationToken ct)
    {
        var quote = await quotes.FindAsync(id, ct).ConfigureAwait(false);
        if (quote is null)
            return null;
        quote.Accept(clock.UtcNow);
        await unitOfWork.SaveChangesAsync(ct).ConfigureAwait(false);
        return ToResponse(quote);
    }

    /// <summary>Records signature metadata using the trusted application clock.</summary>
    public async Task<QuoteResponse?> SignAsync(
        Guid id,
        SignQuoteRequest request,
        CancellationToken ct
    )
    {
        var quote = await quotes.FindAsync(id, ct).ConfigureAwait(false);
        if (quote is null)
            return null;
        quote.Sign(request.SignerName, clock.UtcNow);
        await unitOfWork.SaveChangesAsync(ct).ConfigureAwait(false);
        return ToResponse(quote);
    }

    /// <summary>Builds a deterministic text document for an accepted quote.</summary>
    public async Task<QuoteDocument?> GetDocumentAsync(Guid id, CancellationToken ct)
    {
        var quote = await quotes.FindAsync(id, ct).ConfigureAwait(false);
        if (quote is null || quote.ReviewStatus != QuoteReviewStatus.Accepted)
            return null;
        var builder = new StringBuilder()
            .AppendLine(CultureInfo.InvariantCulture, $"Quote {quote.Number}")
            .AppendLine(CultureInfo.InvariantCulture, $"Customer: {quote.CustomerName}")
            .AppendLine(CultureInfo.InvariantCulture, $"Project: {quote.ProjectName}")
            .AppendLine(CultureInfo.InvariantCulture, $"Issued: {quote.IssuedOn:yyyy-MM-dd}")
            .AppendLine(CultureInfo.InvariantCulture, $"Valid until: {quote.ValidUntil:yyyy-MM-dd}")
            .AppendLine(CultureInfo.InvariantCulture, $"Accepted at: {quote.AcceptedAt:O}")
            .AppendLine(
                CultureInfo.InvariantCulture,
                $"Payment terms: {quote.PaymentTerms ?? string.Empty}"
            )
            .AppendLine()
            .AppendLine(
                "Position | Description | Quantity | Unit price | Discount | VAT | Currency"
            );
        foreach (var line in quote.Lines)
            builder
                .Append(line.Position.ToString(CultureInfo.InvariantCulture))
                .Append(" | ")
                .Append(line.Description)
                .Append(" | ")
                .Append(line.Quantity.ToString(CultureInfo.InvariantCulture))
                .Append(" | ")
                .Append(line.UnitPrice.ToString(CultureInfo.InvariantCulture))
                .Append(" | ")
                .Append(line.DiscountPercent.ToString(CultureInfo.InvariantCulture))
                .Append(" | ")
                .Append(line.VatPercentage.ToString(CultureInfo.InvariantCulture))
                .Append(" | ")
                .AppendLine(line.CurrencyCode);
        return new(
            $"quote-{quote.Number}.txt",
            "text/plain; charset=utf-8",
            Encoding.UTF8.GetBytes(builder.ToString())
        );
    }

    private static QuoteResponse ToResponse(Quote quote) =>
        new(
            quote.Id,
            quote.ProjectId,
            quote.CustomerId,
            quote.CustomerName,
            quote.ProjectName,
            quote.Number,
            quote.IssuedOn,
            quote.ValidUntil,
            quote.PaymentTerms,
            quote.ReviewStatus.ToString(),
            quote.AcceptedAt,
            quote.SignedBy,
            quote.SignedAt,
            [
                .. quote.Lines.Select(line => new QuoteLineResponse(
                    line.Position,
                    line.Description,
                    line.Quantity,
                    line.UnitPrice,
                    line.DiscountPercent,
                    line.VatPercentage,
                    line.CurrencyCode
                )),
            ]
        );
}
