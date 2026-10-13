using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Raccord.Application.Abstractions;
using Raccord.Application.Auditing;
using Raccord.Infrastructure;
using Raccord.Infrastructure.Persistence;
using Raccord.Modules.Contacts;
using Raccord.Modules.Invoicing;
using Raccord.Modules.Projects;
using Raccord.Modules.Workspace;
using Raccord.SharedKernel.Abstractions;
using Raccord.Web.Api;
using Raccord.Web.Authorization;

var builder = WebApplication.CreateBuilder(args);
var requireAuthentication = !builder.Configuration.GetValue<bool>("Raccord:TestingBypassAuth");

builder.AddServiceDefaults();
builder.AddRaccordInfrastructure();
builder.Services.AddDataProtection().PersistKeysToDbContext<RaccordDbContext>();
builder
    .Services.AddIdentityCore<RaccordUser>(options =>
    {
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 12;
    })
    .AddSignInManager()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<RaccordDbContext>();
if (requireAuthentication)
{
    var authentication = builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme);
    authentication.AddIdentityCookies();
    var oidc = builder.Configuration.GetSection("Raccord:Authentication:Oidc");
    if (oidc.GetValue<bool>("Enabled"))
    {
        authentication.AddOpenIdConnect(
            "oidc",
            options =>
            {
                options.Authority =
                    oidc["Authority"]
                    ?? throw new InvalidOperationException("OIDC authority is required.");
                options.ClientId =
                    oidc["ClientId"]
                    ?? throw new InvalidOperationException("OIDC client id is required.");
                options.ClientSecret =
                    oidc["ClientSecret"]
                    ?? throw new InvalidOperationException("OIDC client secret is required.");
                options.ResponseType = "code";
                options.SaveTokens = false;
                options.SignInScheme = IdentityConstants.ApplicationScheme;
                options.CallbackPath = "/api/auth/external/callback";
            }
        );
    }
}
else
{
    builder
        .Services.AddAuthentication("Testing")
        .AddScheme<AuthenticationSchemeOptions, TestingAuthenticationHandler>("Testing", _ => { });
}
builder.Services.Configure<CookieAuthenticationOptions>(
    IdentityConstants.ApplicationScheme,
    options =>
    {
        options.Cookie.Name = "raccord.session";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    }
);
if (requireAuthentication)
{
    builder
        .Services.AddAuthorizationBuilder()
        .AddPermissionPolicies()
        .SetFallbackPolicy(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
}
else
{
    builder.Services.AddAuthorizationBuilder().AddPermissionPolicies();
}

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
builder.Services.AddScoped<InstallationSetup>();
builder.Services.AddScoped<InvitationService>();
builder.Services.AddScoped<WorkspacePreferencesService>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

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
app.UseAuthentication();
app.UseAuthorization();

app.MapOpenApi();
app.MapDefaultEndpoints();
app.MapRaccordAuthentication();
app.MapRaccordSetup();
app.MapRaccordRoles();
app.MapRaccordInvitations();
app.MapRaccordApi(requireAuthentication);

// Client-side routes fall back to the SPA; paths that look like files (assets) and server prefixes never do.
app.MapFallbackToFile("{*path:regex(^(?!api/|health/|openapi/).*$):nonfile}", "index.html");

await app.RunAsync();

/// <summary>Entry point; public so integration tests can host the application.</summary>
public partial class Program;
