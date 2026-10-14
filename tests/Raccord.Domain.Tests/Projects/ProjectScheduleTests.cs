using Raccord.Domain.Projects;
using Shouldly;

namespace Raccord.Domain.Tests.Projects;

public sealed class ProjectScheduleTests
{
    [Fact]
    public void Creating_a_schedule_Should_preserve_dates_and_assignee()
    {
        var assignee = Guid.CreateVersion7();
        var schedule = ProjectSchedule.Create(
            Guid.CreateVersion7(),
            "Load-in",
            new DateOnly(2026, 2, 1),
            new DateOnly(2026, 2, 2),
            assignee
        );

        schedule.Title.ShouldBe("Load-in");
        schedule.AssigneeId.ShouldBe(assignee);
    }

    [Fact]
    public void Shifting_a_schedule_Should_move_both_boundaries()
    {
        var schedule = ProjectSchedule.Create(
            Guid.CreateVersion7(),
            "Load-in",
            new DateOnly(2026, 2, 1),
            new DateOnly(2026, 2, 2),
            null
        );

        schedule.Shift(3);

        schedule.StartDate.ShouldBe(new DateOnly(2026, 2, 4));
        schedule.EndDate.ShouldBe(new DateOnly(2026, 2, 5));
    }
}
