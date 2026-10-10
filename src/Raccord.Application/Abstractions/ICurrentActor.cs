namespace Raccord.Application.Abstractions;

/// <summary>Who is performing the current operation, for audit purposes.</summary>
public interface ICurrentActor
{
    /// <summary>Stable actor name recorded in audit events.</summary>
    string Name { get; }
}
