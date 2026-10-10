using Raccord.Domain.Billing;
using Raccord.Domain.Common;
using Raccord.Domain.Parties;
using Raccord.Domain.Pricing;
using Raccord.Domain.Projects;

namespace Raccord.Domain.Tests.Billing;

public sealed class InvoiceDraftTests
{
    private static readonly DateTimeOffset Now = new(2026, 5, 11, 9, 30, 0, TimeSpan.Zero);
    private static readonly DateOnly IssueDate = new(2026, 5, 11);

    private static readonly LegalParty Issuer = new(
        "Fotograma Rental S.L.",
        "B00000000",
        new PostalAddress("Calle Mayor 1", "28013", "Madrid", "Madrid", "ES")
    );

    private static readonly LegalParty Recipient = new(
        "Productora Ejemplo S.A.",
        "A00000000",
        new PostalAddress("Avenida del Puerto 20", "46023", "Valencia", "Valencia", "ES")
    );

    private readonly Project project = Project.Create(
        "Spring campaign shoot",
        Guid.CreateVersion7(),
        new DateOnly(2026, 5, 4),
        new DateOnly(2026, 5, 8)
    );

    [Fact]
    public void A_draft_copies_the_selected_lines_and_reserves_them_on_the_project()
    {
        var camera = AddLine("Cinema camera kit", 2m, 450m);
        var gaffer = AddLine("Gaffer, 10h day", 1m, 380m);

        var draft = InvoiceDraft.CreateFromProject(project, [camera.Id], "F", Now);

        var line = Assert.Single(draft.Lines);
        Assert.Equal(camera.Id, line.SourceBillableLineId);
        Assert.Equal("Cinema camera kit", line.Description);
        Assert.Equal(900.00m, draft.Totals.TaxableBase.Amount);
        Assert.Equal(1089.00m, draft.Totals.Total.Amount);
        Assert.Equal(BillingState.InDraft, camera.State);
        Assert.Equal(BillingState.Unbilled, gaffer.State);
        Assert.Equal(project.CustomerId, draft.CustomerId);
    }

    [Fact]
    public void Draft_lines_follow_the_project_order_whatever_the_selection_order()
    {
        var camera = AddLine("Cinema camera kit", 2m, 450m);
        var gaffer = AddLine("Gaffer, 10h day", 1m, 380m);
        var van = AddLine("Van with driver", 1m, 220m);

        var draft = InvoiceDraft.CreateFromProject(
            project,
            [van.Id, camera.Id, gaffer.Id],
            "F",
            Now
        );

        Assert.Equal([1, 2, 3], [camera.Position, gaffer.Position, van.Position]);
        Assert.Equal(
            ["Cinema camera kit", "Gaffer, 10h day", "Van with driver"],
            draft.Lines.Select(line => line.Description)
        );
        Assert.Equal([1, 2, 3], draft.Lines.Select(line => line.Position));
    }

    [Fact]
    public void A_draft_needs_at_least_one_line()
    {
        var error = Assert.Throws<BusinessRuleViolationException>(() =>
            InvoiceDraft.CreateFromProject(project, [], "F", Now)
        );

        Assert.Equal("draft.no-lines", error.Code);
    }

    [Fact]
    public void A_complete_draft_has_no_validation_issues()
    {
        var draft = DraftWith(AddLine("Cinema camera kit", 2m, 450m));

        Assert.Empty(draft.Validate(Issuer, Recipient, IssueDate, Sequence()));
    }

    [Fact]
    public void Validation_lists_every_missing_recipient_detail()
    {
        var draft = DraftWith(AddLine("Cinema camera kit", 2m, 450m));
        var incomplete = Recipient with { TaxId = null, Address = null };

        var issues = draft.Validate(Issuer, incomplete, IssueDate, Sequence());

        Assert.Equal(
            ["recipient.tax-id-missing", "recipient.address-missing"],
            issues.Select(issue => issue.Code)
        );
    }

    [Fact]
    public void Validation_requires_an_issuer_profile_with_tax_id()
    {
        var draft = DraftWith(AddLine("Cinema camera kit", 2m, 450m));

        Assert.Equal(
            "issuer.profile-missing",
            Assert.Single(draft.Validate(null, Recipient, IssueDate, Sequence())).Code
        );
        Assert.Equal(
            "issuer.tax-id-missing",
            Assert
                .Single(
                    draft.Validate(Issuer with { TaxId = " " }, Recipient, IssueDate, Sequence())
                )
                .Code
        );
    }

    [Fact]
    public void Validation_rejects_drafts_without_lines_or_with_a_zero_total()
    {
        var free = DraftWith(AddLine("Courtesy recce visit", 1m, 0m));
        var emptied = DraftWith(AddLine("Cinema camera kit", 2m, 450m));
        emptied.RemoveLine(emptied.Lines[0].Id);

        Assert.Equal(
            "draft.total-not-positive",
            Assert.Single(free.Validate(Issuer, Recipient, IssueDate, Sequence())).Code
        );
        Assert.Equal(
            "draft.no-lines",
            Assert.Single(emptied.Validate(Issuer, Recipient, IssueDate, Sequence())).Code
        );
    }

    [Fact]
    public void Validation_rejects_an_issue_date_that_would_break_series_order()
    {
        var draft = DraftWith(AddLine("Cinema camera kit", 2m, 450m));
        var sequence = Sequence();
        sequence.Allocate(IssueDate.AddDays(1));

        var issue = Assert.Single(draft.Validate(Issuer, Recipient, IssueDate, sequence));

        Assert.Equal("numbering.issue-date-not-accepted", issue.Code);
    }

    [Fact]
    public void Removing_a_line_returns_the_billable_line_to_release()
    {
        var camera = AddLine("Cinema camera kit", 2m, 450m);
        var draft = DraftWith(camera);

        var released = draft.RemoveLine(draft.Lines[0].Id);

        Assert.Equal(camera.Id, released);
        Assert.Empty(draft.Lines);
    }

    [Fact]
    public void Finalizing_issues_a_numbered_invoice_with_snapshots_of_parties_and_lines()
    {
        var camera = AddLine("Cinema camera kit", 2m, 450m);
        var gaffer = AddLine("Gaffer, 10h day", 1m, 380m);
        var draft = InvoiceDraft.CreateFromProject(project, [camera.Id, gaffer.Id], "F", Now);
        var sequence = Sequence();

        var invoice = draft.Finalize(Issuer, Recipient, sequence, IssueDate, Now);

        Assert.Equal("F2026-00001", invoice.Number.Value);
        Assert.Equal(IssueDate, invoice.IssueDate);
        Assert.Equal(project.EndDate, invoice.OperationDate);
        Assert.Equal(Now, invoice.IssuedAt);
        Assert.Equal(Issuer, invoice.Issuer);
        Assert.Equal(Recipient, invoice.Recipient);
        Assert.Equal(
            ["Cinema camera kit", "Gaffer, 10h day"],
            invoice.Lines.Select(line => line.Description)
        );
        Assert.Equal(1280.00m, invoice.Totals.TaxableBase.Amount);
        Assert.Equal(268.80m, invoice.Totals.VatAmount.Amount);
        Assert.Equal(1548.80m, invoice.Totals.Total.Amount);
        Assert.Equal(DraftStatus.Issued, draft.Status);
        Assert.Equal(invoice.Id, draft.IssuedInvoiceId);
        Assert.Equal(1, sequence.LastNumber);
    }

    [Fact]
    public void The_operation_date_is_omitted_when_it_equals_the_issue_date()
    {
        var draft = DraftWith(AddLine("Cinema camera kit", 2m, 450m));

        var invoice = draft.Finalize(Issuer, Recipient, Sequence(), project.EndDate, Now);

        Assert.Null(invoice.OperationDate);
    }

    [Fact]
    public void An_invalid_draft_is_not_finalized_and_consumes_no_number()
    {
        var draft = DraftWith(AddLine("Cinema camera kit", 2m, 450m));
        var sequence = Sequence();

        var error = Assert.Throws<BusinessRuleViolationException>(() =>
            draft.Finalize(Issuer, Recipient with { TaxId = null }, sequence, IssueDate, Now)
        );

        Assert.Equal("draft.invalid", error.Code);
        Assert.Equal(0, sequence.LastNumber);
        Assert.Equal(DraftStatus.Open, draft.Status);
    }

    [Fact]
    public void An_issued_draft_can_no_longer_change()
    {
        var draft = DraftWith(AddLine("Cinema camera kit", 2m, 450m));
        draft.Finalize(Issuer, Recipient, Sequence(), IssueDate, Now);

        var finalizeAgain = Assert.Throws<BusinessRuleViolationException>(() =>
            draft.Finalize(Issuer, Recipient, Sequence(), IssueDate, Now)
        );
        var removeLine = Assert.Throws<BusinessRuleViolationException>(() =>
            draft.RemoveLine(draft.Lines[0].Id)
        );

        Assert.Equal("draft.not-open", finalizeAgain.Code);
        Assert.Equal("draft.not-open", removeLine.Code);
    }

    [Fact]
    public void The_draft_must_use_the_sequence_of_its_own_series()
    {
        var draft = DraftWith(AddLine("Cinema camera kit", 2m, 450m));

        var error = Assert.Throws<BusinessRuleViolationException>(() =>
            draft.Finalize(
                Issuer,
                Recipient,
                InvoiceNumberSequence.Start("R", 2026),
                IssueDate,
                Now
            )
        );

        Assert.Equal("numbering.series-mismatch", error.Code);
    }

    private BillableLine AddLine(string description, decimal quantity, decimal unitPrice) =>
        project.AddBillableLine(
            BillableLineKind.Equipment,
            description,
            LinePricing.Create(quantity, unitPrice, 0m, VatRate.Of(21m))
        );

    private InvoiceDraft DraftWith(BillableLine line) =>
        InvoiceDraft.CreateFromProject(project, [line.Id], "F", Now);

    private static InvoiceNumberSequence Sequence() => InvoiceNumberSequence.Start("F", 2026);
}
