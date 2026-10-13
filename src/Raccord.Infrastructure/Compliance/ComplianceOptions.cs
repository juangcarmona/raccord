namespace Raccord.Infrastructure.Compliance;

internal sealed class ComplianceOptions
{
    public const string Section = "Raccord:Compliance";

    public bool Enabled { get; init; }
    public string Endpoint { get; init; } = string.Empty;
    public string? CertificatePath { get; init; }
    public string? CertificatePassword { get; init; }
    public int PollSeconds { get; init; } = 30;
}
