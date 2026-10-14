using Raccord.Contracts.Projects;
using Raccord.Domain.Projects;
using Raccord.Modules.Contacts.Contracts;
using Raccord.Modules.Projects.Contracts;
using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Pricing;

namespace Raccord.Modules.Projects;

/// <summary>Coordinates incoming project requests and reusable classifications.</summary>
public sealed class ProjectCoordination(
    IProjectCoordinationStore coordination,
    IBillableWork projects,
    IPartyDirectory parties,
    IProjectsUnitOfWork unitOfWork
)
{
    /// <summary>Lists incoming project requests.</summary>
    public async Task<IReadOnlyList<ProjectRequestResponse>> ListRequestsAsync(
        CancellationToken cancellationToken
    ) =>
        [
            .. (
                await coordination.ListRequestsAsync(cancellationToken).ConfigureAwait(false)
            ).Select(ToResponse),
        ];

    /// <summary>Records an incoming project request.</summary>
    public async Task<ProjectRequestResponse> ReceiveAsync(
        ReceiveProjectRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        await ValidateCustomerAsync(request.CustomerId, cancellationToken).ConfigureAwait(false);
        var projectRequest = ProjectRequest.Receive(
            request.Name,
            request.RequesterName,
            request.RequesterEmail,
            request.CustomerId,
            request.Description,
            request.StartDate,
            request.EndDate,
            request.ProjectTypeId,
            request.TeamId
        );
        coordination.Add(projectRequest);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(projectRequest);
    }

    /// <summary>Converts a request into a project and retains its source record.</summary>
    public async Task<ProjectResponse?> ConvertAsync(
        Guid requestId,
        CancellationToken cancellationToken
    )
    {
        var request = await coordination
            .FindRequestAsync(requestId, cancellationToken)
            .ConfigureAwait(false);
        if (request is null)
            return null;
        if (request.CustomerId is not Guid customerId)
            throw new BusinessRuleViolationException(
                "projects.request-customer-required",
                "A customer must be linked before converting the request."
            );
        var customer = await parties.FindAsync(customerId, cancellationToken).ConfigureAwait(false);
        if (customer is null)
            throw new BusinessRuleViolationException(
                "customers.not-found",
                $"Customer '{customerId}' does not exist."
            );
        var project = Project.Create(
            request.Name,
            customerId,
            request.StartDate,
            request.EndDate,
            request.ProjectTypeId,
            request.TeamId
        );
        projects.Add(project);
        request.Convert(project.Id);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return new ProjectResponse(
            project.Id,
            project.Name,
            project.CustomerId,
            customer.LegalName,
            project.StartDate,
            project.EndDate,
            Currency.Eur.Code,
            [],
            project.ProjectTypeId,
            project.TeamId
        );
    }

    /// <summary>Declines an incoming request.</summary>
    public async Task<ProjectRequestResponse?> DeclineAsync(
        Guid requestId,
        DeclineProjectRequest input,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(input);
        var request = await coordination
            .FindRequestAsync(requestId, cancellationToken)
            .ConfigureAwait(false);
        if (request is null)
            return null;
        request.Decline(input.Reason);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(request);
    }

    /// <summary>Creates a reusable project type.</summary>
    public async Task<ProjectTypeResponse> CreateTypeAsync(
        CreateProjectClassificationRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var projectType = ProjectType.Create(request.Name, request.Description);
        coordination.Add(projectType);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return new(projectType.Id, projectType.Name, projectType.Description);
    }

    /// <summary>Creates a reusable project team.</summary>
    public async Task<ProjectTeamResponse> CreateTeamAsync(
        CreateProjectClassificationRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var team = ProjectTeam.Create(request.Name, request.Description);
        coordination.Add(team);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return new(team.Id, team.Name, team.Description);
    }

    /// <summary>Lists reusable project types.</summary>
    public async Task<IReadOnlyList<ProjectTypeResponse>> ListTypesAsync(
        CancellationToken cancellationToken
    ) =>
        [
            .. (
                await coordination.ListProjectTypesAsync(cancellationToken).ConfigureAwait(false)
            ).Select(type => new ProjectTypeResponse(type.Id, type.Name, type.Description)),
        ];

    /// <summary>Lists reusable project teams.</summary>
    public async Task<IReadOnlyList<ProjectTeamResponse>> ListTeamsAsync(
        CancellationToken cancellationToken
    ) =>
        [
            .. (await coordination.ListTeamsAsync(cancellationToken).ConfigureAwait(false)).Select(
                team => new ProjectTeamResponse(team.Id, team.Name, team.Description)
            ),
        ];

    private async Task ValidateCustomerAsync(Guid? customerId, CancellationToken cancellationToken)
    {
        if (
            customerId is Guid id
            && await parties.FindAsync(id, cancellationToken).ConfigureAwait(false) is null
        )
            throw new BusinessRuleViolationException(
                "customers.not-found",
                $"Customer '{id}' does not exist."
            );
    }

    private static ProjectRequestResponse ToResponse(ProjectRequest request) =>
        new(
            request.Id,
            request.Name,
            request.RequesterName,
            request.RequesterEmail,
            request.CustomerId,
            request.Description,
            request.StartDate,
            request.EndDate,
            request.ProjectTypeId,
            request.TeamId,
            request.Status.ToString().ToLowerInvariant(),
            request.ProjectId,
            request.DeclineReason,
            request.ReceivedAt,
            request.ResolvedAt
        );
}
