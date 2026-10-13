namespace Raccord.Application.Commercial;

/// <summary>A deterministic downloadable quote representation.</summary>
public sealed record QuoteDocument(string FileName, string ContentType, byte[] Content);
