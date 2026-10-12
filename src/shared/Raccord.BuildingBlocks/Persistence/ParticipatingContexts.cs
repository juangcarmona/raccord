using Microsoft.EntityFrameworkCore;

namespace Raccord.BuildingBlocks.Persistence;

/// <summary>Ordered, identity-deduplicated EF contexts participating in one transaction.</summary>
public sealed class ParticipatingContexts
{
    private readonly List<DbContext> contexts = [];

    /// <summary>Registers a context once, preserving first registration order.</summary>
    public void Register(DbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        if (!contexts.Contains(context, ReferenceEqualityComparer.Instance))
        {
            contexts.Add(context);
        }
    }

    /// <summary>The current participants in registration order.</summary>
    public IReadOnlyList<DbContext> Items => contexts;
}
