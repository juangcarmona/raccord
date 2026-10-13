using System.Globalization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Raccord.BuildingBlocks.Storage;
using Raccord.Domain.Projects;
using Raccord.Modules.Equipment.Contracts;

namespace Raccord.Infrastructure.Persistence;

#pragma warning disable CS1591

public sealed class EquipmentCatalog(RaccordDbContext db, IDocumentStore documents)
{
    public async Task<EquipmentAvailabilityResponse?> GetAvailabilityAsync(
        Guid itemId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken ct
    )
    {
        if (endDate < startDate)
            return null;
        var item = await db
            .EquipmentItems.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == itemId, ct)
            .ConfigureAwait(false);
        if (item is null)
            return null;
        var lines = await db
            .Projects.AsNoTracking()
            .Where(project => project.Status != ProjectStatus.Cancelled)
            .SelectMany(project =>
                project.PlannedEquipmentLines.Select(line => new
                {
                    project.Status,
                    line.EquipmentId,
                    line.Quantity,
                    line.StartDate,
                    line.EndDate,
                })
            )
            .Where(line =>
                line.EquipmentId == itemId && line.StartDate <= endDate && line.EndDate >= startDate
            )
            .ToListAsync(ct)
            .ConfigureAwait(false);
        var reserved = lines
            .Where(line => line.Status is ProjectStatus.Active or ProjectStatus.Completed)
            .Sum(line => line.Quantity);
        var softHeld = lines
            .Where(line => line.Status is ProjectStatus.Draft or ProjectStatus.Planned)
            .Sum(line => line.Quantity);
        return new(
            itemId,
            startDate,
            endDate,
            item.StockQuantity,
            softHeld,
            reserved,
            item.StockQuantity - reserved - softHeld
        );
    }

    public async Task<EquipmentItemSnapshot> CreateAsync(
        CreateEquipmentItemRequest request,
        CancellationToken cancellationToken
    )
    {
        var row = new EquipmentItemRow
        {
            Id = Guid.NewGuid(),
            Code = request.Code.Trim(),
            Name = request.Name.Trim(),
            Type = request.Type,
            StockQuantity = request.StockQuantity,
            SerialNumbers = JsonSerializer.Serialize(request.SerialNumbers ?? []),
        };
        db.EquipmentItems.Add(row);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToSnapshot(row);
    }

    public async Task<EquipmentItemPage> ListAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken
    )
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);
        var query = db.EquipmentItems.AsNoTracking().OrderBy(item => item.Code);
        var total = await query.CountAsync(cancellationToken).ConfigureAwait(false);
        var rows = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return new EquipmentItemPage(rows.Select(ToSnapshot).ToArray(), page, pageSize, total);
    }

    private static EquipmentItemSnapshot ToSnapshot(EquipmentItemRow row) =>
        new(
            row.Id,
            row.Code,
            row.Name,
            row.Type,
            row.StockQuantity,
            ParseSerialNumbers(row.SerialNumbers),
            row.IsPlanned,
            row.Details,
            row.ImageKey,
            ParseIds(row.AccessoryIds),
            ParseIds(row.AlternativeIds),
            row.KitKind,
            ParseKitContent(row.KitContent)
        );

    public async Task<EquipmentItemSnapshot?> UpdateDetailsAsync(
        Guid id,
        UpdateEquipmentItemDetailsRequest request,
        CancellationToken ct
    )
    {
        var row = await db
            .EquipmentItems.FirstOrDefaultAsync(item => item.Id == id, ct)
            .ConfigureAwait(false);
        if (row is null)
            return null;
        row.Details = request.Details?.Trim();
        row.AccessoryIds = JsonSerializer.Serialize(request.AccessoryIds ?? []);
        row.AlternativeIds = JsonSerializer.Serialize(request.AlternativeIds ?? []);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return ToSnapshot(row);
    }

    public async Task<EquipmentItemSnapshot?> ConfigureKitAsync(
        Guid id,
        ConfigureEquipmentKitRequest request,
        CancellationToken ct
    )
    {
        if (request.Content.Any(line => line.Quantity <= 0 || line.ItemId == id))
            return null;
        var row = await db
            .EquipmentItems.FirstOrDefaultAsync(item => item.Id == id, ct)
            .ConfigureAwait(false);
        if (row is null)
            return null;
        var contentIds = request.Content.Select(line => line.ItemId).Distinct().ToArray();
        var existing = await db
            .EquipmentItems.CountAsync(item => contentIds.Contains(item.Id), ct)
            .ConfigureAwait(false);
        if (existing != contentIds.Length)
            return null;
        row.KitKind = request.Kind;
        row.KitContent = JsonSerializer.Serialize(request.Content);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return ToSnapshot(row);
    }

#pragma warning disable MA0051
    public async Task<EquipmentImportResponse> ImportCsvAsync(Stream content, CancellationToken ct)
    {
        using var reader = new StreamReader(content);
        var header = await reader.ReadLineAsync(ct).ConfigureAwait(false);
        if (
            !string.Equals(
                header,
                "code,name,type,stockQuantity",
                StringComparison.OrdinalIgnoreCase
            )
        )
            return new(0, 1, [new(1, false, "Expected header code,name,type,stockQuantity.")]);

        var results = new List<EquipmentImportRowResponse>();
        var seenCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var rowNumber = 1;
        while (await reader.ReadLineAsync(ct).ConfigureAwait(false) is { } line)
        {
            rowNumber++;
            var fields = line.Split(',', StringSplitOptions.None);
            if (
                fields.Length != 4
                || string.IsNullOrWhiteSpace(fields[0])
                || string.IsNullOrWhiteSpace(fields[1])
            )
            {
                results.Add(
                    new(rowNumber, false, "Rows require code, name, type, and stock quantity.")
                );
                continue;
            }
            if (!Enum.TryParse<EquipmentItemType>(fields[2].Trim(), true, out var type))
            {
                results.Add(new(rowNumber, false, "Type must be Rental or Sale."));
                continue;
            }
            if (
                !int.TryParse(fields[3].Trim(), CultureInfo.InvariantCulture, out var stock)
                || stock < 0
            )
            {
                results.Add(
                    new(rowNumber, false, "Stock quantity must be a non-negative integer.")
                );
                continue;
            }
            var code = fields[0].Trim();
            if (
                !seenCodes.Add(code)
                || await db
                    .EquipmentItems.AnyAsync(item => item.Code == code, ct)
                    .ConfigureAwait(false)
            )
            {
                results.Add(
                    new(rowNumber, false, "Code already exists in this import or catalogue.")
                );
                continue;
            }
            db.EquipmentItems.Add(
                new EquipmentItemRow
                {
                    Id = Guid.NewGuid(),
                    Code = code,
                    Name = fields[1].Trim(),
                    Type = type,
                    StockQuantity = stock,
                }
            );
            results.Add(new(rowNumber, true, null));
        }
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return new(
            results.Count(row => row.Imported),
            results.Count(row => !row.Imported),
            results
        );
    }
#pragma warning restore MA0051

    public async Task<EquipmentItemSnapshot?> SaveImageAsync(
        Guid id,
        string fileName,
        string contentType,
        byte[] content,
        CancellationToken ct
    )
    {
        if (
            content.Length is 0 or > 5_000_000
            || !contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)
        )
            return null;
        var row = await db
            .EquipmentItems.FirstOrDefaultAsync(item => item.Id == id, ct)
            .ConfigureAwait(false);
        if (row is null)
            return null;
        row.ImageKey = $"equipment/{id:N}/{Path.GetFileName(fileName)}";
        await documents.SaveAsync(row.ImageKey, content, contentType, ct).ConfigureAwait(false);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return ToSnapshot(row);
    }

    private static string[] ParseSerialNumbers(string value) =>
        JsonSerializer.Deserialize<string[]>(value) ?? [];

    private static Guid[] ParseIds(string value) => JsonSerializer.Deserialize<Guid[]>(value) ?? [];

    private static EquipmentKitContentLine[] ParseKitContent(string value) =>
        JsonSerializer.Deserialize<EquipmentKitContentLine[]>(value) ?? [];
}

#pragma warning restore CS1591
