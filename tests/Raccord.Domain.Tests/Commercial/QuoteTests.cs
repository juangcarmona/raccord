using Raccord.Domain.Commercial;
using Raccord.SharedKernel.Common;

namespace Raccord.Domain.Tests.Commercial;

public sealed class QuoteTests
{
    [Fact]
    public void Generate_Should_snapshot_lines_and_apply_validity()
    {
        var quote = Quote.Generate(Guid.NewGuid(), Guid.NewGuid(), "Customer", "Project", QuoteNumberSequence.Start("Q", 2026).Allocate(), new DateOnly(2026, 10, 13), 14, "30 days", [QuoteLine.FromProjectLine(1, "Camera", 2, 100, 0, 21, "EUR")]);
        quote.Number.ShouldBe("Q2026-00001");
        quote.ValidUntil.ShouldBe(new DateOnly(2026, 10, 27));
        quote.Lines.Single().Description.ShouldBe("Camera");
    }

    [Fact]
    public void Generate_Should_reject_an_empty_plan()
    {
        var error = Should.Throw<BusinessRuleViolationException>(() => Quote.Generate(Guid.NewGuid(), Guid.NewGuid(), "Customer", "Project", QuoteNumberSequence.Start("Q", 2026).Allocate(), new DateOnly(2026, 10, 13), 14, null, []));
        error.Code.ShouldBe("quotes.plan-empty");
    }
}
