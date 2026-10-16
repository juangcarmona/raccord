#pragma warning disable CS1591

namespace Raccord.Contracts.Integrations;

public sealed record IssueCalendarFeedRequest(string Stream);

public sealed record CalendarFeedResponse(Guid Id, string Stream, string Url);

#pragma warning restore CS1591
