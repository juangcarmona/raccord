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
