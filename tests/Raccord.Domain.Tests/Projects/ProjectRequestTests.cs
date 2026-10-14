using Raccord.Domain.Projects;
using Raccord.SharedKernel.Common;

namespace Raccord.Domain.Tests.Projects;

public sealed class ProjectRequestTests
{
    [Fact]
    public void Receiving_a_request_Should_preserve_the_source_contact_and_start_received()
    {
        var request = ProjectRequest.Receive(
            "Spring campaign",
            "Ada Lovelace",
            "ada@example.com",
            Guid.CreateVersion7(),
            "Three-day shoot",
            new DateOnly(2026, 5, 4),
            new DateOnly(2026, 5, 6),
            Guid.CreateVersion7(),
            Guid.CreateVersion7()
        );

        request.Status.ShouldBe(ProjectRequestStatus.Received);
        request.RequesterName.ShouldBe("Ada Lovelace");
        request.RequesterEmail.ShouldBe("ada@example.com");
        request.Description.ShouldBe("Three-day shoot");
        request.ProjectId.ShouldBeNull();
    }

    [Fact]
    public void Converting_a_request_Should_be_one_way_and_retain_the_project_id()
    {
        var request = NewRequest();
        var projectId = Guid.CreateVersion7();

        request.Convert(projectId);

        request.Status.ShouldBe(ProjectRequestStatus.Converted);
        request.ProjectId.ShouldBe(projectId);
        Should
            .Throw<BusinessRuleViolationException>(() => request.Decline("Too late"))
            .Code.ShouldBe("projects.request-already-resolved");
    }

    [Fact]
    public void Declining_a_request_Should_require_a_reason()
    {
        var request = NewRequest();

        var error = Should.Throw<BusinessRuleViolationException>(() => request.Decline(" "));

        error.Code.ShouldBe("projects.decline-reason-required");
    }

    private static ProjectRequest NewRequest() =>
        ProjectRequest.Receive(
            "Spring campaign",
            "Ada Lovelace",
            "ada@example.com",
            null,
            null,
            new DateOnly(2026, 5, 4),
            new DateOnly(2026, 5, 6),
            null,
            null
        );
}
