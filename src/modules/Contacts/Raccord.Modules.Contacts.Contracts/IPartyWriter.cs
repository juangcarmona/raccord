namespace Raccord.Modules.Contacts.Contracts;

/// <summary>Persistence port used by the Contacts use cases.</summary>
public interface IPartyWriter
{
    /// <summary>Adds a customer to the current unit of work.</summary>
    void Add(PartySnapshot party);
}
