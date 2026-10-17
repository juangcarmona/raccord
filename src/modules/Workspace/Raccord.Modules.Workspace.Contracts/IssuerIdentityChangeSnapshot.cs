namespace Raccord.Modules.Workspace.Contracts;

/// <summary>Retained request and review history for issuer legal identity changes.</summary>
public sealed record IssuerIdentityChangeSnapshot(
    Guid Id,
    Guid IssuerId,
    string OldLegalName,
    string OldTaxId,
    string ProposedLegalName,
    string ProposedTaxId,
    string Reason,
    string RequestedById,
    string RequestedByName,
    DateTimeOffset RequestedAt,
    string Status,
    string? DecidedById,
    string? DecidedByName,
    DateTimeOffset? DecidedAt
);
