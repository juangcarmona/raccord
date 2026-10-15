namespace Raccord.Contracts.Crew;

#pragma warning disable CS1591

public enum CrewRateKind
{
    Price,
    Cost,
}

public sealed record CrewRateBand(decimal FromHour, decimal FixedAmount, decimal HourlyAmount);

public sealed record CreateCrewMemberRequest(
    string FirstName,
    string Role,
    string Email,
    string? LastName,
    string? Folder,
    IReadOnlyList<string>? Skills,
    bool IsPlannable = true
);

public sealed record UpdateCrewMemberRequest(
    string FirstName,
    string Role,
    string Email,
    string? LastName,
    string? Folder,
    IReadOnlyList<string>? Skills,
    bool IsPlannable
);

public sealed record CrewMemberResponse(
    Guid Id,
    string FirstName,
    string Role,
    string Email,
    string? LastName,
    string? Folder,
    IReadOnlyList<string> Skills,
    bool IsPlannable
);

public sealed record CreateVehicleRequest(
    string RegistrationNumber,
    int Seats,
    string? Folder,
    IReadOnlyList<string>? Tags,
    bool IsShownInPlanning = true
);

public sealed record UpdateVehicleRequest(
    string RegistrationNumber,
    int Seats,
    string? Folder,
    IReadOnlyList<string>? Tags,
    bool IsShownInPlanning
);

public sealed record VehicleResponse(
    Guid Id,
    string RegistrationNumber,
    int Seats,
    string? Folder,
    IReadOnlyList<string> Tags,
    bool IsShownInPlanning
);

public sealed record CreateCrewRateRequest(
    string Name,
    CrewRateKind Kind,
    IReadOnlyList<CrewRateBand> Bands,
    bool IsDefault = false
);

public sealed record CrewRateResponse(
    Guid Id,
    string Name,
    CrewRateKind Kind,
    IReadOnlyList<CrewRateBand> Bands,
    bool IsDefault
);

#pragma warning restore CS1591
