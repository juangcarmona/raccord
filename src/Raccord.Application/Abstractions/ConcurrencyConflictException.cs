namespace Raccord.Application.Abstractions;

/// <summary>Raised when a concurrent operation changed the same records; the caller may retry.</summary>
public sealed class ConcurrencyConflictException : Exception
{
    /// <summary>Creates the exception.</summary>
    public ConcurrencyConflictException(string message, Exception innerException)
        : base(message, innerException) { }
}
