namespace Raccord.Infrastructure.Compliance;

internal sealed record ComplianceAuthorityResponse(bool Accepted, string? Reference, string? Error);
