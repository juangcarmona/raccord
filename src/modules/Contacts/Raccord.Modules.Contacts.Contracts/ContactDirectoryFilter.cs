using Raccord.SharedKernel.Parties;

namespace Raccord.Modules.Contacts.Contracts;

/// <summary>Optional server-side predicates for the Contacts directory page.</summary>
public sealed record ContactDirectoryFilter(
    int Page,
    int PageSize,
    ContactRole? Role,
    Guid? FolderId,
    IReadOnlyList<Guid> TagIds,
    bool RequireAllTags
);
