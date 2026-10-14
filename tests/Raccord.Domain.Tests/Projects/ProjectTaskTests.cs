using Raccord.Domain.Projects;
using Shouldly;

namespace Raccord.Domain.Tests.Projects;

public sealed class ProjectTaskTests
{
    [Fact]
    public void Create_requires_a_title_and_preserves_board_position()
    {
        var statusId = Guid.CreateVersion7();
        var task = ProjectTask.Create(
            Guid.CreateVersion7(),
            "Prep",
            "Load list",
            statusId,
            2,
            null
        );

        task.Title.ShouldBe("Prep");
        task.Position.ShouldBe(2);
        task.StatusId.ShouldBe(statusId);
    }

    [Fact]
    public void Progress_moves_task_to_a_new_status()
    {
        var task = ProjectTask.Create(
            Guid.CreateVersion7(),
            "Prep",
            null,
            Guid.CreateVersion7(),
            0,
            null
        );
        var statusId = Guid.CreateVersion7();

        task.Progress(statusId, 4);

        task.StatusId.ShouldBe(statusId);
        task.Position.ShouldBe(4);
    }

    [Fact]
    public void Project_rejects_progress_outside_percentage_range()
    {
        var project = Project.Create(
            "Show",
            Guid.CreateVersion7(),
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 1, 2)
        );

        Should.Throw<Exception>(() => project.UpdateProgress(101));
    }
}
