using Raccord.Domain.Billing;

namespace Raccord.Modules.Compliance.Contracts;

/// <summary>Registers issued invoices for compliance processing in the issuing transaction.</summary>
public interface IComplianceRecorder
{
    /// <summary>Registers an issued invoice for compliance processing.</summary>
    void RegisterIssuedInvoice(IssuedInvoice invoice, IssuedInvoiceRepresentation representation);
}
