namespace Raccord.Application.Invoicing;

/// <summary>A downloadable invoice document.</summary>
/// <param name="FileName">Suggested file name.</param>
/// <param name="ContentType">Media type.</param>
/// <param name="Content">Document bytes.</param>
public sealed record InvoiceDocument(string FileName, string ContentType, byte[] Content);
