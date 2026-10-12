using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Raccord.Application.Abstractions;
using Raccord.Application.Invoicing;
using Raccord.Contracts.Common;
using Raccord.SharedKernel.Common;

namespace Raccord.Web.Api;

/// <summary>Translates domain and application failures into RFC 9457 problem details with stable codes.</summary>
internal sealed partial class ApiExceptionHandler(
    IProblemDetailsService problemDetails,
    ILogger<ApiExceptionHandler> logger
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        var problem = exception switch
        {
            InvoiceValidationException validation => Problem(
                StatusCodes.Status422UnprocessableEntity,
                "draft.invalid",
                "The invoice draft cannot be finalized.",
                validation.Message,
                [
                    .. validation.Issues.Select(issue => new ValidationIssueResponse(
                        issue.Code,
                        issue.Message
                    )),
                ]
            ),
            BusinessRuleViolationException { Code: "issued-invoice.integrity-violation" } rule =>
                IntegrityFailure(rule),
            BusinessRuleViolationException { Code: "draft.not-open" } rule => Problem(
                StatusCodes.Status409Conflict,
                rule.Code,
                "The draft has already been issued.",
                rule.Message
            ),
            BusinessRuleViolationException rule => Problem(
                StatusCodes.Status422UnprocessableEntity,
                rule.Code,
                "A business rule was violated.",
                rule.Message
            ),
            ConcurrencyConflictException conflict => Problem(
                StatusCodes.Status409Conflict,
                "concurrency.conflict",
                "The records changed while processing the request.",
                conflict.Message
            ),
            _ => null,
        };

        if (problem is null)
        {
            return false;
        }

        httpContext.Response.StatusCode =
            problem.Status ?? StatusCodes.Status500InternalServerError;
        return await problemDetails
            .TryWriteAsync(
                new ProblemDetailsContext
                {
                    HttpContext = httpContext,
                    ProblemDetails = problem,
                    Exception = exception,
                }
            )
            .ConfigureAwait(false);
    }

    private ProblemDetails IntegrityFailure(BusinessRuleViolationException rule)
    {
        LogIntegrityFailure(logger, rule);
        return Problem(
            StatusCodes.Status500InternalServerError,
            rule.Code,
            "Stored data failed its integrity check.",
            "The record does not match its content hash and was not served. Investigate before continuing."
        );
    }

    [LoggerMessage(
        Level = LogLevel.Critical,
        Message = "Integrity check failed for a stored record"
    )]
    private static partial void LogIntegrityFailure(ILogger logger, Exception exception);

    private static ProblemDetails Problem(
        int status,
        string code,
        string title,
        string detail,
        IReadOnlyList<ValidationIssueResponse>? issues = null
    )
    {
        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail,
            Type = "urn:raccord:problem:" + code,
        };
        problem.Extensions["code"] = code;
        if (issues is not null)
        {
            problem.Extensions["issues"] = issues;
        }

        return problem;
    }
}
