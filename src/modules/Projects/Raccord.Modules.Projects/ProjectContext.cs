using Raccord.Contracts.Projects;
using Raccord.Domain.Projects;
using Raccord.Modules.Compliance.Contracts;
using Raccord.Modules.Projects.Contracts;
using Raccord.SharedKernel.Auditing;

namespace Raccord.Modules.Projects;

#pragma warning disable CS1591

/// <summary>Coordinates notes, files, fields, tags, saved views and project history.</summary>
public sealed class ProjectContext(
    IBillableWork projects,
    IProjectContextStore context,
    IAuditTrail audit,
    IProjectsUnitOfWork unitOfWork
)
{
    public Task<ProjectContextEntryResponse?> AddNoteAsync(
        Guid projectId,
        AddProjectNoteRequest request,
        CancellationToken cancellationToken
    ) =>
        AddAsync(
            projectId,
            "note",
            request.Title,
            request.Body,
            null,
            null,
            null,
            null,
            null,
            request.Actor,
            cancellationToken
        );

    public Task<ProjectContextEntryResponse?> AddFileAsync(
        Guid projectId,
        AddProjectFileRequest request,
        CancellationToken cancellationToken
    ) =>
        AddAsync(
            projectId,
            "file",
            request.Name,
            request.Content,
            request.Folder,
            null,
            null,
            null,
            null,
            request.Actor,
            cancellationToken
        );

    public Task<ProjectContextEntryResponse?> AddFolderAsync(
        Guid projectId,
        AddProjectFolderRequest request,
        CancellationToken cancellationToken
    ) =>
        AddAsync(
            projectId,
            "folder",
            request.Name,
            null,
            request.ParentFolder,
            null,
            null,
            null,
            null,
            request.Actor,
            cancellationToken
        );

    public Task<ProjectContextEntryResponse?> DefineFieldAsync(
        Guid projectId,
        DefineProjectFieldRequest request,
        CancellationToken cancellationToken
    ) =>
        AddAsync(
            projectId,
            "field",
            request.Key,
            null,
            null,
            request.Key,
            request.Value,
            null,
            null,
            request.Actor,
            cancellationToken,
            request.Label
        );

    public Task<ProjectContextEntryResponse?> AddTagAsync(
        Guid projectId,
        TagProjectRecordRequest request,
        CancellationToken cancellationToken
    ) =>
        AddAsync(
            projectId,
            "tag",
            request.Tag,
            null,
            null,
            null,
            null,
            request.RecordType,
            request.RecordId,
            request.Actor,
            cancellationToken
        );

    public Task<ProjectContextEntryResponse?> SaveViewAsync(
        Guid projectId,
        SaveProjectViewRequest request,
        CancellationToken cancellationToken
    ) =>
        AddAsync(
            projectId,
            "view",
            request.Name,
            request.Definition,
            null,
            null,
            null,
            null,
            null,
            request.Actor,
            cancellationToken
        );

    public async Task<ProjectContextResponse?> GetAsync(
        Guid projectId,
        CancellationToken cancellationToken
    )
    {
        if (await projects.FindAsync(projectId, cancellationToken).ConfigureAwait(false) is null)
            return null;
        var entries = await context.ListAsync(projectId, cancellationToken).ConfigureAwait(false);
        var history = await audit
            .ListForEntityAsync(projectId, cancellationToken)
            .ConfigureAwait(false);
        return new(
            projectId,
            entries.Select(ToResponse).ToList(),
            history
                .Select(item => new ProjectHistoryEntryResponse(
                    item.Id,
                    item.OccurredAt,
                    item.Actor,
                    item.Action,
                    item.EntityType,
                    item.EntityId
                ))
                .ToList()
        );
    }

    private async Task<ProjectContextEntryResponse?> AddAsync(
        Guid projectId,
        string kind,
        string name,
        string? content,
        string? folder,
        string? key,
        string? value,
        string? targetType,
        Guid? targetId,
        string actor,
        CancellationToken cancellationToken,
        string? label = null
    )
    {
        if (await projects.FindAsync(projectId, cancellationToken).ConfigureAwait(false) is null)
            return null;
        var entry = ProjectContextEntry.Create(
            projectId,
            kind,
            label ?? name,
            content,
            folder,
            key,
            value,
            targetType,
            targetId,
            actor
        );
        context.Add(entry);
        audit.Append(
            new AuditEntry(
                entry.CreatedAt,
                actor,
                $"project.context.{kind}.added",
                "project",
                projectId,
                new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    ["entryId"] = entry.Id.ToString(),
                    ["name"] = entry.Name,
                }
            )
        );
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(entry);
    }

    private static ProjectContextEntryResponse ToResponse(ProjectContextEntry entry) =>
        new(
            entry.Id,
            entry.Kind,
            entry.Name,
            entry.Content,
            entry.Folder,
            entry.Key,
            entry.Value,
            entry.TargetType,
            entry.TargetId,
            entry.Actor,
            entry.CreatedAt
        );
}

#pragma warning restore CS1591
