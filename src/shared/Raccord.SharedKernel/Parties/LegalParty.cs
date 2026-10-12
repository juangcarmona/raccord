namespace Raccord.SharedKernel.Parties;

/// <summary>The legal identity of an invoice issuer or recipient at a point in time.</summary>
/// <param name="LegalName">Registered legal name.</param>
/// <param name="TaxId">Tax identification number (NIF in Spain).</param>
/// <param name="Address">Registered or billing address.</param>
public sealed record LegalParty(string LegalName, string? TaxId, PostalAddress? Address);
