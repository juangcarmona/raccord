namespace Raccord.SharedKernel.Abstractions;

/// <summary>The single trusted time source of the application.</summary>
public interface IClock
{
    /// <summary>Current instant in UTC.</summary>
    DateTimeOffset UtcNow { get; }

    /// <summary>Today's date in the organization's time zone.</summary>
    DateOnly Today { get; }
}
