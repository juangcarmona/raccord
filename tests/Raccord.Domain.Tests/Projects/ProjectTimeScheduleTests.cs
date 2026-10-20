using Raccord.Domain.Projects;
using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Pricing;
using Shouldly;

namespace Raccord.Domain.Tests.Projects;

public sealed class ProjectTimeScheduleTests
{
    [Fact]
    public void Creating_a_project_schedule_Should_add_unset_planning_and_usage_periods()
    {
        var project = NewProject();

        project.TimePeriods.Count.ShouldBe(2);
        project.TimePeriods.Single(period => period.IsDefaultPlanning).Name.ShouldBe("Planning");
        project.TimePeriods.Single(period => period.IsDefaultUsage).Name.ShouldBe("Usage");
        foreach (var period in project.TimePeriods)
        {
            period.Start.ShouldBeNull();
            period.End.ShouldBeNull();
            period.ShowOnInternalDocuments.ShouldBeTrue();
            period.ShowOnExternalDocuments.ShouldBeFalse();
        }
    }

    [Fact]
    public void Setting_a_usage_period_inside_planning_Should_preserve_both_ranges()
    {
        var project = NewProject();
        var planning = project.TimePeriods.Single(period => period.IsDefaultPlanning);
        var usage = project.TimePeriods.Single(period => period.IsDefaultUsage);
        var planningStart = new DateTimeOffset(2026, 5, 1, 8, 0, 0, TimeSpan.Zero);
        var usageStart = planningStart.AddHours(2);
        var usageEnd = usageStart.AddDays(1);
        var planningEnd = usageEnd.AddHours(4);

        project.SetTimePeriod(planning.Id, planningStart, planningEnd);
        project.SetTimePeriod(usage.Id, usageStart, usageEnd);

        usage.Start.ShouldBe(usageStart);
        usage.End.ShouldBe(usageEnd);
    }

    [Fact]
    public void Setting_usage_equal_to_planning_Should_be_rejected_without_changing_either_period()
    {
        var project = NewProject();
        var planning = project.TimePeriods.Single(period => period.IsDefaultPlanning);
        var usage = project.TimePeriods.Single(period => period.IsDefaultUsage);
        var start = new DateTimeOffset(2026, 5, 1, 8, 0, 0, TimeSpan.Zero);
        var end = start.AddDays(1);
        project.SetTimePeriod(planning.Id, start, end);

        var error = Should.Throw<BusinessRuleViolationException>(() =>
            project.SetTimePeriod(usage.Id, start, end)
        );

        error.Code.ShouldBe("projects.usage-period-outside-planning");
        usage.Start.ShouldBeNull();
        usage.End.ShouldBeNull();
    }

    [Fact]
    public void Adding_and_editing_a_named_period_Should_preserve_document_visibility_settings()
    {
        var project = NewProject();
        var period = project.AddTimePeriod("Load-in");

        project.UpdateTimePeriod(
            period.Id,
            "Build",
            new DateTimeOffset(2026, 5, 1, 8, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2026, 5, 1, 18, 0, 0, TimeSpan.Zero),
            showOnInternalDocuments: true,
            showOnExternalDocuments: false
        );

        period.Name.ShouldBe("Build");
        period.ShowOnInternalDocuments.ShouldBeTrue();
        period.ShowOnExternalDocuments.ShouldBeFalse();
    }

    [Fact]
    public void Duplicating_a_period_Should_insert_an_undesignated_copy_after_it()
    {
        var project = NewProject();
        var original = project.AddTimePeriod("Show");
        var copy = project.DuplicateTimePeriod(original.Id);

        copy.Name.ShouldBe("Show copy");
        copy.IsDefaultPlanning.ShouldBeFalse();
        copy.IsDefaultUsage.ShouldBeFalse();
        copy.Position.ShouldBe(original.Position + 1);
    }

    [Fact]
    public void Reordering_periods_Should_assign_each_period_one_position()
    {
        var project = NewProject();
        var original = project.TimePeriods.Select(period => period.Id).ToList();
        var added = project.AddTimePeriod("Load-in");
        var ordered = new[] { added.Id, original[0], original[1] };

        project.ReorderTimePeriods(ordered);

        project.TimePeriods.Select(period => period.Id).ToArray().ShouldBe(ordered);
        project.TimePeriods.Select(period => period.Position).ShouldBe([0, 1, 2]);
    }

    [Fact]
    public void Designating_a_named_period_Should_move_the_default_role_without_duplicating_it()
    {
        var project = NewProject();
        var originalPlanning = project.TimePeriods.Single(period => period.IsDefaultPlanning);
        var selected = project.AddTimePeriod("Show");

        project.DesignateTimePeriod(selected.Id, isDefaultPlanning: true, isDefaultUsage: false);

        originalPlanning.IsDefaultPlanning.ShouldBeFalse();
        selected.IsDefaultPlanning.ShouldBeTrue();
        project.TimePeriods.Count(period => period.IsDefaultPlanning).ShouldBe(1);
        project.TimePeriods.Count(period => period.IsDefaultUsage).ShouldBe(1);
    }

    [Fact]
    public void Reordering_with_a_missing_period_Should_be_rejected_without_changing_the_schedule()
    {
        var project = NewProject();
        var original = project.TimePeriods.Select(period => period.Id).ToArray();

        var error = Should.Throw<BusinessRuleViolationException>(() =>
            project.ReorderTimePeriods([original[0], Guid.CreateVersion7()])
        );

        error.Code.ShouldBe("projects.time-period-order-invalid");
        project.TimePeriods.Select(period => period.Id).ToArray().ShouldBe(original);
    }

    [Fact]
    public void Updating_a_period_outside_the_designated_planning_period_Should_be_rejected()
    {
        var project = NewProject();
        var planning = project.TimePeriods.Single(period => period.IsDefaultPlanning);
        var usage = project.TimePeriods.Single(period => period.IsDefaultUsage);
        var planningStart = new DateTimeOffset(2026, 5, 1, 8, 0, 0, TimeSpan.Zero);
        project.SetTimePeriod(usage.Id, planningStart.AddDays(4), planningStart.AddDays(5));

        var error = Should.Throw<BusinessRuleViolationException>(() =>
            project.SetTimePeriod(planning.Id, planningStart, planningStart.AddDays(3))
        );

        error.Code.ShouldBe("projects.usage-period-outside-planning");
        planning.Start.ShouldBeNull();
        planning.End.ShouldBeNull();
        usage.Start.ShouldBe(planningStart.AddDays(4));
        usage.End.ShouldBe(planningStart.AddDays(5));
    }

    [Fact]
    public void A_completed_project_Should_block_operational_edits_until_reopened()
    {
        var project = NewProject();
        project.ChangeStatus(ProjectStatus.Complete);

        var periodError = Should.Throw<BusinessRuleViolationException>(() =>
            project.AddTimePeriod("Show")
        );
        var equipmentError = Should.Throw<BusinessRuleViolationException>(() =>
            project.AddPackingLine(Guid.CreateVersion7(), 1)
        );
        var billingError = Should.Throw<BusinessRuleViolationException>(() =>
            project.AddBillableLine(
                BillableLineKind.Service,
                "Production support",
                LinePricing.Create(1m, 100m, 0m, VatRate.Of(21m))
            )
        );

        periodError.Code.ShouldBe("projects.complete-read-only");
        equipmentError.Code.ShouldBe("projects.complete-read-only");
        billingError.Code.ShouldBe("projects.complete-read-only");
        project.TimePeriods.Count.ShouldBe(2);
        project.PackingLines.ShouldBeEmpty();
        project.BillableLines.ShouldBeEmpty();

        project.ChangeStatus(ProjectStatus.InProgress);
        project.AddTimePeriod("Show");
        project.AddPackingLine(Guid.CreateVersion7(), 1);
        project.AddBillableLine(
            BillableLineKind.Service,
            "Production support",
            LinePricing.Create(1m, 100m, 0m, VatRate.Of(21m))
        );
        project.PackingLines.Count.ShouldBe(1);
        project.BillableLines.Count.ShouldBe(1);
        project.TimePeriods.Count.ShouldBe(3);
    }

    private static Project NewProject() =>
        Project.Create(
            "Campaign",
            Guid.CreateVersion7(),
            new DateOnly(2026, 5, 1),
            new DateOnly(2026, 5, 3)
        );
}
