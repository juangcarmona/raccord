var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres").WithDataVolume("raccord-postgres-data");
var database = postgres.AddDatabase("raccord");

// Azurite emulates Azure Blob Storage locally, the same API the Azure deployment uses.
var documents = builder
    .AddAzureStorage("storage")
    .RunAsEmulator(emulator => emulator.WithDataVolume("raccord-azurite-data"))
    .AddBlobs("documents");

// One-shot schema migration plus fictional demo data; the API starts only after it completes.
var migrations = builder
    .AddProject<Projects.Raccord_Web>("migrations")
    .WithArgs("migrate", "--seed-demo")
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
    .WithHttpHealthCheck("/health/ready");

builder
    .AddViteApp("frontend", "../Raccord.Frontend")
    .WithNpm(install: true, installCommand: "ci")
    .WithReference(web)
    .WaitFor(web);

builder.Build().Run();
