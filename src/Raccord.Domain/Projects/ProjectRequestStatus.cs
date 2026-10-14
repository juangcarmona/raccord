namespace Raccord.Domain.Projects;

/// <summary>Lifecycle of incoming work before it becomes a project.</summary>
public enum ProjectRequestStatus
{
    /// <summary>The request is awaiting a decision.</summary>
    Received,

    /// <summary>The request produced a project.</summary>
    Converted,

    /// <summary>The request was declined.</summary>
    Declined,
}
