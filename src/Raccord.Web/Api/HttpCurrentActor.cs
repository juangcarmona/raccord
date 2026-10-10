using Raccord.Application.Abstractions;

namespace Raccord.Web.Api;

/// <summary>
/// The actor recorded in audit events. Until authentication exists (see arc42 §11) every request is attributed
/// to <c>unauthenticated</c>, which makes the gap visible in the audit trail instead of hiding it.
/// </summary>
internal sealed class HttpCurrentActor(IHttpContextAccessor accessor) : ICurrentActor
{
    public string Name =>
        accessor.HttpContext?.User.Identity is { IsAuthenticated: true, Name: { } name }
            ? name
            : "unauthenticated";
}
