using System.Globalization;
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

        var feeds = integrations
            .MapGroup("/calendar-feeds")
            .RequirePermission(PermissionCatalog.ApiAccessManage);
        feeds.MapPost("/", IssueCalendarFeedAsync);
        feeds.MapDelete("/{id:guid}", RevokeCalendarFeedAsync);

        integrations.MapGet("/projects", ListProjectsAsync).AllowAnonymous();
        integrations.MapPost("/projects", CreateProjectAsync).AllowAnonymous();
    }

    public static void MapCalendarFeed(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/calendar-feeds/{token}/{stream}.ics",
                async (
                    string token,
                    string stream,
                    CalendarFeedCatalog catalog,
                    HttpResponse response,
                    CancellationToken ct
                ) =>
                {
                    var projects = await catalog.LoadAsync(token, stream, ct);
                    if (projects is null)
                    {
                        response.StatusCode = StatusCodes.Status404NotFound;
                        return;
                    }

                    response.ContentType = "text/calendar; charset=utf-8";
                    await response.WriteAsync(ToCalendar(stream, projects), ct);
                }
            )
            .AllowAnonymous();
    }

    private static async Task<Created<CalendarFeedResponse>> IssueCalendarFeedAsync(
        IssueCalendarFeedRequest request,
        HttpContext context,
        CalendarFeedCatalog catalog,
        CancellationToken ct
    )
    {
        var result = await catalog.IssueAsync(
            request.Stream,
            $"{context.Request.Scheme}://{context.Request.Host}",
            ct
        );
        return TypedResults.Created(result.Url, result);
    }

    private static async Task<Results<NoContent, NotFound>> RevokeCalendarFeedAsync(
        Guid id,
        CalendarFeedCatalog catalog,
        CancellationToken ct
    ) => await catalog.RevokeAsync(id, ct) ? TypedResults.NoContent() : TypedResults.NotFound();

    private static string ToCalendar(
        string stream,
        IReadOnlyList<global::Raccord.Domain.Projects.Project> projects
    )
    {
        var builder = new System.Text.StringBuilder(
            "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nPRODID:-//Raccord//Personal Schedule//EN\r\n"
        );
        foreach (var project in projects)
        {
            builder.Append("BEGIN:VEVENT\r\n");
            builder.Append("UID:").Append(project.Id).Append("@raccord\r\n");
            builder
                .Append("DTSTART;VALUE=DATE:")
                .Append(project.StartDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture));
            builder
                .Append("\r\nDTEND;VALUE=DATE:")
                .Append(
                    project.EndDate.AddDays(1).ToString("yyyyMMdd", CultureInfo.InvariantCulture)
                );
            builder.Append("\r\nSUMMARY:").Append(EscapeCalendar(project.Name));
            if (project.Status == global::Raccord.Domain.Projects.ProjectStatus.Cancelled)
                builder.Append("\r\nSTATUS:CANCELLED");
            builder.Append("\r\nEND:VEVENT\r\n");
        }
        builder.Append("END:VCALENDAR\r\n");
        return builder.ToString();
    }

    private static string EscapeCalendar(string value) =>
        value.Replace("\\", "\\\\").Replace(";", "\\;").Replace(",", "\\,").Replace("\n", "\\n");

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
