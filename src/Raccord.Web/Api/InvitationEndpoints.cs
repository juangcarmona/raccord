using Microsoft.AspNetCore.Http.HttpResults;
using Raccord.Infrastructure.Persistence;
using Raccord.Web.Authorization;

namespace Raccord.Web.Api;

internal static class InvitationEndpoints
{
    public static IEndpointRouteBuilder MapRaccordInvitations(this IEndpointRouteBuilder app)
    {
        var invitations = app.MapGroup("/api/invitations").WithTags("Invitations");
        invitations.MapPost("/", CreateAsync).RequirePermission(PermissionCatalog.RolesAssign);
        invitations.MapPost("/{token}/accept", AcceptAsync).AllowAnonymous();
        return app;
    }

    private static async Task<Results<Ok<InvitationCreated>, BadRequest<string>>> CreateAsync(
        InvitationRequest request,
        InvitationService invitations,
        CancellationToken cancellationToken
    ) =>
        await invitations
            .CreateAsync(request.Email, request.Role, cancellationToken)
            .ConfigureAwait(false)
            is { } created
            ? TypedResults.Ok(created)
            : TypedResults.BadRequest("The selected role does not exist.");

    private static async Task<IResult> AcceptAsync(
        string token,
        JoinRequest request,
        InvitationService invitations,
        CancellationToken cancellationToken
    )
    {
        var result = await invitations
            .JoinAsync(token, request.Password, cancellationToken)
            .ConfigureAwait(false);
        return result.Kind switch
        {
            JoinResultKind.Joined => TypedResults.Ok(),
            JoinResultKind.ExistingUser => TypedResults.Conflict(
                "An account already exists for this e-mail."
            ),
            JoinResultKind.InvalidPassword => TypedResults.BadRequest(
                string.Join(" ", result.Errors)
            ),
            _ => TypedResults.BadRequest("The invitation is invalid or has expired."),
        };
    }

    private sealed record InvitationRequest(string Email, string Role);

    private sealed record JoinRequest(string Password);
}
