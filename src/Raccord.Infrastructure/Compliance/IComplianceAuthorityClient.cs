namespace Raccord.Infrastructure.Compliance;

internal interface IComplianceAuthorityClient
{
    Task<ComplianceAuthorityResponse> SubmitAsync(
        string idempotencyKey,
        string invoiceNumber,
        string representation,
        CancellationToken cancellationToken
    );
}
