using System.Text.Json.Serialization;

namespace Raccord.Modules.Contacts.Contracts;

/// <summary>The legal identity type of a directory contact.</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ContactType
{
    /// <summary>A registered company or other organization.</summary>
    Company,

    /// <summary>A natural person acting in a private capacity.</summary>
    PrivatePerson,
}
