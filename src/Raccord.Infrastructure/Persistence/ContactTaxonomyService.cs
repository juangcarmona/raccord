using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Raccord.Contracts.Customers;

#pragma warning disable CS1591

namespace Raccord.Infrastructure.Persistence;

public sealed class ContactTaxonomyService(RaccordDbContext db)
{
    public async Task<ContactTaxonomyResponse?> GetAsync(Guid id, CancellationToken ct) =>
        await db
            .Customers.Where(customer => customer.Id == id)
            .Select(customer => new ContactTaxonomyResponse(
                customer.Id,
                Parse(customer.Tags),
                customer.Folder
            ))
            .SingleOrDefaultAsync(ct)
            .ConfigureAwait(false);

    public async Task<ContactTaxonomyResponse?> SetAsync(
        Guid id,
        ContactTaxonomyRequest request,
        CancellationToken ct
    )
    {
        var customer = await db
            .Customers.SingleOrDefaultAsync(item => item.Id == id, ct)
            .ConfigureAwait(false);
        if (customer is null)
            return null;
        customer.Tags = JsonSerializer.Serialize(
            request
                .Tags.Where(tag => !string.IsNullOrWhiteSpace(tag))
                .Select(tag => tag.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Order(StringComparer.OrdinalIgnoreCase)
        );
        customer.Folder = string.IsNullOrWhiteSpace(request.Folder) ? null : request.Folder.Trim();
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return new ContactTaxonomyResponse(customer.Id, Parse(customer.Tags), customer.Folder);
    }

    private static string[] Parse(string? value) =>
        string.IsNullOrWhiteSpace(value) ? [] : JsonSerializer.Deserialize<string[]>(value) ?? [];
}

#pragma warning restore CS1591
