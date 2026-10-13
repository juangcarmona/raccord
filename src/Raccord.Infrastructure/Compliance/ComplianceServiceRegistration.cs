using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Raccord.Infrastructure.Compliance;

internal static class ComplianceServiceRegistration
{
    public static IServiceCollection AddComplianceSubmission(this IServiceCollection services)
    {
        services.AddOptions<ComplianceOptions>().BindConfiguration(ComplianceOptions.Section);
        services
            .AddHttpClient(nameof(HttpComplianceAuthorityClient))
            .ConfigurePrimaryHttpMessageHandler(provider =>
            {
                var settings = provider.GetRequiredService<IOptions<ComplianceOptions>>().Value;
                var handler = new HttpClientHandler();
                if (!string.IsNullOrWhiteSpace(settings.CertificatePath))
                    handler.ClientCertificates.Add(
                        X509CertificateLoader.LoadPkcs12FromFile(
                            settings.CertificatePath,
                            settings.CertificatePassword
                        )
                    );
                return handler;
            });
        services.AddScoped<IComplianceAuthorityClient, HttpComplianceAuthorityClient>();
        return services.AddHostedService<ComplianceSubmissionWorker>();
    }
}
