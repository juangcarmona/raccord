using Raccord.Domain.Billing;
using Raccord.Domain.Common;

namespace Raccord.Domain.Tests.Billing;

public sealed class InvoiceNumberSequenceTests
{
    [Fact]
    public void Numbers_Should_be_correlative_within_a_series_and_year()
    {
        var sequence = InvoiceNumberSequence.Start("F", 2026);

        var first = sequence.Allocate(new DateOnly(2026, 3, 2));
        var second = sequence.Allocate(new DateOnly(2026, 3, 2));

        first.Value.ShouldBe("F2026-00001");
        first.Sequence.ShouldBe(1);
        second.Value.ShouldBe("F2026-00002");
        sequence.LastNumber.ShouldBe(2);
    }

    [Fact]
    public void Issue_dates_Should_not_go_backwards_within_a_series()
    {
        var sequence = InvoiceNumberSequence.Start("F", 2026);
        sequence.Allocate(new DateOnly(2026, 3, 10));

        var error = Should.Throw<BusinessRuleViolationException>(() =>
            sequence.Allocate(new DateOnly(2026, 3, 9))
        );

        error.Code.ShouldBe("numbering.issue-date-before-previous");
        sequence.LastNumber.ShouldBe(1);
    }

    [Fact]
    public void A_yearly_sequence_Should_only_number_invoices_of_its_year()
    {
        var sequence = InvoiceNumberSequence.Start("F", 2026);

        var error = Should.Throw<BusinessRuleViolationException>(() =>
            sequence.Allocate(new DateOnly(2027, 1, 1))
        );

        error.Code.ShouldBe("numbering.year-mismatch");
    }

    [Theory]
    [InlineData("")]
    [InlineData("f")]
    [InlineData("F-1")]
    [InlineData("ABCDEFGHIJK")]
    public void Series_codes_Should_be_short_uppercase_alphanumerics(string code)
    {
        var error = Should.Throw<BusinessRuleViolationException>(() =>
            InvoiceNumberSequence.Start(code, 2026)
        );

        error.Code.ShouldBe("numbering.series-code-invalid");
    }

    [Fact]
    public void Validation_Should_report_an_issue_date_before_the_last_issued_invoice()
    {
        var sequence = InvoiceNumberSequence.Start("F", 2026);
        sequence.Allocate(new DateOnly(2026, 6, 30));

        sequence.Accepts(new DateOnly(2026, 6, 29)).ShouldBeFalse();
        sequence.Accepts(new DateOnly(2026, 6, 30)).ShouldBeTrue();
    }
}
