namespace Raccord.BuildingBlocks.Api;

/// <summary>A safe, stable issue included in a problem response.</summary>
public sealed record ProblemIssue(string Code, string Message);
