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

    private static async Task<IResult> CreateAdministratorAsync(
        SetupRequest request,
        InstallationSetup installation,
        CancellationToken cancellationToken
    )
    {
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
            _ => TypedResults.Ok(new SetupResponse(request.Email)),
        };
    }

    private sealed record SetupRequest(string Email, string Password);

    private sealed record SetupStatusResponse(bool Required);

    private sealed record SetupResponse(string Email);

    private sealed record SetupError(string Code, string Message);
}
