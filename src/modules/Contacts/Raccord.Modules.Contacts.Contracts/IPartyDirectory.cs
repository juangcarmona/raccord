namespace Raccord.Modules.Contacts.Contracts;

/// <summary>Reads party snapshots and stages newly registered customers.</summary>
public interface IPartyDirectory
{
    /// <summary>Finds a party by identifier.</summary>
    Task<PartySnapshot?> FindAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>Lists parties ordered by legal name.</summary>
    Task<IReadOnlyList<PartySnapshot>> ListAsync(CancellationToken cancellationToken);
}
