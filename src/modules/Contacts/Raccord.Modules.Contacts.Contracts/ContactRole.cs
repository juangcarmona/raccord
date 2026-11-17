using System.Text.Json.Serialization;

namespace Raccord.Modules.Contacts.Contracts;

/// <summary>A supported business role carried by a directory contact.</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ContactRole
{
    /// <summary>A contact that can be selected as a project or invoice customer.</summary>
    Customer,

    /// <summary>A contact that supplies goods or services.</summary>
    Supplier,

    /// <summary>A contact that represents a project venue.</summary>
    Venue,
}
