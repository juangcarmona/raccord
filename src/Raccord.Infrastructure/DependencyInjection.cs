using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Raccord.Application.Abstractions;
using Raccord.Application.Auditing;
using Raccord.Application.Compliance;
using Raccord.Application.Customers;
using Raccord.Application.Documents;
using Raccord.Application.Invoicing;
using Raccord.Application.Organization;
using Raccord.Application.Projects;
using Raccord.Infrastructure.Compliance;
using Raccord.Infrastructure.Documents;
using Raccord.Infrastructure.Persistence;
using Raccord.Infrastructure.Persistence.Repositories;
using Raccord.Infrastructure.Time;

namespace Raccord.Infrastructure;

/// <summary>Registers the adapters behind the application ports.</summary>
public static class DependencyInjection
{
    /// <summary>Connection name of the PostgreSQL database.</summary>
    public const string DatabaseConnectionName = "raccord";

    /// <summary>Connection name of the Azure Blob document storage.</summary>
    public const string DocumentsConnectionName = "documents";

    /// <summary>Adds persistence, document storage, rendering, compliance and time adapters.</summary>
    public static IHostApplicationBuilder AddRaccordInfrastructure(this IHostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.AddNpgsqlDbContext<RaccordDbContext>(DatabaseConnectionName);

        var services = builder.Services;
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IInvoiceDraftRepository, InvoiceDraftRepository>();
        services.AddScoped<IIssuedInvoiceRepository, IssuedInvoiceRepository>();
        services.AddScoped<IInvoiceSeriesRepository, InvoiceSeriesRepository>();
        services.AddScoped<IIssuerProfileRepository, IssuerProfileRepository>();
        services.AddScoped<IAuditTrail, AuditTrail>();
        services.AddScoped<IComplianceRecorder, PendingComplianceRecorder>();
        services.AddSingleton<IInvoiceDocumentRenderer, PdfInvoiceRenderer>();
        services.AddScoped<DatabaseMaintenance>();

        var timeZoneId = builder.Configuration["Raccord:TimeZone"] ?? "Europe/Madrid";
        services.AddSingleton(TimeProvider.System);
        services.AddSingleton<IClock>(provider => new SystemClock(provider.GetRequiredService<TimeProvider>(), TimeZoneInfo.FindSystemTimeZoneById(timeZoneId)));

        var storage = builder.Configuration.GetSection(StorageOptions.Section).Get<StorageOptions>() ?? new StorageOptions();
        if (string.Equals(storage.Provider, "AzureBlob", StringComparison.OrdinalIgnoreCase))
        {
            builder.AddAzureBlobServiceClient(DocumentsConnectionName);
            services.AddSingleton<IDocumentStore>(provider => new AzureBlobDocumentStore(provider.GetRequiredService<BlobServiceClient>(), storage.ContainerName));
        }
        else
        {
            services.AddSingleton<IDocumentStore>(new FileSystemDocumentStore(storage.FileSystemRoot));
        }

        return builder;
    }
}
