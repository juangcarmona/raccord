#pragma warning disable CS1591, MA0048, MA0004

using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Raccord.Infrastructure.Persistence;

public sealed class InvitationService(
    RaccordDbContext db,
    UserManager<RaccordUser> users,
    RoleManager<IdentityRole<Guid>> roles
)
{
    public async Task<InvitationCreated?> CreateAsync(
        string email,
        string roleName,
        CancellationToken cancellationToken
    )
    {
        if (!await roles.RoleExistsAsync(roleName).ConfigureAwait(false))
        {
            return null;
        }

        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(32)).ToLowerInvariant();
        db.Set<Invitation>()
            .Add(
                new Invitation
                {
                    Email = email.Trim(),
                    RoleName = roleName,
                    TokenHash = Hash(token),
                    ExpiresAt = DateTimeOffset.UtcNow.AddDays(30),
                }
            );
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return new InvitationCreated(token, DateTimeOffset.UtcNow.AddDays(30));
    }

    public async Task<JoinResult> JoinAsync(
        string token,
        string password,
        CancellationToken cancellationToken
    )
    {
        var invitation = await db.Set<Invitation>()
            .SingleOrDefaultAsync(item => item.TokenHash == Hash(token), cancellationToken)
            .ConfigureAwait(false);
        if (
            invitation is null
            || invitation.AcceptedAt is not null
            || invitation.ExpiresAt <= DateTimeOffset.UtcNow
        )
        {
            return JoinResult.Invalid;
        }

        if (await users.FindByEmailAsync(invitation.Email).ConfigureAwait(false) is not null)
        {
            return JoinResult.ExistingUser;
        }

        await using var transaction = await db
            .Database.BeginTransactionAsync(cancellationToken)
            .ConfigureAwait(false);
        var user = new RaccordUser
        {
            UserName = invitation.Email,
            Email = invitation.Email,
            EmailConfirmed = true,
        };
        var created = await users.CreateAsync(user, password).ConfigureAwait(false);
        if (!created.Succeeded)
        {
            return JoinResult.InvalidPassword(created.Errors.Select(error => error.Description));
        }

        var assigned = await users.AddToRoleAsync(user, invitation.RoleName).ConfigureAwait(false);
        if (!assigned.Succeeded)
        {
            return JoinResult.InvalidPassword(assigned.Errors.Select(error => error.Description));
        }

        invitation.AcceptedAt = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
        return JoinResult.Joined;
    }

    private static string Hash(string token) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token))).ToLowerInvariant();
}

public sealed record InvitationCreated(string Token, DateTimeOffset ExpiresAt);

public sealed record JoinResult(JoinResultKind Kind, IReadOnlyList<string> Errors)
{
    public static JoinResult Invalid { get; } = new(JoinResultKind.Invalid, []);
    public static JoinResult ExistingUser { get; } = new(JoinResultKind.ExistingUser, []);
    public static JoinResult Joined { get; } = new(JoinResultKind.Joined, []);

    public static JoinResult InvalidPassword(IEnumerable<string> errors) =>
        new(JoinResultKind.InvalidPassword, errors.ToArray());
}

public enum JoinResultKind
{
    Invalid,
    ExistingUser,
    InvalidPassword,
    Joined,
}

#pragma warning restore CS1591, MA0048, MA0004
