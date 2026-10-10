using Raccord.Domain.Billing;
using Raccord.Domain.Common;

namespace Raccord.Domain.Tests.Billing;

public sealed class InvoiceNumberSequenceTests
{
    [Fact]
    public void Numbers_are_correlative_within_a_series_and_year()
    {
        var sequence = InvoiceNumberSequence.Start("F", 2026);

        var first = sequence.Allocate(new DateOnly(2026, 3, 2));
        var second = sequence.Allocate(new DateOnly(2026, 3, 2));

        Assert.Equal("F2026-00001", first.Value);
        Assert.Equal(1, first.Sequence);
        Assert.Equal("F2026-00002", second.Value);
        Assert.Equal(2, sequence.LastNumber);
    }

    [Fact]
    public void Issue_dates_cannot_go_backwards_within_a_series()
    {
        var sequence = InvoiceNumberSequence.Start("F", 2026);
        sequence.Allocate(new DateOnly(2026, 3, 10));

        var error = Assert.Throws<BusinessRuleViolationException>(() =>
            sequence.Allocate(new DateOnly(2026, 3, 9))
        );

        Assert.Equal("numbering.issue-date-before-previous", error.Code);
        Assert.Equal(1, sequence.LastNumber);
    }

    [Fact]
    public void A_yearly_sequence_only_numbers_invoices_of_its_year()
    {
        var sequence = InvoiceNumberSequence.Start("F", 2026);

        var error = Assert.Throws<BusinessRuleViolationException>(() =>
            sequence.Allocate(new DateOnly(2027, 1, 1))
        );

        Assert.Equal("numbering.year-mismatch", error.Code);
    }

    [Theory]
    [InlineData("")]
    [InlineData("f")]
    [InlineData("F-1")]
    [InlineData("ABCDEFGHIJK")]
    public void Series_codes_are_short_uppercase_alphanumerics(string code)
    {
        var error = Assert.Throws<BusinessRuleViolationException>(() =>
            InvoiceNumberSequence.Start(code, 2026)
        );

        Assert.Equal("numbering.series-code-invalid", error.Code);
    }

    [Fact]
    public void Validation_reports_an_issue_date_before_the_last_issued_invoice()
    {
        var sequence = InvoiceNumberSequence.Start("F", 2026);
        sequence.Allocate(new DateOnly(2026, 6, 30));

        Assert.False(sequence.Accepts(new DateOnly(2026, 6, 29)));
        Assert.True(sequence.Accepts(new DateOnly(2026, 6, 30)));
    }
}
