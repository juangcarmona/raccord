namespace Raccord.Domain.Projects;

/// <summary>Explicit outcome for equipment that does not follow the normal return path.</summary>
public enum PackingLineDisposition
{
    /// <summary>Equipment follows the standard receive flow.</summary>
    NormalReturn,

    /// <summary>Equipment moves directly to another project or destination.</summary>
    CrossDocked,

    /// <summary>Equipment is returned as a sale item for separate handling.</summary>
    SaleReturn,
}
