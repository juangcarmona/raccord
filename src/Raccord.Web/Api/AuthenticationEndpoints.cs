using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Raccord.Infrastructure.Persistence;
using Raccord.Web.Authorization;

namespace Raccord.Web.Api;

internal static class AuthenticationEndpoints
{
    public static IEndpointRouteBuilder MapRaccordAuthentication(this IEndpointRouteBuilder app)
    {
        var auth = app.MapGroup("/api/auth").WithTags("Authentication");
        auth.MapPost("/sign-in", SignInAsync).AllowAnonymous();
        auth.MapPost("/sign-out", SignOutAsync);
        auth.MapGet("/me", GetCurrentUserAsync);
        auth.MapPut("/me", UpdateCurrentUserAsync);
        return app;
    }

    private static async Task<Results<Ok<CurrentUserResponse>, UnauthorizedHttpResult>> SignInAsync(
        SignInRequest request,
        SignInManager<RaccordUser> signIn,
        UserManager<RaccordUser> users,
        RoleManager<IdentityRole<Guid>> roles,
        CancellationToken cancellationToken
    )
    {
        var result = await signIn.PasswordSignInAsync(
            request.Email,
            request.Password,
            request.RememberMe,
            lockoutOnFailure: true
        );
        if (!result.Succeeded)
        {
            return TypedResults.Unauthorized();
        }

        var user = await users.FindByEmailAsync(request.Email).ConfigureAwait(false);
        if (user is null)
        {
            return TypedResults.Unauthorized();
        }

        return TypedResults.Ok(await ToResponseAsync(user, users, roles).ConfigureAwait(false));
    }

    private static async Task<Ok> SignOutAsync(SignInManager<RaccordUser> signIn)
    {
        await signIn.SignOutAsync();
        return TypedResults.Ok();
    }

    private static async Task<
        Results<Ok<CurrentUserResponse>, UnauthorizedHttpResult>
    > GetCurrentUserAsync(
        HttpContext context,
        UserManager<RaccordUser> users,
        RoleManager<IdentityRole<Guid>> roles
    )
    {
        var user = await users.GetUserAsync(context.User).ConfigureAwait(false);
        return user is null
            ? TypedResults.Unauthorized()
            : TypedResults.Ok(await ToResponseAsync(user, users, roles).ConfigureAwait(false));
    }

    private static async Task<
        Results<Ok<CurrentUserResponse>, BadRequest<string>, UnauthorizedHttpResult>
    > UpdateCurrentUserAsync(
        UpdateProfileRequest request,
        HttpContext context,
        UserManager<RaccordUser> users,
        RoleManager<IdentityRole<Guid>> roles
    )
    {
        var displayName = request.DisplayName?.Trim();
        var language = request.PreferredLanguage?.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(displayName) || displayName.Length > 100)
        {
            return TypedResults.BadRequest(
                "A display name between 1 and 100 characters is required."
            );
        }

        if (language is not ("es" or "en"))
        {
            return TypedResults.BadRequest("Preferred language must be 'es' or 'en'.");
        }

        var user = await users.GetUserAsync(context.User).ConfigureAwait(false);
        if (user is null)
        {
            return TypedResults.Unauthorized();
        }

        user.DisplayName = displayName;
        user.PreferredLanguage = language;
        var result = await users.UpdateAsync(user).ConfigureAwait(false);
        if (!result.Succeeded)
        {
            return TypedResults.BadRequest(
                string.Join(" ", result.Errors.Select(error => error.Description))
            );
        }

        return TypedResults.Ok(await ToResponseAsync(user, users, roles).ConfigureAwait(false));
    }

    private static async Task<CurrentUserResponse> ToResponseAsync(
        RaccordUser user,
        UserManager<RaccordUser> users,
        RoleManager<IdentityRole<Guid>> roles
    ) =>
        new(
            user.Id.ToString(),
            user.Email ?? string.Empty,
            user.DisplayName,
            user.PreferredLanguage,
            await CanManageRolesAsync(user, users, roles).ConfigureAwait(false)
        );

    private static async Task<bool> CanManageRolesAsync(
        RaccordUser user,
        UserManager<RaccordUser> users,
        RoleManager<IdentityRole<Guid>> roles
    )
    {
        foreach (var roleName in await users.GetRolesAsync(user).ConfigureAwait(false))
        {
            var role = await roles.FindByNameAsync(roleName).ConfigureAwait(false);
            if (role is null)
            {
                continue;
            }

            var claims = await roles.GetClaimsAsync(role).ConfigureAwait(false);
            if (
                claims.Any(claim =>
                    string.Equals(claim.Type, PermissionCatalog.ClaimType, StringComparison.Ordinal)
                    && (
                        string.Equals(claim.Value, PermissionCatalog.All, StringComparison.Ordinal)
                        || string.Equals(
                            claim.Value,
                            PermissionCatalog.RolesRead,
                            StringComparison.Ordinal
                        )
                        || string.Equals(
                            claim.Value,
                            PermissionCatalog.RolesEdit,
                            StringComparison.Ordinal
                        )
                    )
                )
            )
            {
                return true;
            }
        }

        return false;
    }

    private sealed record SignInRequest(string Email, string Password, bool RememberMe = false);

    private sealed record UpdateProfileRequest(string? DisplayName, string? PreferredLanguage);

    private sealed record CurrentUserResponse(
        string Id,
        string Email,
        string? DisplayName,
        string PreferredLanguage,
        bool CanManageRoles
    );
}
