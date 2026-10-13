using Microsoft.AspNetCore.Authentication;
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
        auth.MapGet("/external", ChallengeExternal).AllowAnonymous();
        auth.MapGet("/external/callback", CompleteExternalAsync).AllowAnonymous();
        auth.MapGet(
            "/me",
            (HttpContext context) =>
                TypedResults.Ok(
                    new CurrentUserResponse(context.User.Identity?.Name ?? string.Empty)
                )
        );
        return app;
    }

    private static async Task<Results<Ok<CurrentUserResponse>, UnauthorizedHttpResult>> SignInAsync(
        SignInRequest request,
        SignInManager<RaccordUser> signIn,
        RoleManager<IdentityRole<Guid>> roles,
        UserManager<RaccordUser> users,
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
        if (
            user is not null
            && await RequiresExternalSignInAsync(user, users, roles).ConfigureAwait(false)
        )
        {
            await signIn.SignOutAsync().ConfigureAwait(false);
            return TypedResults.Unauthorized();
        }

        return TypedResults.Ok(new CurrentUserResponse(request.Email));
    }

    private static async Task<bool> RequiresExternalSignInAsync(
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

            if (
                (await roles.GetClaimsAsync(role).ConfigureAwait(false)).Any(claim =>
                    string.Equals(
                        claim.Type,
                        PermissionCatalog.SsoRequiredClaimType,
                        StringComparison.Ordinal
                    )
                    && string.Equals(
                        claim.Value,
                        bool.TrueString,
                        StringComparison.OrdinalIgnoreCase
                    )
                )
            )
            {
                return true;
            }
        }

        return false;
    }

    private static async Task<Ok> SignOutAsync(SignInManager<RaccordUser> signIn)
    {
        await signIn.SignOutAsync();
        return TypedResults.Ok();
    }

    private static IResult ChallengeExternal(string returnUrl, HttpContext context)
    {
        if (
            !context
                .RequestServices.GetRequiredService<IConfiguration>()
                .GetValue<bool>("Raccord:Authentication:Oidc:Enabled")
        )
        {
            return TypedResults.NotFound();
        }

        return Results.Challenge(
            new AuthenticationProperties
            {
                RedirectUri =
                    $"/api/auth/external/callback?returnUrl={Uri.EscapeDataString(returnUrl)}",
            },
            ["oidc"]
        );
    }

    private static async Task<IResult> CompleteExternalAsync(
        string? returnUrl,
        HttpContext context,
        SignInManager<RaccordUser> signIn,
        UserManager<RaccordUser> users
    )
    {
        var info = await signIn.GetExternalLoginInfoAsync().ConfigureAwait(false);
        if (info is null)
        {
            return TypedResults.Unauthorized();
        }

        var current =
            context.User.Identity?.IsAuthenticated == true
                ? await users.GetUserAsync(context.User).ConfigureAwait(false)
                : null;
        if (current is not null)
        {
            var linked = await users.AddLoginAsync(current, info).ConfigureAwait(false);
            return linked.Succeeded
                ? Results.Redirect(returnUrl ?? "/")
                : TypedResults.BadRequest(linked.Errors);
        }

        var result = await signIn
            .ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: false,
                bypassTwoFactor: true
            )
            .ConfigureAwait(false);
        return result.Succeeded ? Results.Redirect(returnUrl ?? "/") : TypedResults.Unauthorized();
    }

    private sealed record SignInRequest(string Email, string Password, bool RememberMe = false);

    private sealed record CurrentUserResponse(string Email);
}
