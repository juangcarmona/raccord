namespace Raccord.Modules.Contacts.Contracts;

/// <summary>A stable page of contact directory results.</summary>
public sealed record ContactPage(
    IReadOnlyList<ContactSnapshot> Items,
    int Page,
    int PageSize,
    int TotalCount
)
{
    /// <summary>Whether another page follows this one.</summary>
    public bool HasNextPage => (long)Page * PageSize < TotalCount;

    /// <summary>Whether a previous page exists.</summary>
    public bool HasPreviousPage => Page > 1;
}
