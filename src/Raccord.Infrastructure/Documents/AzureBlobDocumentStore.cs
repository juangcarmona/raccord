using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Raccord.Application.Documents;

namespace Raccord.Infrastructure.Documents;

/// <summary>Stores documents in an Azure Blob Storage container (Azurite locally, Azure Storage in the cloud).</summary>
internal sealed class AzureBlobDocumentStore(BlobServiceClient client, string containerName) : IDocumentStore
{
    private readonly BlobContainerClient container = client.GetBlobContainerClient(containerName);
    private volatile bool containerReady;

    public async Task SaveAsync(string key, byte[] content, string contentType, CancellationToken cancellationToken)
    {
        if (!containerReady)
        {
            await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            containerReady = true;
        }

        var options = new BlobUploadOptions { HttpHeaders = new BlobHttpHeaders { ContentType = contentType } };
        await container.GetBlobClient(key).UploadAsync(BinaryData.FromBytes(content), options, cancellationToken).ConfigureAwait(false);
    }

    public async Task<byte[]?> ReadAsync(string key, CancellationToken cancellationToken)
    {
        try
        {
            var result = await container.GetBlobClient(key).DownloadContentAsync(cancellationToken).ConfigureAwait(false);
            return result.Value.Content.ToArray();
        }
        catch (RequestFailedException exception) when (exception.Status == 404)
        {
            return null;
        }
    }
}
