using Raccord.Domain.Common;
using Raccord.Domain.Pricing;
using Raccord.Domain.Projects;

namespace Raccord.Domain.Tests.Projects;

public sealed class ProjectTests
{
    private static readonly Guid CustomerId = Guid.CreateVersion7();

    [Fact]
    public void A_project_cannot_end_before_it_starts()
    {
        var error = Assert.Throws<BusinessRuleViolationException>(
            () => Project.Create("Spring campaign shoot", CustomerId, new DateOnly(2026, 5, 10), new DateOnly(2026, 5, 9)));

        Assert.Equal("projects.period-invalid", error.Code);
    }

    [Fact]
    public void New_billable_lines_are_unbilled()
    {
        var project = NewProject();

        var line = project.AddBillableLine(BillableLineKind.Equipment, "Cinema camera kit", Pricing(2m, 450m));

        Assert.Equal(BillingState.Unbilled, line.State);
        Assert.Contains(line, project.BillableLines);
    }

    [Fact]
    public void Reserving_lines_for_a_draft_moves_them_out_of_the_unbilled_pool()
    {
        var project = NewProject();
        var camera = project.AddBillableLine(BillableLineKind.Equipment, "Cinema camera kit", Pricing(2m, 450m));
        var gaffer = project.AddBillableLine(BillableLineKind.Labour, "Gaffer, 10h day", Pricing(1m, 380m));
        var draftId = Guid.CreateVersion7();

        var reserved = project.ReserveForDraft([camera.Id], draftId);

        Assert.Equal([camera], reserved);
        Assert.Equal(BillingState.InDraft, camera.State);
        Assert.Equal(draftId, camera.DraftId);
        Assert.Equal(BillingState.Unbilled, gaffer.State);
    }

    [Fact]
    public void A_line_already_in_a_draft_cannot_be_reserved_again()
    {
        var project = NewProject();
        var line = project.AddBillableLine(BillableLineKind.Transport, "Van with driver", Pricing(1m, 220m));
        project.ReserveForDraft([line.Id], Guid.CreateVersion7());

        var error = Assert.Throws<BusinessRuleViolationException>(() => project.ReserveForDraft([line.Id], Guid.CreateVersion7()));

        Assert.Equal("billing.line-not-unbilled", error.Code);
    }

    [Fact]
    public void Reserving_an_unknown_line_is_rejected()
    {
        var project = NewProject();

        var error = Assert.Throws<BusinessRuleViolationException>(() => project.ReserveForDraft([Guid.CreateVersion7()], Guid.CreateVersion7()));

        Assert.Equal("projects.billable-line-not-found", error.Code);
    }

    [Fact]
    public void Released_lines_can_be_billed_again()
    {
        var project = NewProject();
        var line = project.AddBillableLine(BillableLineKind.Consumable, "Gel and diffusion pack", Pricing(3m, 18.5m));
        var draftId = Guid.CreateVersion7();
        project.ReserveForDraft([line.Id], draftId);

        project.ReleaseFromDraft(draftId, [line.Id]);

        Assert.Equal(BillingState.Unbilled, line.State);
        Assert.Null(line.DraftId);
    }

    [Fact]
    public void Invoiced_lines_are_never_billable_again()
    {
        var project = NewProject();
        var line = project.AddBillableLine(BillableLineKind.Service, "Colour grading", Pricing(8m, 95m));
        var draftId = Guid.CreateVersion7();
        var invoiceId = Guid.CreateVersion7();
        project.ReserveForDraft([line.Id], draftId);

        project.MarkInvoiced(draftId, invoiceId);
        project.ReleaseFromDraft(draftId, [line.Id]);

        Assert.Equal(BillingState.Invoiced, line.State);
        Assert.Equal(invoiceId, line.InvoiceId);
        var error = Assert.Throws<BusinessRuleViolationException>(() => project.ReserveForDraft([line.Id], Guid.CreateVersion7()));
        Assert.Equal("billing.line-not-unbilled", error.Code);
    }

    [Fact]
    public void Billable_lines_need_a_description()
    {
        var project = NewProject();

        var error = Assert.Throws<BusinessRuleViolationException>(
            () => project.AddBillableLine(BillableLineKind.Other, "  ", Pricing(1m, 10m)));

        Assert.Equal("projects.billable-line-description-required", error.Code);
    }

    private static Project NewProject() =>
        Project.Create("Spring campaign shoot", CustomerId, new DateOnly(2026, 5, 4), new DateOnly(2026, 5, 8));

    private static LinePricing Pricing(decimal quantity, decimal unitPrice) =>
        LinePricing.Create(quantity, unitPrice, 0m, VatRate.Of(21m));
}
