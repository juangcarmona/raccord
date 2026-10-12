using ErrorOr;
using Raccord.SharedKernel.Common;

namespace Raccord.BuildingBlocks.Messaging;

internal sealed class SharedKernelExceptionMapper : IExpectedExceptionMapper
{
    public ExpectedFailure? TryMap(Exception exception) =>
        exception switch
        {
            BusinessRuleViolationException { Code: "draft.not-open", Message: var message } =>
                new ExpectedFailure(Error.Conflict("draft.not-open", message)),
            BusinessRuleViolationException { Code: "issued-invoice.integrity-violation" } => null,
            BusinessRuleViolationException rule => new ExpectedFailure(
                Error.Custom(1000, rule.Code, rule.Message)
            ),
            _ => null,
        };
}
