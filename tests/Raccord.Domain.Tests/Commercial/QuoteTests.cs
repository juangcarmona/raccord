using Raccord.Domain.Commercial;
using Raccord.SharedKernel.Common;

namespace Raccord.Domain.Tests.Commercial;

public sealed class QuoteTests
{
    [Fact]
    public void Generate_Should_snapshot_lines_and_apply_validity()
    {
        var quote = Quote.Generate(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Customer",
            "Project",
            QuoteNumberSequence.Start("Q", 2026).Allocate(),
            new DateOnly(2026, 10, 13),
            14,
            "30 days",
            [QuoteLine.FromProjectLine(1, "Camera", 2, 100, 0, 21, "EUR")]
        );
        quote.Number.ShouldBe("Q2026-00001");
        quote.ValidUntil.ShouldBe(new DateOnly(2026, 10, 27));
        quote.Lines.Single().Description.ShouldBe("Camera");
    }

    [Fact]
    public void Generate_Should_reject_an_empty_plan()
    {
        var error = Should.Throw<BusinessRuleViolationException>(() =>
            Quote.Generate(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Customer",
                "Project",
                QuoteNumberSequence.Start("Q", 2026).Allocate(),
                new DateOnly(2026, 10, 13),
                14,
                null,
                []
            )
        );
        error.Code.ShouldBe("quotes.plan-empty");
    }

    [Fact]
    public void Review_Should_move_a_draft_to_signature_readiness_once()
    {
        var quote = Quote.Generate(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Customer",
            "Project",
            QuoteNumberSequence.Start("Q", 2026).Allocate(),
            new DateOnly(2026, 10, 13),
            14,
            null,
            [QuoteLine.FromProjectLine(1, "Camera", 1, 100, 0, 21, "EUR")]
        );

        quote.MarkReadyForSignature();

        quote.ReviewStatus.ShouldBe(QuoteReviewStatus.ReadyForSignature);
        var error = Should.Throw<BusinessRuleViolationException>(quote.MarkReadyForSignature);
        error.Code.ShouldBe("quotes.review-transition-invalid");
    }

    [Fact]
    public void Acceptance_Should_require_signature_readiness_and_capture_timestamp()
    {
        var quote = Quote.Generate(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Customer",
            "Project",
            QuoteNumberSequence.Start("Q", 2026).Allocate(),
            new DateOnly(2026, 10, 13),
            14,
            null,
            [QuoteLine.FromProjectLine(1, "Camera", 1, 100, 0, 21, "EUR")]
        );
        var timestamp = new DateTimeOffset(2026, 10, 13, 12, 30, 0, TimeSpan.Zero);

        Should
            .Throw<BusinessRuleViolationException>(() => quote.Accept(timestamp))
            .Code.ShouldBe("quotes.acceptance-invalid");
        quote.MarkReadyForSignature();
        quote.Accept(timestamp);

        quote.ReviewStatus.ShouldBe(QuoteReviewStatus.Accepted);
        quote.AcceptedAt.ShouldBe(timestamp);
    }
}
