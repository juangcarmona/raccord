#pragma warning disable CS1591
namespace Raccord.Contracts.Integrations;

public static class ApiAccessScopes
{
    public const string ProjectsRead = "projects.read";
    public const string ProjectsWrite = "projects.write";

    public static readonly IReadOnlySet<string> All = new HashSet<string>(
        [ProjectsRead, ProjectsWrite],
        StringComparer.Ordinal
    );
}

public sealed record IssueApiTokenRequest(string Name, IReadOnlyList<string> Scopes);

public sealed record ApiTokenResponse(
    Guid Id,
    string Name,
    IReadOnlyList<string> Scopes,
    DateTimeOffset CreatedAt,
    string? Token = null
);

public sealed record IntegrationProjectResponse(
    Guid Id,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate
);
#pragma warning restore CS1591
