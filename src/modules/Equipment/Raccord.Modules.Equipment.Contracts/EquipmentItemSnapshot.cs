#pragma warning disable CS1591, MA0048

namespace Raccord.Modules.Equipment.Contracts;

/// <summary>Stable equipment identity published for consumers such as Projects.</summary>
public enum EquipmentItemType
{
    Rental,
    Sale,
}

public enum EquipmentKitKind
{
    None,
    Physical,
    Virtual,
}

public sealed record EquipmentKitContentLine(Guid ItemId, int Quantity);

public sealed record EquipmentItemSnapshot(
    Guid Id,
    string Code,
    string Name,
    EquipmentItemType Type,
    int StockQuantity,
    IReadOnlyList<string> SerialNumbers,
    bool IsPlanned = false,
    string? Details = null,
    string? ImageKey = null,
    IReadOnlyList<Guid>? AccessoryIds = null,
    IReadOnlyList<Guid>? AlternativeIds = null,
    EquipmentKitKind KitKind = EquipmentKitKind.None,
    IReadOnlyList<EquipmentKitContentLine>? KitContent = null,
    bool IsArchived = false
);

public sealed record UpdateEquipmentItemDetailsRequest(
    string? Details,
    IReadOnlyList<Guid>? AccessoryIds,
    IReadOnlyList<Guid>? AlternativeIds
);

public sealed record ConfigureEquipmentKitRequest(
    EquipmentKitKind Kind,
    IReadOnlyList<EquipmentKitContentLine> Content
);

public sealed record CreateEquipmentItemRequest(
    string Code,
    string Name,
    EquipmentItemType Type,
    int StockQuantity,
    IReadOnlyList<string>? SerialNumbers = null
);

public sealed record UpdateEquipmentItemRequest(
    string Code,
    string Name,
    EquipmentItemType Type,
    int StockQuantity,
    IReadOnlyList<string>? SerialNumbers = null
);

public sealed record EquipmentItemPage(
    IReadOnlyList<EquipmentItemSnapshot> Items,
    int Page,
    int PageSize,
    int TotalCount
);

public sealed record EquipmentImportRowResponse(int Row, bool Imported, string? Error);

public sealed record EquipmentImportResponse(
    int ImportedCount,
    int RejectedCount,
    IReadOnlyList<EquipmentImportRowResponse> Rows
);

public sealed record EquipmentAvailabilityResponse(
    Guid ItemId,
    DateOnly StartDate,
    DateOnly EndDate,
    int StockQuantity,
    int SoftHeldQuantity,
    int ReservedQuantity,
    int AvailableQuantity
);

public sealed record EquipmentShortageResponse(
    Guid ProjectId,
    Guid LineId,
    Guid EquipmentId,
    int Quantity,
    int AvailableQuantity,
    IReadOnlyList<Guid> AlternativeIds
);

public sealed record ScheduleStockMovementRequest(
    int Quantity,
    DateOnly EffectiveDate,
    string? Description = null
);

public sealed record CorrectStockRequest(
    int StockQuantity,
    int ExpectedStockQuantity,
    string? Description = null
);

public sealed record StockCountLine(Guid ItemId, int ExpectedStockQuantity, int CountedQuantity);

public sealed record CompleteStockCountRequest(
    string Name,
    IReadOnlyList<StockCountLine> Items,
    string? Description = null
);

public sealed record StockMovementSnapshot(
    Guid Id,
    Guid ItemId,
    int Quantity,
    int StockQuantityAfter,
    DateOnly EffectiveDate,
    string Kind,
    string? Description,
    DateTimeOffset CreatedAt
);

public sealed record StockCountResult(
    Guid ItemId,
    int ExpectedStockQuantity,
    int CountedQuantity,
    int Difference,
    int StockQuantity
);

public sealed record SellEquipmentRequest(
    int Quantity,
    string SaleCode,
    string SaleName,
    string? Description = null,
    int? ExpectedStockQuantity = null
);

public sealed record EquipmentSaleResponse(
    EquipmentItemSnapshot RentalItem,
    EquipmentItemSnapshot SaleItem,
    StockMovementSnapshot Movement
);

public sealed record EquipmentProjectHistoryResponse(
    Guid ProjectId,
    string ProjectName,
    string Status,
    DateOnly StartDate,
    DateOnly EndDate,
    int Quantity
);

#pragma warning restore CS1591, MA0048
