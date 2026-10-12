namespace Raccord.BuildingBlocks.Persistence;

/// <summary>Persistence conflict that is safe for an application adapter to translate.</summary>
public sealed class PersistenceConflictException : Exception
{
    /// <summary>Creates a persistence conflict with its database cause.</summary>
    public PersistenceConflictException(string message, Exception innerException)
        : base(message, innerException) { }
}
