namespace Raccord.Domain.Billing;

/// <summary>A reason why a draft cannot be issued yet.</summary>
/// <param name="Code">Stable rule code, for example <c>recipient.tax-id-missing</c>.</param>
/// <param name="Message">English explanation for logs and API clients; the UI localizes by code.</param>
public sealed record ValidationIssue(string Code, string Message);
