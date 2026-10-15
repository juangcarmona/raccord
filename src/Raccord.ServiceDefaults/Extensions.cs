using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Microsoft.Extensions.Hosting;

/// <summary>Shared hosting defaults: service discovery, resilience, health checks and OpenTelemetry.</summary>
public static class Extensions
{
    /// <summary>Readiness endpoint: every registered check, including the database, must pass.</summary>
    public const string ReadinessPath = "/health/ready";

    /// <summary>Liveness endpoint: only checks tagged <c>live</c>.</summary>
    public const string LivenessPath = "/health/live";

    /// <summary>Adds service discovery, resilience, health checks, and telemetry.</summary>
    public static TBuilder AddServiceDefaults<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        builder.ConfigureOpenTelemetry();
        builder.AddDefaultHealthChecks();
        builder.Services.AddServiceDiscovery();
        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            http.AddStandardResilienceHandler();
            http.AddServiceDiscovery();
        });

        return builder;
    }

    /// <summary>Configures logs, metrics and traces; exports over OTLP when an endpoint is configured.</summary>
    public static TBuilder ConfigureOpenTelemetry<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        builder
            .Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
            )
            .WithTracing(tracing =>
                tracing
                    .AddSource(builder.Environment.ApplicationName)
                    .AddAspNetCoreInstrumentation(options =>
                        options.Filter = context =>
                            !context.Request.Path.StartsWithSegments(
                                "/health",
                                StringComparison.Ordinal
                            )
                    )
                    .AddHttpClientInstrumentation()
            );

        if (!string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]))
        {
            builder.Services.AddOpenTelemetry().UseOtlpExporter();
        }

        return builder;
    }

    /// <summary>Adds the liveness check; integrations add readiness checks such as the database.</summary>
    public static TBuilder AddDefaultHealthChecks<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        builder
            .Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);
        return builder;
    }

    /// <summary>
    /// Maps liveness and readiness endpoints in every environment. They return only a status word, never details,
    /// so container orchestrators and load balancers can use them safely.
    /// </summary>
    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);
        app.MapHealthChecks(ReadinessPath).AllowAnonymous();
        app.MapHealthChecks(
                LivenessPath,
                new HealthCheckOptions { Predicate = check => check.Tags.Contains("live") }
            )
            .AllowAnonymous();
        return app;
    }
}
