namespace Raccord.Modules.Workspace.Contracts;

/// <summary>Invoice series definition published by Workspace.</summary>
public sealed record InvoiceSeriesSnapshot(string Code, string Description, bool IsDefault);
