namespace Raccord.Application.Documents;

/// <summary>Binary document storage (object storage), addressed by key.</summary>
public interface IDocumentStore
{
    /// <summary>Stores a document, replacing any document with the same key.</summary>
    Task SaveAsync(
        string key,
        byte[] content,
        string contentType,
        CancellationToken cancellationToken
    );

    /// <summary>Reads a document, or returns null when it does not exist.</summary>
    Task<byte[]?> ReadAsync(string key, CancellationToken cancellationToken);
}
