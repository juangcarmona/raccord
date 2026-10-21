using Microsoft.AspNetCore.Identity;
using Raccord.Modules.Projects.Contracts;

namespace Raccord.Infrastructure.Persistence;

internal sealed class ProjectAccountManagerDirectory(UserManager<RaccordUser> users)
    : IProjectAccountManagerDirectory
{
    public async Task<IReadOnlyList<ProjectAccountManagerSnapshot>> ListAsync(
        CancellationToken cancellationToken
    )
    {
        cancellationToken.ThrowIfCancellationRequested();
        var admins = await users.GetUsersInRoleAsync("Admin").ConfigureAwait(false);
        var office = await users.GetUsersInRoleAsync("Office").ConfigureAwait(false);
        return
        [
            .. admins
                .Concat(office)
                .DistinctBy(user => user.Id)
                .OrderBy(user => user.DisplayName, StringComparer.OrdinalIgnoreCase)
                .ThenBy(user => user.Id)
                .Select(user => new ProjectAccountManagerSnapshot(
                    user.Id,
                    user.DisplayName ?? user.Email ?? user.UserName ?? user.Id.ToString()
                )),
        ];
    }

    public async Task<bool> IsEligibleAsync(Guid userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var user = await users.FindByIdAsync(userId.ToString()).ConfigureAwait(false);
        if (user is null)
            return false;

        var roles = await users.GetRolesAsync(user).ConfigureAwait(false);
        return roles.Contains("Admin", StringComparer.OrdinalIgnoreCase)
            || roles.Contains("Office", StringComparer.OrdinalIgnoreCase);
    }
}
