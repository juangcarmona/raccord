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
    public void New_billable_lines_Should_keep_the_selected_vat_class_and_scheme_identity()
    {
        var project = NewProject();
        var vatClassId = Guid.CreateVersion7();
        var vatSchemeId = Guid.CreateVersion7();

        var line = project.AddBillableLine(
            BillableLineKind.Equipment,
            "Cinema camera kit",
            Pricing(2m, 450m),
            vatClassId,
            vatSchemeId
        );

        line.VatClassId.ShouldBe(vatClassId);
        line.VatSchemeId.ShouldBe(vatSchemeId);
        line.VatPercentage.ShouldBe(21m);
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
