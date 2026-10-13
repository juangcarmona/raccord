#pragma warning disable MA0048, MA0006

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Raccord.Infrastructure.Persistence;

namespace Raccord.Web.Authorization;

internal sealed record PermissionRequirement(string Permission) : IAuthorizationRequirement;

internal sealed class PermissionAuthorizationHandler(
    UserManager<RaccordUser> users,
    RoleManager<IdentityRole<Guid>> roles
) : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement
    )
    {
        if (context.User.HasClaim(PermissionCatalog.ClaimType, PermissionCatalog.All))
        {
            context.Succeed(requirement);
            return;
        }

        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var id))
        {
            return;
        }

        var user = await users.FindByIdAsync(id.ToString()).ConfigureAwait(false);
        if (user is null)
        {
            return;
        }

        var userRoles = await users.GetRolesAsync(user).ConfigureAwait(false);
        foreach (var roleName in userRoles)
        {
            var role = await roles.FindByNameAsync(roleName).ConfigureAwait(false);
            if (role is null)
            {
                continue;
            }

            var claims = await roles.GetClaimsAsync(role).ConfigureAwait(false);
            if (
                claims.Any(claim =>
                    claim.Type == PermissionCatalog.ClaimType
                    && claim.Value == requirement.Permission
                )
            )
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}

internal static class PermissionEndpointExtensions
{
    public static TBuilder RequirePermission<TBuilder>(this TBuilder builder, string permission)
        where TBuilder : IEndpointConventionBuilder
    {
        builder.RequireAuthorization(policy =>
            policy.Requirements.Add(new PermissionRequirement(permission))
        );
        return builder;
    }
}

#pragma warning restore MA0048, MA0006
