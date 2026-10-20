namespace Raccord.Domain.Projects;

/// <summary>A named interval on a project's time schedule.</summary>
public sealed class ProjectTimePeriod
{
    private ProjectTimePeriod()
    {
        Name = string.Empty;
    }

    internal ProjectTimePeriod(
        string name,
        int position,
        bool isDefaultPlanning,
        bool isDefaultUsage
    )
    {
        Id = Guid.CreateVersion7();
        Name = name;
        Position = position;
        IsDefaultPlanning = isDefaultPlanning;
        IsDefaultUsage = isDefaultUsage;
    }

    /// <summary>Period identifier.</summary>
    public Guid Id { get; private set; }

    /// <summary>Schedule entry name.</summary>
    public string Name { get; private set; }

    /// <summary>Manual display order.</summary>
    public int Position { get; private set; }

    /// <summary>Whether the period is the project's default planning period.</summary>
    public bool IsDefaultPlanning { get; private set; }

    /// <summary>Whether the period is the project's default usage period.</summary>
    public bool IsDefaultUsage { get; private set; }

    /// <summary>Period start, unset until timed.</summary>
    public DateTimeOffset? Start { get; private set; }

    /// <summary>Period end, unset until timed.</summary>
    public DateTimeOffset? End { get; private set; }

    internal void SetRange(DateTimeOffset? start, DateTimeOffset? end)
    {
        Start = start;
        End = end;
    }
}
