using ErrorOr;
using Raccord.Application.Abstractions;
using Raccord.Application.Invoicing;
using Raccord.BuildingBlocks.Api;
using Raccord.BuildingBlocks.Messaging;

namespace Raccord.Infrastructure.Messaging;

/// <summary>Adapts current application-owned exceptions without reversing the shared dependency direction.</summary>
internal sealed class ApplicationExceptionMapper : IExpectedExceptionMapper
{
    public ExpectedFailure? TryMap(Exception exception) =>
        exception switch
        {
            InvoiceValidationException validation => new ExpectedFailure(
                Error.Validation(
                    "draft.invalid",
                    validation.Message,
                    new Dictionary<string, object>(StringComparer.Ordinal)
                    {
                        ["raccord.issues"] = validation
                            .Issues.Select(issue => new ProblemIssue(issue.Code, issue.Message))
                            .ToArray(),
                        ["raccord.title"] = "The invoice draft cannot be finalized.",
                    }
                )
            ),
            ConcurrencyConflictException conflict => new ExpectedFailure(
                Error.Conflict("concurrency.conflict", conflict.Message)
            ),
            _ => null,
        };
}
