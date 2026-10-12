using Raccord.Domain.Billing;
using Raccord.Domain.Projects;
using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Parties;
using Raccord.SharedKernel.Pricing;

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
    public void A_draft_Should_copy_the_selected_lines_and_reserve_them_on_the_project()
    {
        var camera = AddLine("Cinema camera kit", 2m, 450m);
        var gaffer = AddLine("Gaffer, 10h day", 1m, 380m);

        var draft = InvoiceDraft.CreateFromProject(project, [camera.Id], "F", Now);

        var line = draft.Lines.ShouldHaveSingleItem();
        line.SourceBillableLineId.ShouldBe(camera.Id);
        line.Description.ShouldBe("Cinema camera kit");
        draft.Totals.TaxableBase.Amount.ShouldBe(900.00m);
        draft.Totals.Total.Amount.ShouldBe(1089.00m);
        camera.State.ShouldBe(BillingState.InDraft);
        gaffer.State.ShouldBe(BillingState.Unbilled);
        draft.CustomerId.ShouldBe(project.CustomerId);
    }

    [Fact]
    public void Draft_lines_Should_follow_the_project_order_whatever_the_selection_order()
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

        new[] { camera.Position, gaffer.Position, van.Position }.ShouldBe([1, 2, 3]);
        draft
            .Lines.Select(line => line.Description)
            .ShouldBe(["Cinema camera kit", "Gaffer, 10h day", "Van with driver"]);
        draft.Lines.Select(line => line.Position).ShouldBe([1, 2, 3]);
    }

    [Fact]
    public void A_draft_Should_require_at_least_one_line()
    {
        var error = Should.Throw<BusinessRuleViolationException>(() =>
            InvoiceDraft.CreateFromProject(project, [], "F", Now)
        );

        error.Code.ShouldBe("draft.no-lines");
    }

    [Fact]
    public void A_complete_draft_Should_have_no_validation_issues()
    {
        var draft = DraftWith(AddLine("Cinema camera kit", 2m, 450m));

        draft.Validate(Issuer, Recipient, IssueDate, Sequence()).ShouldBeEmpty();
    }

    [Fact]
    public void Validation_Should_list_every_missing_recipient_detail()
    {
        var draft = DraftWith(AddLine("Cinema camera kit", 2m, 450m));
        var incomplete = Recipient with { TaxId = null, Address = null };

        var issues = draft.Validate(Issuer, incomplete, IssueDate, Sequence());

        issues
            .Select(issue => issue.Code)
            .ShouldBe(["recipient.tax-id-missing", "recipient.address-missing"]);
    }

    [Fact]
    public void Validation_Should_require_an_issuer_profile_with_tax_id()
    {
        var draft = DraftWith(AddLine("Cinema camera kit", 2m, 450m));

        draft
            .Validate(null, Recipient, IssueDate, Sequence())
            .ShouldHaveSingleItem()
            .Code.ShouldBe("issuer.profile-missing");
        draft
            .Validate(Issuer with { TaxId = " " }, Recipient, IssueDate, Sequence())
            .ShouldHaveSingleItem()
            .Code.ShouldBe("issuer.tax-id-missing");
    }

    [Fact]
    public void Validation_Should_reject_drafts_without_lines_or_with_a_zero_total()
    {
        var free = DraftWith(AddLine("Courtesy recce visit", 1m, 0m));
        var emptied = DraftWith(AddLine("Cinema camera kit", 2m, 450m));
        emptied.RemoveLine(emptied.Lines[0].Id);

        free.Validate(Issuer, Recipient, IssueDate, Sequence())
            .ShouldHaveSingleItem()
            .Code.ShouldBe("draft.total-not-positive");
        emptied
            .Validate(Issuer, Recipient, IssueDate, Sequence())
            .ShouldHaveSingleItem()
            .Code.ShouldBe("draft.no-lines");
    }

    [Fact]
    public void Validation_Should_reject_an_issue_date_that_would_break_series_order()
    {
        var draft = DraftWith(AddLine("Cinema camera kit", 2m, 450m));
        var sequence = Sequence();
        sequence.Allocate(IssueDate.AddDays(1));

        var issue = draft.Validate(Issuer, Recipient, IssueDate, sequence).ShouldHaveSingleItem();

        issue.Code.ShouldBe("numbering.issue-date-not-accepted");
    }

    [Fact]
    public void Removing_a_line_Should_return_the_billable_line_to_release()
    {
        var camera = AddLine("Cinema camera kit", 2m, 450m);
        var draft = DraftWith(camera);

        var released = draft.RemoveLine(draft.Lines[0].Id);

        released.ShouldBe(camera.Id);
        draft.Lines.ShouldBeEmpty();
    }

    [Fact]
    public void Finalizing_Should_issue_a_numbered_invoice_with_snapshots_of_parties_and_lines()
    {
        var camera = AddLine("Cinema camera kit", 2m, 450m);
        var gaffer = AddLine("Gaffer, 10h day", 1m, 380m);
        var draft = InvoiceDraft.CreateFromProject(project, [camera.Id, gaffer.Id], "F", Now);
        var sequence = Sequence();

        var invoice = draft.Finalize(Issuer, Recipient, sequence, IssueDate, Now);

        invoice.Number.Value.ShouldBe("F2026-00001");
        invoice.IssueDate.ShouldBe(IssueDate);
        invoice.OperationDate.ShouldBe(project.EndDate);
        invoice.IssuedAt.ShouldBe(Now);
        invoice.Issuer.ShouldBe(Issuer);
        invoice.Recipient.ShouldBe(Recipient);
        invoice
            .Lines.Select(line => line.Description)
            .ShouldBe(["Cinema camera kit", "Gaffer, 10h day"]);
        invoice.Totals.TaxableBase.Amount.ShouldBe(1280.00m);
        invoice.Totals.VatAmount.Amount.ShouldBe(268.80m);
        invoice.Totals.Total.Amount.ShouldBe(1548.80m);
        draft.Status.ShouldBe(DraftStatus.Issued);
        draft.IssuedInvoiceId.ShouldBe(invoice.Id);
        sequence.LastNumber.ShouldBe(1);
    }

    [Fact]
    public void The_operation_date_Should_be_omitted_when_it_equals_the_issue_date()
    {
        var draft = DraftWith(AddLine("Cinema camera kit", 2m, 450m));

        var invoice = draft.Finalize(Issuer, Recipient, Sequence(), project.EndDate, Now);

        invoice.OperationDate.ShouldBeNull();
    }

    [Fact]
    public void An_invalid_draft_Should_not_be_finalized_and_consume_no_number()
    {
        var draft = DraftWith(AddLine("Cinema camera kit", 2m, 450m));
        var sequence = Sequence();

        var error = Should.Throw<BusinessRuleViolationException>(() =>
            draft.Finalize(Issuer, Recipient with { TaxId = null }, sequence, IssueDate, Now)
        );

        error.Code.ShouldBe("draft.invalid");
        sequence.LastNumber.ShouldBe(0);
        draft.Status.ShouldBe(DraftStatus.Open);
    }

    [Fact]
    public void An_issued_draft_Should_no_longer_change()
    {
        var draft = DraftWith(AddLine("Cinema camera kit", 2m, 450m));
        draft.Finalize(Issuer, Recipient, Sequence(), IssueDate, Now);

        var finalizeAgain = Should.Throw<BusinessRuleViolationException>(() =>
            draft.Finalize(Issuer, Recipient, Sequence(), IssueDate, Now)
        );
        var removeLine = Should.Throw<BusinessRuleViolationException>(() =>
            draft.RemoveLine(draft.Lines[0].Id)
        );

        finalizeAgain.Code.ShouldBe("draft.not-open");
        removeLine.Code.ShouldBe("draft.not-open");
    }

    [Fact]
    public void The_draft_Should_use_the_sequence_of_its_own_series()
    {
        var draft = DraftWith(AddLine("Cinema camera kit", 2m, 450m));

        var error = Should.Throw<BusinessRuleViolationException>(() =>
            draft.Finalize(
                Issuer,
                Recipient,
                InvoiceNumberSequence.Start("R", 2026),
                IssueDate,
                Now
            )
        );

        error.Code.ShouldBe("numbering.series-mismatch");
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
