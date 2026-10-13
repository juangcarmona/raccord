#pragma warning disable CS1591
namespace Raccord.Contracts.Customers;

public sealed record ContactTaxonomyRequest(IReadOnlyCollection<string> Tags, string? Folder);

public sealed record ContactTaxonomyResponse(
    Guid ContactId,
    IReadOnlyList<string> Tags,
    string? Folder
);
#pragma warning restore CS1591
