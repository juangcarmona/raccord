using Microsoft.AspNetCore.Identity;

namespace Raccord.Infrastructure.Persistence;

/// <summary>A local user account stored by the Workspace module.</summary>
public sealed class RaccordUser : IdentityUser<Guid>
{
    /// <summary>Creates a user with a sortable identifier.</summary>
    public RaccordUser() => Id = Guid.CreateVersion7();

    /// <summary>The name shown in the signed-in application.</summary>
    public string? DisplayName { get; set; }

    /// <summary>The user's personal interface language.</summary>
    public string PreferredLanguage { get; set; } = "es";
}
