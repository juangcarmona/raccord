using ErrorOr;
using Raccord.BuildingBlocks.Api;

namespace Raccord.BuildingBlocks.Messaging;

/// <summary>An expected exception mapping with optional safe problem issues.</summary>
public sealed record ExpectedFailure(Error Error, IReadOnlyList<ProblemIssue>? Issues = null);
