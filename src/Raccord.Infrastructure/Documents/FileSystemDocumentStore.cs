using Raccord.Application.Documents;

namespace Raccord.Infrastructure.Documents;

/// <summary>Stores documents as files under a root directory; suited to single-server self-hosting with a mounted volume.</summary>
internal sealed class FileSystemDocumentStore : IDocumentStore
{
    private readonly string root;

    public FileSystemDocumentStore(string rootPath)
    {
        root = Path.GetFullPath(rootPath);
    }

    public async Task SaveAsync(string key, byte[] content, string contentType, CancellationToken cancellationToken)
    {
        var path = Resolve(key);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        var temporary = path + ".tmp";
        await File.WriteAllBytesAsync(temporary, content, cancellationToken).ConfigureAwait(false);
        File.Move(temporary, path, overwrite: true);
    }

    public async Task<byte[]?> ReadAsync(string key, CancellationToken cancellationToken)
    {
        var path = Resolve(key);
        return File.Exists(path) ? await File.ReadAllBytesAsync(path, cancellationToken).ConfigureAwait(false) : null;
    }

    private string Resolve(string key)
    {
        if (string.IsNullOrWhiteSpace(key) || Path.IsPathRooted(key) || key.Contains(':', StringComparison.Ordinal))
        {
            throw new ArgumentException("Document keys must be relative paths.", nameof(key));
        }

        var path = Path.GetFullPath(Path.Combine(root, key));
        if (!path.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.Ordinal))
        {
            throw new ArgumentException("Document keys cannot leave the storage root.", nameof(key));
        }

        return path;
    }
}
