namespace Raccord.Domain.Projects;

/// <summary>The fixed commercial lifecycle of a project.</summary>
public enum ProjectStatus
{
    /// <summary>A customer request that has not been planned.</summary>
    Inquiry,

    /// <summary>A newly created project being planned.</summary>
    Draft,

    /// <summary>A quotation is awaiting customer agreement.</summary>
    Pending,

    /// <summary>The customer has confirmed the project.</summary>
    Confirmed,

    /// <summary>Project execution has started.</summary>
    InProgress,

    /// <summary>All equipment has been returned.</summary>
    ExecutionDone,

    /// <summary>The team has closed the project.</summary>
    Complete,

    /// <summary>The project will not go ahead.</summary>
    Cancelled,
}
