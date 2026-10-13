var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres").WithDataVolume("raccord-postgres-data");
var database = postgres.AddDatabase("raccord");

// Azurite emulates Azure Blob Storage locally, the same API the Azure deployment uses.
var documents = builder
    .AddAzureStorage("storage")
    .RunAsEmulator(emulator => emulator.WithDataVolume("raccord-azurite-data"))
    .AddBlobs("documents");

// One-shot schema migration; the API starts only after it completes.
var migrations = builder
    .AddProject<Projects.Raccord_Web>("migrations")
    .WithArgs("migrate")
    .WithReference(database)
    .WaitFor(database);

var web = builder
    .AddProject<Projects.Raccord_Web>("web")
    .WithReference(database)
    .WithReference(documents)
    .WithEnvironment("Storage__Provider", "AzureBlob")
    .WaitFor(database)
    .WaitFor(documents)
    .WaitForCompletion(migrations)
    // Pinned to the http endpoint: when both schemes are modelled (the testing builder runs
    // projects with --no-launch-profile, so Aspire models http and https), the check prefers
    // https — which fails forever on a CI runner where nothing trusts the self-signed dev
    // cert, stalling every resource that waits on web.
    .WithHttpHealthCheck("/health/ready", endpointName: "http");

builder
    .AddViteApp("frontend", "../Raccord.Frontend")
    .WithNpm(install: true, installCommand: "ci")
    .WithReference(web)
    .WaitFor(web);

builder.Build().Run();
