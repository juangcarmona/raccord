using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Raccord.Infrastructure.Persistence;

/// <summary>Creates the first administrator and predefined installation roles atomically.</summary>
public sealed class InstallationSetup(
    RaccordDbContext db,
    UserManager<RaccordUser> users,
    RoleManager<IdentityRole<Guid>> roles,
    VatCatalogInitializer vatCatalog
)
{
    /// <summary>The roles available to every new installation.</summary>
    public static readonly string[] PredefinedRoles = ["Admin", "Office", "Warehouse", "Crew"];

    /// <summary>Returns whether the installation has no users and still needs setup.</summary>
    public async Task<bool> IsRequiredAsync(CancellationToken cancellationToken) =>
        !await db.Users.AnyAsync(cancellationToken).ConfigureAwait(false);

    /// <summary>Creates the first user, assigns Admin, and seeds all predefined roles.</summary>
    public async Task<SetupResult> CreateFirstAdministratorAsync(
        string email,
        string password,
        CancellationToken cancellationToken
    )
    {
        var strategy = db.Database.CreateExecutionStrategy();
        return await strategy
            .ExecuteAsync(() => CreateWithinTransactionAsync(email, password, cancellationToken))
            .ConfigureAwait(false);
    }

    private async Task<SetupResult> CreateWithinTransactionAsync(
        string email,
        string password,
        CancellationToken cancellationToken
    )
    {
        using var transaction = await db
            .Database.BeginTransactionAsync(cancellationToken)
            .ConfigureAwait(false);
        if (await db.Users.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            return SetupResult.AlreadyCompleted;
        }

        foreach (var roleName in PredefinedRoles)
        {
            if (!await roles.RoleExistsAsync(roleName).ConfigureAwait(false))
            {
                var roleResult = await roles
                    .CreateAsync(new IdentityRole<Guid>(roleName) { Id = Guid.CreateVersion7() })
                    .ConfigureAwait(false);
                if (!roleResult.Succeeded)
                {
                    return SetupResult.Failed(roleResult.Errors.Select(error => error.Description));
                }
            }

            await EnsureRolePermissionsAsync(roleName).ConfigureAwait(false);
        }

        var user = new RaccordUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
        };
        var userResult = await users.CreateAsync(user, password).ConfigureAwait(false);
        if (!userResult.Succeeded)
        {
            return SetupResult.Failed(userResult.Errors.Select(error => error.Description));
        }

        var assignment = await users.AddToRoleAsync(user, "Admin").ConfigureAwait(false);
        if (!assignment.Succeeded)
        {
            return SetupResult.Failed(assignment.Errors.Select(error => error.Description));
        }

        await vatCatalog.EnsureInitializedAsync(email, cancellationToken).ConfigureAwait(false);

        await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
        return SetupResult.Created;
    }

    private async Task EnsureRolePermissionsAsync(string roleName)
    {
        var role = await roles.FindByNameAsync(roleName).ConfigureAwait(false);
        if (role is null)
        {
            return;
        }

        var existingClaims = await roles.GetClaimsAsync(role).ConfigureAwait(false);
        var permissions = PermissionCatalog.PredefinedRoles[roleName];
        foreach (
            var permission in permissions.Where(permission =>
                existingClaims.All(claim =>
                    !string.Equals(claim.Value, permission, StringComparison.Ordinal)
                )
            )
        )
        {
            await roles
                .AddClaimAsync(role, new Claim(PermissionCatalog.ClaimType, permission))
                .ConfigureAwait(false);
        }
    }

    /// <summary>The outcome of a first-administrator attempt.</summary>
    public enum SetupResultKind
    {
        /// <summary>The administrator was created.</summary>
        Created,

        /// <summary>A user already existed.</summary>
        AlreadyCompleted,

        /// <summary>Identity validation rejected the request.</summary>
        Failed,
    }

    /// <summary>Result details returned by the setup operation.</summary>
    public sealed record SetupResult(SetupResultKind Kind, IReadOnlyList<string> Errors)
    {
        /// <summary>A successful setup result.</summary>
        public static SetupResult Created { get; } = new(SetupResultKind.Created, []);

        /// <summary>A result indicating setup was already completed.</summary>
        public static SetupResult AlreadyCompleted { get; } =
            new(SetupResultKind.AlreadyCompleted, []);

        /// <summary>Creates a failed setup result with validation errors.</summary>
        public static SetupResult Failed(IEnumerable<string> errors) =>
            new(SetupResultKind.Failed, errors.ToArray());
    }
}
