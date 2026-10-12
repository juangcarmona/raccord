using System.Text.Json.Serialization;
using Raccord.Application.Abstractions;
using Raccord.Application.Auditing;
using Raccord.Application.Invoicing;
using Raccord.Infrastructure;
using Raccord.Infrastructure.Persistence;
using Raccord.Modules.Contacts;
using Raccord.Modules.Projects;
using Raccord.Modules.Workspace;
using Raccord.SharedKernel.Abstractions;
using Raccord.Web.Api;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddRaccordInfrastructure();

builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict
);
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, HttpCurrentActor>();
builder.Services.AddScoped<CustomerDirectory>();
builder.Services.AddScoped<ProjectWorkspace>();
builder.Services.AddScoped<InvoicingWorkflow>();
builder.Services.AddScoped<OrganizationSettings>();
builder.Services.AddScoped<AuditLog>();

var app = builder.Build();

if (args.Contains("migrate", StringComparer.Ordinal))
{
    // One-shot maintenance mode used by Docker Compose, Aspire and deployment pipelines.
    await using var scope = app.Services.CreateAsyncScope();
    var maintenance = scope.ServiceProvider.GetRequiredService<DatabaseMaintenance>();
    await maintenance.MigrateAsync(CancellationToken.None);
    if (
        args.Contains("--seed-demo", StringComparer.Ordinal)
        || app.Configuration.GetValue<bool>("Raccord:SeedDemoData")
    )
    {
        await maintenance.SeedDemoDataAsync(CancellationToken.None);
    }

    return;
}

app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapOpenApi();
app.MapDefaultEndpoints();
app.MapRaccordApi();

// Client-side routes fall back to the SPA; paths that look like files (assets) and server prefixes never do.
app.MapFallbackToFile("{*path:regex(^(?!api/|health/|openapi/).*$):nonfile}", "index.html");

await app.RunAsync();

/// <summary>Entry point; public so integration tests can host the application.</summary>
public partial class Program;
