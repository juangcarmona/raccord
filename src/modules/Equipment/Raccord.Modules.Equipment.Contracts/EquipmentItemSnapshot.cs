#pragma warning disable CS1591, MA0048

namespace Raccord.Modules.Equipment.Contracts;

/// <summary>Stable equipment identity published for consumers such as Projects.</summary>
public enum EquipmentItemType
{
    Rental,
    Sale,
}

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
    IReadOnlyList<Guid>? AlternativeIds = null
);

public sealed record UpdateEquipmentItemDetailsRequest(
    string? Details,
    IReadOnlyList<Guid>? AccessoryIds,
    IReadOnlyList<Guid>? AlternativeIds
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

#pragma warning restore CS1591, MA0048
