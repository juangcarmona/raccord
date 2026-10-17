using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Raccord.Infrastructure.Persistence;

namespace Raccord.Web.Authorization;

internal sealed class TestingAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder
) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var userId = Guid.TryParse(Request.Headers["X-Test-UserId"], out var requestedUserId)
            ? requestedUserId
            : Guid.Empty;
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Name, "test-admin@example.com"),
        };
        if (!Request.Headers.ContainsKey("X-Test-NoPermissions"))
            claims.Add(new Claim(PermissionCatalog.ClaimType, PermissionCatalog.All));
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        return Task.FromResult(
            AuthenticateResult.Success(
                new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name)
            )
        );
    }
}
