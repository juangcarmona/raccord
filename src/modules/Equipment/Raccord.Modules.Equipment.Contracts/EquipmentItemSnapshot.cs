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
    IReadOnlyList<EquipmentKitContentLine>? KitContent = null
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

#pragma warning restore CS1591, MA0048
