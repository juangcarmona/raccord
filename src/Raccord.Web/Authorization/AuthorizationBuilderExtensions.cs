using Microsoft.AspNetCore.Authorization;
using Raccord.Infrastructure.Persistence;

namespace Raccord.Web.Authorization;

internal static class AuthorizationBuilderExtensions
{
    public static AuthorizationBuilder AddPermissionPolicies(this AuthorizationBuilder builder)
    {
        foreach (var permission in PermissionCatalog.AllPermissions)
        {
            builder.AddPolicy(
                permission,
                policy => policy.AddRequirements(new PermissionRequirement(permission))
            );
        }

        return builder;
    }
}
