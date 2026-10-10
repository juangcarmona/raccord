using Raccord.Domain.Billing;

namespace Raccord.Application.Compliance;

/// <summary>
/// The seam between invoicing and tax compliance. It receives every issued invoice inside the issuing transaction,
/// so no invoice can exist without its compliance record. Submission to a tax authority happens behind this seam.
/// </summary>
public interface IComplianceRecorder
{
    /// <summary>Registers an issued invoice for compliance processing.</summary>
    void RegisterIssuedInvoice(IssuedInvoice invoice, IssuedInvoiceRepresentation representation);
}
