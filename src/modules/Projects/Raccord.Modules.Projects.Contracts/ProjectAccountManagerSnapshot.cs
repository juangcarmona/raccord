namespace Raccord.Modules.Projects.Contracts;

/// <summary>A user allowed to manage projects.</summary>
public sealed record ProjectAccountManagerSnapshot(Guid Id, string DisplayName);
