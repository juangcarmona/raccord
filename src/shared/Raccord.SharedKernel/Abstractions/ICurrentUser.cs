namespace Raccord.SharedKernel.Abstractions;

/// <summary>Who is performing the current operation, for audit purposes.</summary>
public interface ICurrentUser
{
    /// <summary>Stable actor name recorded in audit events.</summary>
    string Name { get; }
}
