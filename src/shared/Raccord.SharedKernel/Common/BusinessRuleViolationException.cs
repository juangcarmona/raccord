namespace Raccord.SharedKernel.Common;

/// <summary>Raised when a command would break a business invariant.</summary>
public sealed class BusinessRuleViolationException : Exception
{
    /// <summary>Creates the exception with a stable, machine-readable rule code.</summary>
    public BusinessRuleViolationException(string code, string message)
        : base(message)
    {
        Code = code;
    }

    /// <summary>A stable rule identifier, such as <c>draft.no-lines</c>.</summary>
    public string Code { get; }
}
