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
    public async Task<EquipmentSaleResponse?> SellAsync(
        Guid itemId,
        SellEquipmentRequest request,
        CancellationToken ct
    )
    {
        if (
            request.Quantity <= 0
            || string.IsNullOrWhiteSpace(request.SaleCode)
            || string.IsNullOrWhiteSpace(request.SaleName)
        )
            return null;
        var item = await db
            .EquipmentItems.FirstOrDefaultAsync(row => row.Id == itemId, ct)
            .ConfigureAwait(false);
        if (
            item is null
            || item.IsArchived
            || item.Type != EquipmentItemType.Rental
            || request.Quantity > item.StockQuantity
            || request.ExpectedStockQuantity is not null
                && request.ExpectedStockQuantity != item.StockQuantity
        )
            return null;
        var saleItem = new EquipmentItemRow
        {
            Id = Guid.NewGuid(),
            Code = request.SaleCode.Trim(),
            Name = request.SaleName.Trim(),
            Type = EquipmentItemType.Sale,
            StockQuantity = 0,
            SerialNumbers = "[]",
            AccessoryIds = "[]",
            AlternativeIds = "[]",
            KitContent = "[]",
        };
        var movement = NewMovement(item, -request.Quantity, "Sale", request.Description);
        item.StockQuantity -= request.Quantity;
        db.EquipmentItems.Add(saleItem);
        db.EquipmentStockMovements.Add(movement);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return new(ToSnapshot(item), ToSnapshot(saleItem), ToMovementSnapshot(movement));
    }

    public async Task<EquipmentItemSnapshot?> ArchiveAsync(
        Guid itemId,
        int expectedStockQuantity,
        CancellationToken ct
    )
    {
        var item = await db
            .EquipmentItems.FirstOrDefaultAsync(row => row.Id == itemId, ct)
            .ConfigureAwait(false);
        if (item is null || item.IsArchived || item.StockQuantity != expectedStockQuantity)
            return null;
        var movement = NewMovement(item, -item.StockQuantity, "Archive", "Archived");
        item.StockQuantity = 0;
        item.IsArchived = true;
        db.EquipmentStockMovements.Add(movement);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return ToSnapshot(item);
    }

    public async Task<IReadOnlyList<EquipmentProjectHistoryResponse>> ProjectHistoryAsync(
        IReadOnlyList<Guid> itemIds,
        CancellationToken ct
    )
    {
        var ids = itemIds.Distinct().ToArray();
        if (ids.Length == 0)
            return [];
        return await db
            .Projects.AsNoTracking()
            .Where(project =>
                project.PlannedEquipmentLines.Count(line => ids.Contains(line.EquipmentId))
                == ids.Length
            )
            .SelectMany(project =>
                project
                    .PlannedEquipmentLines.Where(line => ids.Contains(line.EquipmentId))
                    .GroupBy(line => new
                    {
                        project.Id,
                        project.Name,
                        project.Status,
                        project.StartDate,
                        project.EndDate,
                    })
                    .Select(group => new EquipmentProjectHistoryResponse(
                        group.Key.Id,
                        group.Key.Name,
                        group.Key.Status.ToString(),
                        group.Key.StartDate,
                        group.Key.EndDate,
                        group.Sum(line => line.Quantity)
                    ))
            )
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<StockMovementSnapshot?> ScheduleStockMovementAsync(
        Guid itemId,
        ScheduleStockMovementRequest request,
        CancellationToken ct
    )
    {
        if (request.Quantity == 0)
            return null;
        var item = await db
            .EquipmentItems.FirstOrDefaultAsync(row => row.Id == itemId, ct)
            .ConfigureAwait(false);
        if (item is null || item.KitKind == EquipmentKitKind.Virtual)
            return null;
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        if (request.EffectiveDate < today)
            return null;
        var movement = new EquipmentStockMovementRow
        {
            Id = Guid.NewGuid(),
            EquipmentItemId = itemId,
            Quantity = request.Quantity,
            StockQuantityAfter = item.StockQuantity + request.Quantity,
            EffectiveDate = request.EffectiveDate,
            Kind = "Scheduled",
            Description = request.Description?.Trim(),
            CreatedAt = DateTimeOffset.UtcNow,
        };
        db.EquipmentStockMovements.Add(movement);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return ToMovementSnapshot(movement);
    }

    public async Task<StockMovementSnapshot?> CorrectStockAsync(
        Guid itemId,
        CorrectStockRequest request,
        CancellationToken ct
    )
    {
        if (request.StockQuantity < 0)
            return null;
        var item = await db
            .EquipmentItems.FirstOrDefaultAsync(row => row.Id == itemId, ct)
            .ConfigureAwait(false);
        if (
            item is null
            || item.KitKind == EquipmentKitKind.Virtual
            || item.StockQuantity != request.ExpectedStockQuantity
        )
            return null;
        var movement = NewMovement(
            item,
            request.StockQuantity - item.StockQuantity,
            "Correction",
            request.Description
        );
        item.StockQuantity = request.StockQuantity;
        db.EquipmentStockMovements.Add(movement);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return ToMovementSnapshot(movement);
    }

    public async Task<IReadOnlyList<StockCountResult>?> CompleteStockCountAsync(
        CompleteStockCountRequest request,
        CancellationToken ct
    )
    {
        if (
            string.IsNullOrWhiteSpace(request.Name)
            || request.Items.Count == 0
            || request.Items.Any(line => line.CountedQuantity < 0)
        )
            return null;
        var ids = request.Items.Select(line => line.ItemId).Distinct().ToArray();
        var items = await db
            .EquipmentItems.Where(item => ids.Contains(item.Id))
            .ToDictionaryAsync(item => item.Id, ct)
            .ConfigureAwait(false);
        if (
            items.Count != ids.Length
            || items.Values.Any(item => item.KitKind == EquipmentKitKind.Virtual)
        )
            return null;
        var results = new List<StockCountResult>(request.Items.Count);
        foreach (var line in request.Items)
        {
            var item = items[line.ItemId];
            if (item.StockQuantity != line.ExpectedStockQuantity)
                return null;
            var difference = line.CountedQuantity - item.StockQuantity;
            var movement = NewMovement(
                item,
                difference,
                "Count",
                request.Description ?? request.Name
            );
            item.StockQuantity = line.CountedQuantity;
            db.EquipmentStockMovements.Add(movement);
            results.Add(
                new(
                    line.ItemId,
                    line.ExpectedStockQuantity,
                    line.CountedQuantity,
                    difference,
                    item.StockQuantity
                )
            );
        }
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return results;
    }

    public async Task<IReadOnlyList<StockMovementSnapshot>> ListStockMovementsAsync(
        Guid itemId,
        CancellationToken ct
    )
    {
        var movements = await db
            .EquipmentStockMovements.AsNoTracking()
            .Where(movement => movement.EquipmentItemId == itemId)
            .OrderByDescending(movement => movement.EffectiveDate)
            .ThenByDescending(movement => movement.CreatedAt)
            .ToListAsync(ct)
            .ConfigureAwait(false);
        return movements.Select(ToMovementSnapshot).ToArray();
    }

    private static EquipmentStockMovementRow NewMovement(
        EquipmentItemRow item,
        int quantity,
        string kind,
        string? description
    ) =>
        new()
        {
            Id = Guid.NewGuid(),
            EquipmentItemId = item.Id,
            Quantity = quantity,
            StockQuantityAfter = item.StockQuantity + quantity,
            EffectiveDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Kind = kind,
            Description = description?.Trim(),
            CreatedAt = DateTimeOffset.UtcNow,
        };

    private static StockMovementSnapshot ToMovementSnapshot(EquipmentStockMovementRow movement) =>
        new(
            movement.Id,
            movement.EquipmentItemId,
            movement.Quantity,
            movement.StockQuantityAfter,
            movement.EffectiveDate,
            movement.Kind,
            movement.Description,
            movement.CreatedAt
        );

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
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var scheduledQuantity =
            await db
                .EquipmentStockMovements.AsNoTracking()
                .Where(movement =>
                    movement.EquipmentItemId == itemId
                    && movement.EffectiveDate > today
                    && movement.EffectiveDate <= endDate
                )
                .SumAsync(movement => (int?)movement.Quantity, ct)
                .ConfigureAwait(false)
            ?? 0;
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
            item.StockQuantity + scheduledQuantity,
            softHeld,
            reserved,
            item.StockQuantity + scheduledQuantity - reserved - softHeld
        );
    }

    public async Task<IReadOnlyList<EquipmentShortageResponse>> ListShortagesAsync(
        CancellationToken ct
    )
    {
        var items = await db
            .EquipmentItems.AsNoTracking()
            .ToDictionaryAsync(item => item.Id, ct)
            .ConfigureAwait(false);
        var projects = await db
            .Projects.AsNoTracking()
            .Where(project => project.Status != ProjectStatus.Cancelled)
            .ToListAsync(ct)
            .ConfigureAwait(false);
        var result = new List<EquipmentShortageResponse>();
        foreach (var project in projects)
        foreach (var line in project.PlannedEquipmentLines)
        {
            if (!items.TryGetValue(line.EquipmentId, out var item))
                continue;
            var committed = projects
                .SelectMany(candidate =>
                    candidate
                        .PlannedEquipmentLines.Where(candidateLine =>
                            candidateLine.EquipmentId == line.EquipmentId
                            && candidateLine.StartDate <= line.EndDate
                            && candidateLine.EndDate >= line.StartDate
                        )
                        .Select(candidateLine =>
                            candidate.Status is ProjectStatus.Active or ProjectStatus.Completed
                            || candidate.Status is ProjectStatus.Draft or ProjectStatus.Planned
                                ? candidateLine.Quantity
                                : 0
                        )
                )
                .Sum();
            var available = item.StockQuantity - committed;
            if (available < 0)
                result.Add(
                    new(
                        project.Id,
                        line.Id,
                        line.EquipmentId,
                        line.Quantity,
                        available,
                        ParseIds(item.AlternativeIds)
                    )
                );
        }
        return result;
    }

    public async Task<bool> ResolveShortageAsync(
        Guid lineId,
        Guid alternativeId,
        CancellationToken ct
    )
    {
        var alternative = await db
            .EquipmentItems.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == alternativeId, ct)
            .ConfigureAwait(false);
        var project = (
            await db
                .Projects.Where(candidate =>
                    candidate.PlannedEquipmentLines.Any(line => line.Id == lineId)
                )
                .ToListAsync(ct)
                .ConfigureAwait(false)
        ).SingleOrDefault();
        var line = project?.PlannedEquipmentLines.SingleOrDefault(candidate =>
            candidate.Id == lineId
        );
        if (alternative is null || project is null || line is null)
            return false;
        var source = await db
            .EquipmentItems.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == line.EquipmentId, ct)
            .ConfigureAwait(false);
        if (source is null || !ParseIds(source.AlternativeIds).Contains(alternativeId))
            return false;
        line.ReplaceEquipment(alternativeId);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return true;
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
            ParseKitContent(row.KitContent),
            row.IsArchived
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
