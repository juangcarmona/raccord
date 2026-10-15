namespace Raccord.Contracts.Projects;

#pragma warning disable CS1591

public sealed record AddProjectNoteRequest(string Title, string Body, string Actor);

public sealed record AddProjectFileRequest(
    string Name,
    string? Folder,
    string Content,
    string Actor
);

public sealed record AddProjectFolderRequest(string Name, string? ParentFolder, string Actor);

public sealed record DefineProjectFieldRequest(
    string Key,
    string Label,
    string Value,
    string Actor
);

public sealed record TagProjectRecordRequest(
    string Tag,
    string? RecordType,
    Guid? RecordId,
    string Actor
);

public sealed record SaveProjectViewRequest(string Name, string Definition, string Actor);

public sealed record ProjectContextEntryResponse(
    Guid Id,
    string Kind,
    string Name,
    string? Content,
    string? Folder,
    string? Key,
    string? Value,
    string? TargetType,
    Guid? TargetId,
    string Actor,
    DateTimeOffset CreatedAt
);

public sealed record ProjectContextResponse(
    Guid ProjectId,
    IReadOnlyList<ProjectContextEntryResponse> Entries,
    IReadOnlyList<ProjectHistoryEntryResponse> History
);

public sealed record ProjectHistoryEntryResponse(
    Guid Id,
    DateTimeOffset OccurredAt,
    string Actor,
    string Action,
    string EntityType,
    Guid EntityId
);

#pragma warning restore CS1591
