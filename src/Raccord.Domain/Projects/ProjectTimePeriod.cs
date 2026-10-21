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
        bool isDefaultUsage,
        bool showOnInternalDocuments = true,
        bool showOnExternalDocuments = false
    )
    {
        Id = Guid.CreateVersion7();
        Name = name;
        Position = position;
        IsDefaultPlanning = isDefaultPlanning;
        IsDefaultUsage = isDefaultUsage;
        ShowOnInternalDocuments = showOnInternalDocuments;
        ShowOnExternalDocuments = showOnExternalDocuments;
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

    /// <summary>Whether the period appears on internal project documents.</summary>
    public bool ShowOnInternalDocuments { get; private set; } = true;

    /// <summary>Whether the period appears on external project documents.</summary>
    public bool ShowOnExternalDocuments { get; private set; }

    /// <summary>Period start, unset until timed.</summary>
    public DateTimeOffset? Start { get; private set; }

    /// <summary>Period end, unset until timed.</summary>
    public DateTimeOffset? End { get; private set; }

    internal void SetRange(DateTimeOffset? start, DateTimeOffset? end)
    {
        Start = start;
        End = end;
    }

    internal void Update(
        string name,
        DateTimeOffset? start,
        DateTimeOffset? end,
        bool showOnInternalDocuments,
        bool showOnExternalDocuments
    )
    {
        Name = name;
        SetRange(start, end);
        ShowOnInternalDocuments = showOnInternalDocuments;
        ShowOnExternalDocuments = showOnExternalDocuments;
    }

    internal void SetPosition(int position) => Position = position;

    internal void SetDesignation(bool isDefaultPlanning, bool isDefaultUsage)
    {
        IsDefaultPlanning = isDefaultPlanning;
        IsDefaultUsage = isDefaultUsage;
    }

    internal ProjectTimePeriod Duplicate(int position) =>
        new(
            Name + " copy",
            position,
            isDefaultPlanning: false,
            isDefaultUsage: false,
            ShowOnInternalDocuments,
            ShowOnExternalDocuments
        )
        {
            Start = Start,
            End = End,
        };
}
