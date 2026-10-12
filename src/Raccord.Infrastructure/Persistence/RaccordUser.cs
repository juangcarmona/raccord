using Microsoft.AspNetCore.Identity;

namespace Raccord.Infrastructure.Persistence;

/// <summary>A local user account stored by the Workspace module.</summary>
public sealed class RaccordUser : IdentityUser<Guid>
{
    /// <summary>Creates a user with a sortable identifier.</summary>
    public RaccordUser() => Id = Guid.CreateVersion7();
}
