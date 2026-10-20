namespace Raccord.Modules.Contacts;

/// <summary>Unsupported or malformed contact import data.</summary>
public sealed class ContactImportFormatException(string code, string message) : Exception(message)
{
    /// <summary>Stable error code for the invalid import input.</summary>
    public string Code { get; } = code;
}
