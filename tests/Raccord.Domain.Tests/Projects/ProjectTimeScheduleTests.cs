using Raccord.Domain.Projects;
using Raccord.SharedKernel.Common;
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

    private static Project NewProject() =>
        Project.Create(
            "Campaign",
            Guid.CreateVersion7(),
            new DateOnly(2026, 5, 1),
            new DateOnly(2026, 5, 3)
        );
}
