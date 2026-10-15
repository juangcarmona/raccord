using Microsoft.AspNetCore.Http.HttpResults;
using Raccord.Application.Projects;
using Raccord.Contracts.Integrations;
using Raccord.Contracts.Projects;
using Raccord.Infrastructure.Persistence;
using Raccord.Web.Authorization;

namespace Raccord.Web.Api;

internal static class IntegrationEndpoints
{
    public static void MapIntegrations(RouteGroupBuilder api)
    {
        var integrations = api.MapGroup("/integrations").WithTags("Integrations");
        var tokens = integrations
            .MapGroup("/api-tokens")
            .RequirePermission(PermissionCatalog.ApiAccessManage);
        tokens.MapGet("/", ListTokensAsync);
        tokens.MapPost("/", IssueTokenAsync);
        tokens.MapPost("/{id:guid}/regenerate", RegenerateTokenAsync);

        integrations.MapGet("/projects", ListProjectsAsync).AllowAnonymous();
        integrations.MapPost("/projects", CreateProjectAsync).AllowAnonymous();
    }

    private static async Task<Ok<IReadOnlyList<ApiTokenResponse>>> ListTokensAsync(
        ApiAccessCatalog catalog,
        CancellationToken ct
    ) => TypedResults.Ok(await catalog.ListAsync(ct));

    private static async Task<
        Results<Created<ApiTokenResponse>, BadRequest<string>>
    > IssueTokenAsync(IssueApiTokenRequest request, ApiAccessCatalog catalog, CancellationToken ct)
    {
        if (
            string.IsNullOrWhiteSpace(request.Name)
            || request.Scopes is null
            || request.Scopes.Count == 0
            || request.Scopes.Any(scope =>
                !ApiAccessScopes.All.Contains(scope, StringComparer.Ordinal)
            )
        )
        {
            return TypedResults.BadRequest("A name and only supported scopes are required.");
        }

        var token = await catalog.IssueAsync(request, ct);
        return TypedResults.Created($"/api/integrations/api-tokens/{token.Id}", token);
    }

    private static async Task<Results<Ok<ApiTokenResponse>, NotFound>> RegenerateTokenAsync(
        Guid id,
        ApiAccessCatalog catalog,
        CancellationToken ct
    ) =>
        await catalog.RegenerateAsync(id, ct) is { } token
            ? TypedResults.Ok(token)
            : TypedResults.NotFound();

    private static async Task<
        Results<Ok<IReadOnlyList<IntegrationProjectResponse>>, UnauthorizedHttpResult>
    > ListProjectsAsync(
        HttpContext context,
        ApiAccessCatalog tokens,
        ProjectWorkspace projects,
        CancellationToken ct
    )
    {
        if (!await tokens.HasScopeAsync(GetToken(context), ApiAccessScopes.ProjectsRead, ct))
        {
            return TypedResults.Unauthorized();
        }

        var result = (await projects.ListAsync(ct))
            .Select(project => new IntegrationProjectResponse(
                project.Id,
                project.Name,
                project.StartDate,
                project.EndDate
            ))
            .ToArray();
        return TypedResults.Ok<IReadOnlyList<IntegrationProjectResponse>>(result);
    }

    private static async Task<
        Results<Created<IntegrationProjectResponse>, UnauthorizedHttpResult>
    > CreateProjectAsync(
        HttpContext context,
        CreateProjectRequest request,
        ApiAccessCatalog tokens,
        ProjectWorkspace projects,
        CancellationToken ct
    )
    {
        if (!await tokens.HasScopeAsync(GetToken(context), ApiAccessScopes.ProjectsWrite, ct))
        {
            return TypedResults.Unauthorized();
        }

        var project = await projects.CreateAsync(request, ct);
        return TypedResults.Created(
            $"/api/integrations/projects/{project.Id}",
            new IntegrationProjectResponse(
                project.Id,
                project.Name,
                project.StartDate,
                project.EndDate
            )
        );
    }

    private static string? GetToken(HttpContext context) =>
        context.Request.Headers["X-Api-Key"].FirstOrDefault();
}
