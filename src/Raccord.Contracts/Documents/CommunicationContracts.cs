#pragma warning disable CS1591

namespace Raccord.Contracts.Documents;

public sealed record CreateCommunicationRequest(
    string Recipient,
    string Subject,
    string? DocumentKey = null
);

public sealed record RecordDeliveryOutcomeRequest(string Status, string? FailureReason = null);

public sealed record CommunicationSnapshot(
    Guid Id,
    string Recipient,
    string Subject,
    string? DocumentKey,
    string Status,
    string? FailureReason,
    DateTimeOffset CreatedAt,
    DateTimeOffset? DeliveredAt
);

#pragma warning restore CS1591
