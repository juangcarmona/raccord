using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;
using Raccord.Application.Documents;

namespace Raccord.BuildingBlocks.Storage;

/// <summary>Registers one document storage adapter from the existing storage options.</summary>
public static class DocumentStorageRegistration
{
    /// <summary>Registers the configured file-system or Azure Blob document store.</summary>
    public static IServiceCollection AddDocumentStorage(
        this IServiceCollection services,
        StorageOptions options
    )
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(options);
        if (string.Equals(options.Provider, "AzureBlob", StringComparison.OrdinalIgnoreCase))
        {
            services.AddSingleton<IDocumentStore>(provider => new AzureBlobDocumentStore(
                provider.GetRequiredService<BlobServiceClient>(),
                options.ContainerName
            ));
        }
        else
        {
            services.AddSingleton<IDocumentStore>(
                new FileSystemDocumentStore(options.FileSystemRoot)
            );
        }

        return services;
    }
}
