using ErrorOr;
using Microsoft.AspNetCore.Http;

namespace Raccord.BuildingBlocks.Api;

/// <summary>Maps ErrorOr failures to the application's problem response contract.</summary>
public static class ApiResults
{
    /// <summary>Creates a problem result for the supplied errors.</summary>
    public static IResult ToProblem(IReadOnlyList<Error> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);
        if (errors.Count == 0)
        {
            throw new ArgumentException("At least one error is required.", nameof(errors));
        }

        var first = errors[0];
        var status = first.NumericType switch
        {
            (int)ErrorType.Validation or 1000 => StatusCodes.Status422UnprocessableEntity,
            (int)ErrorType.Conflict => StatusCodes.Status409Conflict,
            (int)ErrorType.NotFound => StatusCodes.Status404NotFound,
            (int)ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            (int)ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError,
        };
        var issues =
            first.Metadata?.TryGetValue("raccord.issues", out var value) == true
                ? value as IReadOnlyList<ProblemIssue>
                : null;
        if (issues is null && first.Type == ErrorType.Validation)
        {
            issues = [.. errors.Select(error => new ProblemIssue(error.Code, error.Description))];
        }

        var payload = new Dictionary<string, object?>(StringComparer.Ordinal)
        {
            ["type"] = $"urn:raccord:problem:{first.Code}",
            ["title"] =
                first.Metadata?.TryGetValue("raccord.title", out var title) == true
                    ? title as string ?? "Request failed"
                    : "Request failed",
            ["status"] = status,
            ["code"] = first.Code,
            ["detail"] =
                status == StatusCodes.Status500InternalServerError
                    ? "The request could not be completed."
                    : first.Description,
        };
        if (issues is not null)
        {
            payload["issues"] = issues;
        }

        return Results.Json(payload, statusCode: status, contentType: "application/problem+json");
    }
}
