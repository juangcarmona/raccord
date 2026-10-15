#pragma warning disable CS1591

namespace Raccord.Contracts.Crew;

public enum CrewAvailabilityKind
{
    Available,
    Unavailable,
}

public enum CrewInvitationStatus
{
    Pending,
    Accepted,
    Declined,
}

public sealed record SetCrewAvailabilityRequest(
    Guid CrewMemberId,
    DateOnly StartDate,
    DateOnly EndDate,
    CrewAvailabilityKind Kind,
    string? Remark = null
);

public sealed record InviteCrewRequest(Guid CrewMemberId, string? Remark = null);

public sealed record RespondToCrewInvitationRequest(CrewInvitationStatus Status);

public sealed record PostFunctionRequest(string? Brief = null);

public sealed record CrewAvailabilityResponse(
    Guid Id,
    Guid CrewMemberId,
    DateOnly StartDate,
    DateOnly EndDate,
    CrewAvailabilityKind Kind,
    string? Remark
);

public sealed record CrewInvitationResponse(
    Guid Id,
    Guid FunctionId,
    Guid CrewMemberId,
    CrewInvitationStatus Status,
    string? Remark
);

public sealed record CrewJobResponse(
    Guid Id,
    Guid FunctionId,
    string? Brief,
    DateTimeOffset PostedAt,
    int SignupCount
);

public sealed record CrewSignupResponse(
    Guid Id,
    Guid JobId,
    Guid CrewMemberId,
    DateTimeOffset SignedUpAt
);

#pragma warning restore CS1591
