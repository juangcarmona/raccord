namespace Raccord.Domain.Projects;

/// <summary>Explains why equipment is present on a packing list.</summary>
public enum PackingLineRole
{
    /// <summary>Equipment included in the original project plan.</summary>
    Required,

    /// <summary>Equipment added during warehouse preparation.</summary>
    Additional,

    /// <summary>Equipment included for temporary use.</summary>
    Temporary,

    /// <summary>Equipment planned but not mandatory for the project.</summary>
    Optional,
}
