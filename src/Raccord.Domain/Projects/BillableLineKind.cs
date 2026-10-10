namespace Raccord.Domain.Projects;

/// <summary>What a billable line charges for.</summary>
public enum BillableLineKind
{
    /// <summary>Rented or supplied equipment.</summary>
    Equipment = 1,

    /// <summary>Crew time.</summary>
    Labour = 2,

    /// <summary>Vehicles, drivers and freight.</summary>
    Transport = 3,

    /// <summary>Material used up during the job.</summary>
    Consumable = 4,

    /// <summary>Services such as post-production or design.</summary>
    Service = 5,

    /// <summary>Anything else that is billed.</summary>
    Other = 6,
}
