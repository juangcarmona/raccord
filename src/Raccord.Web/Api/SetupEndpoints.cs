using Microsoft.AspNetCore.Http.HttpResults;
using Raccord.Infrastructure.Persistence;

namespace Raccord.Web.Api;

internal static class SetupEndpoints
{
    public static IEndpointRouteBuilder MapRaccordSetup(this IEndpointRouteBuilder app)
    {
        var setup = app.MapGroup("/api/setup").WithTags("Installation setup").AllowAnonymous();
        setup.MapGet("/status", GetStatusAsync);
        setup.MapPost("/", CreateAdministratorAsync);
        return app;
    }

    private static async Task<Ok<SetupStatusResponse>> GetStatusAsync(
        InstallationSetup installation,
        CancellationToken cancellationToken
    ) =>
        TypedResults.Ok(
            new SetupStatusResponse(await installation.IsRequiredAsync(cancellationToken))
        );

    private static async Task<
        Results<Ok<SetupResponse>, BadRequest<SetupError>, Conflict<SetupError>>
    > CreateAdministratorAsync(
        SetupRequest request,
        InstallationSetup installation,
        RegistrationPasswordCheck passwordCheck,
        CancellationToken cancellationToken
    )
    {
        if (
            string.IsNullOrWhiteSpace(request.Email)
            || string.IsNullOrEmpty(request.Password)
            || request.Password.Length > 1024
        )
            return TypedResults.BadRequest(
                new SetupError("setup-invalid", "Enter an email and a valid password.")
            );
        if (!await installation.IsRequiredAsync(cancellationToken))
            return TypedResults.Conflict(
                new SetupError("setup-completed", "The installation has already been set up.")
            );
        var check = await passwordCheck.CheckAsync(request.Password, cancellationToken);
        var result = await installation.CreateFirstAdministratorAsync(
            request.Email,
            request.Password,
            cancellationToken
        );
        return result.Kind switch
        {
            InstallationSetup.SetupResultKind.AlreadyCompleted => TypedResults.Conflict(
                new SetupError("setup-completed", "The installation has already been set up.")
            ),
            InstallationSetup.SetupResultKind.Failed => TypedResults.BadRequest(
                new SetupError("setup-invalid", string.Join(" ", result.Errors))
            ),
            _ => TypedResults.Ok(new SetupResponse(request.Email, check)),
        };
    }

    private sealed record SetupRequest(string Email, string Password);

    private sealed record SetupStatusResponse(bool Required);

    private sealed record SetupResponse(string Email, string PasswordCheck);

    private sealed record SetupError(string Code, string Message);
}
