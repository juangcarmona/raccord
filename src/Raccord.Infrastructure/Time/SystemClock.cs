using Raccord.Application.Abstractions;

namespace Raccord.Infrastructure.Time;

/// <summary>System time, with "today" evaluated in the organization's time zone.</summary>
internal sealed class SystemClock(TimeProvider timeProvider, TimeZoneInfo organizationTimeZone) : IClock
{
    public DateTimeOffset UtcNow => timeProvider.GetUtcNow();

    public DateOnly Today => DateOnly.FromDateTime(TimeZoneInfo.ConvertTime(UtcNow, organizationTimeZone).DateTime);
}
