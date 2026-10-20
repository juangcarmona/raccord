using Raccord.Domain.Projects;
using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Pricing;

namespace Raccord.Domain.Tests.Projects;

public sealed class ProjectTests
{
    private static readonly Guid CustomerId = Guid.CreateVersion7();

    [Fact]
    public void A_project_Should_not_end_before_it_starts()
    {
        var error = Should.Throw<BusinessRuleViolationException>(() =>
            Project.Create(
                "Spring campaign shoot",
                CustomerId,
                new DateOnly(2026, 5, 10),
                new DateOnly(2026, 5, 9)
            )
        );

        error.Code.ShouldBe("projects.period-invalid");
    }

    [Fact]
    public void New_billable_lines_Should_be_unbilled()
    {
        var project = NewProject();

        var line = project.AddBillableLine(
            BillableLineKind.Equipment,
            "Cinema camera kit",
            Pricing(2m, 450m)
        );

        line.State.ShouldBe(BillingState.Unbilled);
        project.BillableLines.ShouldContain(line);
    }

    [Fact]
    public void A_new_project_Should_start_in_draft_status()
    {
        NewProject().Status.ShouldBe(ProjectStatus.Draft);
    }

    [Fact]
    public void A_project_Should_allow_manual_status_changes_without_changing_warehouse_state()
    {
        var project = NewProject();
        var line = project.AddPackingLine(Guid.CreateVersion7(), 1);

        project.ChangeStatus(ProjectStatus.InProgress);

        project.Status.ShouldBe(ProjectStatus.InProgress);
        line.IsPacked.ShouldBeFalse();
        line.IsBookedOut.ShouldBeFalse();
        line.IsReceived.ShouldBeFalse();
    }

    [Fact]
    public void Cancelling_a_project_Should_preserve_its_invoiced_work()
    {
        var project = NewProject();
        var line = project.AddBillableLine(
            BillableLineKind.Service,
            "Colour grading",
            Pricing(8m, 95m)
        );
        var draftId = Guid.CreateVersion7();
        var invoiceId = Guid.CreateVersion7();
        project.ReserveForDraft([line.Id], draftId);
        project.MarkInvoiced(draftId, invoiceId);

        project.Cancel(
            hasOpenItems: false,
            cancelledAt: new DateTimeOffset(2026, 5, 1, 12, 0, 0, TimeSpan.Zero)
        );

        project.Status.ShouldBe(ProjectStatus.Cancelled);
        line.State.ShouldBe(BillingState.Invoiced);
        line.InvoiceId.ShouldBe(invoiceId);
        line.Description.ShouldBe("Colour grading");
    }

    [Fact]
    public void Changing_project_status_Should_preserve_its_invoiced_work()
    {
        var project = NewProject();
        var line = project.AddBillableLine(
            BillableLineKind.Service,
            "Colour grading",
            Pricing(8m, 95m)
        );
        var draftId = Guid.CreateVersion7();
        var invoiceId = Guid.CreateVersion7();
        project.ReserveForDraft([line.Id], draftId);
        project.MarkInvoiced(draftId, invoiceId);

        project.ChangeStatus(ProjectStatus.Confirmed);

        line.State.ShouldBe(BillingState.Invoiced);
        line.InvoiceId.ShouldBe(invoiceId);
        line.Description.ShouldBe("Colour grading");
    }

    [Fact]
    public void Cancelling_a_project_with_open_items_Should_keep_a_persistent_warning()
    {
        var project = NewProject();
        var cancelledAt = new DateTimeOffset(2026, 5, 1, 12, 0, 0, TimeSpan.Zero);

        project.Cancel(hasOpenItems: true, cancelledAt);

        project.Status.ShouldBe(ProjectStatus.Cancelled);
        project.HasCancellationWarning.ShouldBeTrue();
        project.CancelledAt.ShouldBe(cancelledAt);
    }

    [Fact]
    public void Changing_status_directly_to_cancelled_Should_require_the_cancellation_flow()
    {
        var project = NewProject();

        var error = Should.Throw<BusinessRuleViolationException>(() =>
            project.ChangeStatus(ProjectStatus.Cancelled)
        );

        error.Code.ShouldBe("projects.cancellation-confirmation-required");
    }

    [Fact]
    public void Editing_project_details_Should_update_the_name_and_period()
    {
        var project = NewProject();
        var start = new DateOnly(2026, 6, 1);
        var end = new DateOnly(2026, 6, 3);

        project.EditDetails(CustomerId, "  Summer campaign  ", start, end, null, null);

        project.Name.ShouldBe("Summer campaign");
        project.StartDate.ShouldBe(start);
        project.EndDate.ShouldBe(end);
    }

    [Fact]
    public void Editing_project_details_Should_trim_and_persist_the_external_reference()
    {
        var project = NewProject();

        project.EditDetails(
            CustomerId,
            "Summer campaign",
            new DateOnly(2026, 6, 1),
            new DateOnly(2026, 6, 3),
            null,
            null,
            "  PO-2026-041  "
        );

        project.ExternalReference.ShouldBe("PO-2026-041");
    }

    [Fact]
    public void Editing_project_details_Should_reject_an_overlong_external_reference_without_mutation()
    {
        var project = NewProject();
        var externalReference = new string('x', 201);

        var error = Should.Throw<BusinessRuleViolationException>(() =>
            project.EditDetails(
                CustomerId,
                "Summer campaign",
                new DateOnly(2026, 6, 1),
                new DateOnly(2026, 6, 3),
                null,
                null,
                externalReference
            )
        );

        error.Code.ShouldBe("projects.external-reference-too-long");
        project.ExternalReference.ShouldBeNull();
    }

    [Fact]
    public void Editing_project_details_Should_reject_an_invalid_period_without_changing_the_project()
    {
        var project = NewProject();

        var error = Should.Throw<BusinessRuleViolationException>(() =>
            project.EditDetails(
                CustomerId,
                "Summer campaign",
                new DateOnly(2026, 6, 3),
                new DateOnly(2026, 6, 1),
                null,
                null
            )
        );

        error.Code.ShouldBe("projects.period-invalid");
        project.Name.ShouldBe("Spring campaign shoot");
        project.StartDate.ShouldBe(new DateOnly(2026, 5, 4));
        project.EndDate.ShouldBe(new DateOnly(2026, 5, 8));
    }

    [Fact]
    public void A_completed_project_Should_require_reopening_before_details_are_edited()
    {
        var project = NewProject();
        project.ChangeStatus(ProjectStatus.Complete);

        var error = Should.Throw<BusinessRuleViolationException>(() =>
            project.EditDetails(
                CustomerId,
                "Summer campaign",
                new DateOnly(2026, 6, 1),
                new DateOnly(2026, 6, 3),
                null,
                null
            )
        );

        error.Code.ShouldBe("projects.complete-read-only");
        project.Name.ShouldBe("Spring campaign shoot");
    }

    [Fact]
    public void Reserving_lines_for_a_draft_Should_move_them_out_of_the_unbilled_pool()
    {
        var project = NewProject();
        var camera = project.AddBillableLine(
            BillableLineKind.Equipment,
            "Cinema camera kit",
            Pricing(2m, 450m)
        );
        var gaffer = project.AddBillableLine(
            BillableLineKind.Labour,
            "Gaffer, 10h day",
            Pricing(1m, 380m)
        );
        var draftId = Guid.CreateVersion7();

        var reserved = project.ReserveForDraft([camera.Id], draftId);

        reserved.ShouldBe([camera]);
        camera.State.ShouldBe(BillingState.InDraft);
        camera.DraftId.ShouldBe(draftId);
        gaffer.State.ShouldBe(BillingState.Unbilled);
    }

    [Fact]
    public void A_line_already_in_a_draft_Should_not_be_reserved_again()
    {
        var project = NewProject();
        var line = project.AddBillableLine(
            BillableLineKind.Transport,
            "Van with driver",
            Pricing(1m, 220m)
        );
        project.ReserveForDraft([line.Id], Guid.CreateVersion7());

        var error = Should.Throw<BusinessRuleViolationException>(() =>
            project.ReserveForDraft([line.Id], Guid.CreateVersion7())
        );

        error.Code.ShouldBe("billing.line-not-unbilled");
    }

    [Fact]
    public void Reserving_an_unknown_line_Should_be_rejected()
    {
        var project = NewProject();

        var error = Should.Throw<BusinessRuleViolationException>(() =>
            project.ReserveForDraft([Guid.CreateVersion7()], Guid.CreateVersion7())
        );

        error.Code.ShouldBe("projects.billable-line-not-found");
    }

    [Fact]
    public void Released_lines_Should_be_billable_again()
    {
        var project = NewProject();
        var line = project.AddBillableLine(
            BillableLineKind.Consumable,
            "Gel and diffusion pack",
            Pricing(3m, 18.5m)
        );
        var draftId = Guid.CreateVersion7();
        project.ReserveForDraft([line.Id], draftId);

        project.ReleaseFromDraft(draftId, [line.Id]);

        line.State.ShouldBe(BillingState.Unbilled);
        line.DraftId.ShouldBeNull();
    }

    [Fact]
    public void Invoiced_lines_Should_never_be_billable_again()
    {
        var project = NewProject();
        var line = project.AddBillableLine(
            BillableLineKind.Service,
            "Colour grading",
            Pricing(8m, 95m)
        );
        var draftId = Guid.CreateVersion7();
        var invoiceId = Guid.CreateVersion7();
        project.ReserveForDraft([line.Id], draftId);

        project.MarkInvoiced(draftId, invoiceId);
        project.ReleaseFromDraft(draftId, [line.Id]);

        line.State.ShouldBe(BillingState.Invoiced);
        line.InvoiceId.ShouldBe(invoiceId);
        Should
            .Throw<BusinessRuleViolationException>(() =>
                project.ReserveForDraft([line.Id], Guid.CreateVersion7())
            )
            .Code.ShouldBe("billing.line-not-unbilled");
    }

    [Fact]
    public void Billable_lines_Should_require_a_description()
    {
        var project = NewProject();

        var error = Should.Throw<BusinessRuleViolationException>(() =>
            project.AddBillableLine(BillableLineKind.Other, "  ", Pricing(1m, 10m))
        );

        error.Code.ShouldBe("projects.billable-line-description-required");
    }

    private static Project NewProject() =>
        Project.Create(
            "Spring campaign shoot",
            CustomerId,
            new DateOnly(2026, 5, 4),
            new DateOnly(2026, 5, 8)
        );

    private static LinePricing Pricing(decimal quantity, decimal unitPrice) =>
        LinePricing.Create(quantity, unitPrice, 0m, VatRate.Of(21m));
}
