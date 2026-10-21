namespace Raccord.Modules.Projects.Contracts;

/// <summary>Counts the crew planning records affected by project cancellation.</summary>
public sealed record ProjectCancellationOperationsSummary(
    int PlannedCrewCount,
    int PlannedTransportCount,
    int ActiveInvitationCount
);
