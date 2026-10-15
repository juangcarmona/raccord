using Raccord.SharedKernel.Common;

namespace Raccord.Domain.Projects;

#pragma warning disable CS1591

/// <summary>A project-scoped record kept with the project's working context.</summary>
public sealed class ProjectContextEntry
{
    private ProjectContextEntry() { }

    private ProjectContextEntry(
        Guid projectId,
        string kind,
        string name,
        string? content,
        string? folder,
        string? key,
        string? value,
        string? targetType,
        Guid? targetId,
        string actor
    )
    {
        Id = Guid.CreateVersion7();
        ProjectId = projectId;
        Kind = kind;
        Name = name;
        Content = content;
        Folder = folder;
        Key = key;
        Value = value;
        TargetType = targetType;
        TargetId = targetId;
        Actor = actor;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid ProjectId { get; private set; }
    public string Kind { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Content { get; private set; }
    public string? Folder { get; private set; }
    public string? Key { get; private set; }
    public string? Value { get; private set; }
    public string? TargetType { get; private set; }
    public Guid? TargetId { get; private set; }
    public string Actor { get; private set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; private set; }

    public static ProjectContextEntry Create(
        Guid projectId,
        string kind,
        string name,
        string? content,
        string? folder,
        string? key,
        string? value,
        string? targetType,
        Guid? targetId,
        string actor
    )
    {
        if (projectId == Guid.Empty)
            throw new BusinessRuleViolationException(
                "projects.context-project-required",
                "A project is required."
            );
        if (string.IsNullOrWhiteSpace(kind))
            throw new BusinessRuleViolationException(
                "projects.context-kind-required",
                "A context record kind is required."
            );
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessRuleViolationException(
                "projects.context-name-required",
                "A context record name is required."
            );
        if (string.IsNullOrWhiteSpace(actor))
            throw new BusinessRuleViolationException(
                "projects.context-actor-required",
                "An actor is required."
            );
        return new(projectId, kind, name, content, folder, key, value, targetType, targetId, actor);
    }
}

#pragma warning restore CS1591
