namespace Raccord.Infrastructure;

/// <summary>Document storage configuration (section <c>Storage</c>).</summary>
public sealed class StorageOptions
{
    /// <summary>Configuration section name.</summary>
    public const string Section = "Storage";

    /// <summary><c>FileSystem</c> (default, for self-hosting with a volume) or <c>AzureBlob</c>.</summary>
    public string Provider { get; set; } = "FileSystem";

    /// <summary>Root directory used by the file-system provider.</summary>
    public string FileSystemRoot { get; set; } = "data/documents";

    /// <summary>Container used by the Azure Blob provider; the connection is named <c>documents</c>.</summary>
    public string ContainerName { get; set; } = "documents";
}
