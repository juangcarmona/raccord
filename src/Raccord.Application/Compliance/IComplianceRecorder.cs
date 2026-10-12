using Raccord.Modules.Compliance.Contracts;

namespace Raccord.Application.Compliance;

/// <summary>
/// The seam between invoicing and tax compliance. It receives every issued invoice inside the issuing transaction,
/// so no invoice can exist without its compliance record. Submission to a tax authority happens behind this seam.
/// </summary>
public interface IComplianceRecorder : Raccord.Modules.Compliance.Contracts.IComplianceRecorder { }
