#pragma warning disable MA0048, MA0002, MA0006

using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Raccord.Infrastructure.Persistence;
using Raccord.Web.Authorization;

namespace Raccord.Web.Api;

internal static class RoleEndpoints
{
    public static IEndpointRouteBuilder MapRaccordRoles(this IEndpointRouteBuilder app)
    {
        var roles = app.MapGroup("/api/roles").WithTags("Roles").RequireAuthorization();
        roles.MapGet("/", ListAsync).RequirePermission(PermissionCatalog.RolesRead);
        roles.MapPost("/", CreateAsync).RequirePermission(PermissionCatalog.RolesEdit);
        roles.MapPut("/{name}", UpdateAsync).RequirePermission(PermissionCatalog.RolesEdit);
        roles
            .MapPut("/users/{userId:guid}", AssignAsync)
            .RequirePermission(PermissionCatalog.RolesAssign);
        return app;
    }

    private static async Task<Ok<IReadOnlyList<RoleResponse>>> ListAsync(
        RoleManager<IdentityRole<Guid>> roles,
        UserManager<RaccordUser> users
    )
    {
        var result = new List<RoleResponse>();
        foreach (var role in roles.Roles.OrderBy(role => role.Name))
        {
            var claims = await roles.GetClaimsAsync(role).ConfigureAwait(false);
            var count = (await users.GetUsersInRoleAsync(role.Name!).ConfigureAwait(false)).Count;
            result.Add(
                new RoleResponse(
                    role.Name!,
                    PermissionCatalog.PredefinedRoles.ContainsKey(role.Name!),
                    claims
                        .Where(claim => claim.Type == PermissionCatalog.ClaimType)
                        .Select(claim => claim.Value)
                        .Order()
                        .ToArray(),
                    count
                )
            );
        }

        return TypedResults.Ok<IReadOnlyList<RoleResponse>>(result);
    }

    private static async Task<Results<Created<RoleResponse>, BadRequest<string>>> CreateAsync(
        RoleRequest request,
        RoleManager<IdentityRole<Guid>> roles
    )
    {
        if (string.IsNullOrWhiteSpace(request.Name) || await roles.RoleExistsAsync(request.Name))
        {
            return TypedResults.BadRequest("A unique role name is required.");
        }

        var role = new IdentityRole<Guid>(request.Name.Trim()) { Id = Guid.CreateVersion7() };
        var result = await roles.CreateAsync(role).ConfigureAwait(false);
        if (!result.Succeeded)
        {
            return TypedResults.BadRequest(
                string.Join(" ", result.Errors.Select(error => error.Description))
            );
        }

        await ReplaceClaimsAsync(role, request.Permissions, roles).ConfigureAwait(false);
        return TypedResults.Created(
            $"/api/roles/{role.Name}",
            ToResponse(role, request.Permissions, 0)
        );
    }

    private static async Task<Results<Ok<RoleResponse>, NotFound, BadRequest<string>>> UpdateAsync(
        string name,
        RoleRequest request,
        RoleManager<IdentityRole<Guid>> roles,
        UserManager<RaccordUser> users
    )
    {
        var role = await roles.FindByNameAsync(name).ConfigureAwait(false);
        if (role is null)
        {
            return TypedResults.NotFound();
        }

        await ReplaceClaimsAsync(role, request.Permissions, roles).ConfigureAwait(false);
        var count = (await users.GetUsersInRoleAsync(role.Name!).ConfigureAwait(false)).Count;
        return TypedResults.Ok(ToResponse(role, request.Permissions, count));
    }

    private static async Task<Results<NoContent, NotFound, BadRequest<string>>> AssignAsync(
        Guid userId,
        AssignRoleRequest request,
        UserManager<RaccordUser> users,
        RoleManager<IdentityRole<Guid>> roles
    )
    {
        var user = await users.FindByIdAsync(userId.ToString()).ConfigureAwait(false);
        if (user is null || !await roles.RoleExistsAsync(request.Role).ConfigureAwait(false))
        {
            return TypedResults.NotFound();
        }

        var current = await users.GetRolesAsync(user).ConfigureAwait(false);
        var remove = await users.RemoveFromRolesAsync(user, current).ConfigureAwait(false);
        if (!remove.Succeeded)
        {
            return TypedResults.BadRequest(
                string.Join(" ", remove.Errors.Select(error => error.Description))
            );
        }

        var add = await users.AddToRoleAsync(user, request.Role).ConfigureAwait(false);
        if (!add.Succeeded)
        {
            return TypedResults.BadRequest(
                string.Join(" ", add.Errors.Select(error => error.Description))
            );
        }

        await users.UpdateSecurityStampAsync(user).ConfigureAwait(false);
        return TypedResults.NoContent();
    }

    private static async Task ReplaceClaimsAsync(
        IdentityRole<Guid> role,
        IReadOnlyCollection<string> permissions,
        RoleManager<IdentityRole<Guid>> roles
    )
    {
        var current = await roles.GetClaimsAsync(role).ConfigureAwait(false);
        foreach (var claim in current.Where(claim => claim.Type == PermissionCatalog.ClaimType))
        {
            await roles.RemoveClaimAsync(role, claim).ConfigureAwait(false);
        }

        foreach (var permission in permissions.Distinct(StringComparer.Ordinal))
        {
            await roles
                .AddClaimAsync(role, new Claim(PermissionCatalog.ClaimType, permission))
                .ConfigureAwait(false);
        }
    }

    private static RoleResponse ToResponse(
        IdentityRole<Guid> role,
        IEnumerable<string> permissions,
        int users
    ) =>
        new(
            role.Name!,
            PermissionCatalog.PredefinedRoles.ContainsKey(role.Name!),
            permissions.Order().ToArray(),
            users
        );

    private sealed record RoleRequest(string Name, IReadOnlyCollection<string> Permissions);

    private sealed record AssignRoleRequest(string Role);
}

internal sealed record RoleResponse(
    string Name,
    bool Predefined,
    IReadOnlyList<string> Permissions,
    int UserCount
);

#pragma warning restore MA0048, MA0002, MA0006
