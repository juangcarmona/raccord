using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Raccord.Infrastructure.Persistence;

namespace Raccord.Web.Api;

internal static class AuthenticationEndpoints
{
    public static IEndpointRouteBuilder MapRaccordAuthentication(this IEndpointRouteBuilder app)
    {
        var auth = app.MapGroup("/api/auth").WithTags("Authentication");
        auth.MapPost("/sign-in", SignInAsync).AllowAnonymous();
        auth.MapPost("/sign-out", SignOutAsync);
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

        return TypedResults.Ok(new CurrentUserResponse(request.Email));
    }

    private static async Task<Ok> SignOutAsync(SignInManager<RaccordUser> signIn)
    {
        await signIn.SignOutAsync();
        return TypedResults.Ok();
    }

    private sealed record SignInRequest(string Email, string Password, bool RememberMe = false);

    private sealed record CurrentUserResponse(string Email);
}
