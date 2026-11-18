using Aspire.Hosting.ApplicationModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var externalTestServices =
    builder.Environment.IsEnvironment("Testing")
    && !string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString("raccord"));
IResourceBuilder<IResourceWithConnectionString> database = externalTestServices
    ? builder.AddConnectionString("raccord")
    : builder
        .AddPostgres("postgres")
        .WithDataVolume("raccord-postgres-data")
        .AddDatabase("raccord");

// One-shot schema migration plus fictional demo data; the API starts only after it completes.
var migrations = builder
    .AddProject<Projects.Raccord_Web>("migrations")
    .WithArgs(
        builder.Environment.IsEnvironment("Testing")
        && builder.Configuration.GetValue<bool>("Raccord:FreshInstallation")
            ? ["migrate"]
            : ["migrate", "--seed-demo"]
    )
    .WithReference(database)
    .WaitFor(database);

var web = builder
    .AddProject<Projects.Raccord_Web>("web")
    .WithHttpEndpoint(name: "http")
    .WithReference(database)
    .WaitFor(database)
    .WaitForCompletion(migrations)
    // Pinned to the http endpoint: when both schemes are modelled (the testing builder runs
    // projects with --no-launch-profile, so Aspire models http and https), the check prefers
    // https — which fails forever on a CI runner where nothing trusts the self-signed dev
    // cert, stalling every resource that waits on web.
    .WithHttpHealthCheck("/health/ready", endpointName: "http");

if (externalTestServices)
{
    web.WithEnvironment("Storage__Provider", "FileSystem")
        .WithEnvironment(
            "Storage__FileSystemRoot",
            builder.Configuration["Storage:FileSystemRoot"]!
        );
}
else
{
    // Azurite emulates Azure Blob Storage locally, the same API the Azure deployment uses.
    var documents = builder
        .AddAzureStorage("storage")
        .RunAsEmulator(emulator => emulator.WithDataVolume("raccord-azurite-data"))
        .AddBlobs("documents");
    web.WithReference(documents)
        .WithEnvironment("Storage__Provider", "AzureBlob")
        .WaitFor(documents);
}

builder
    .AddViteApp("frontend", "../../Raccord.Frontend")
    .WithNpm(install: true, installCommand: "ci")
    .WithReference(web)
    .WaitFor(web);

builder.Build().Run();
