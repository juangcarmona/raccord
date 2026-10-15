#pragma warning disable CS1591

namespace Raccord.Contracts.Equipment;

public sealed record CreateEquipmentItemRequest(string Code, string Name, int StockQuantity);

public sealed record AssignEquipmentScanCodeRequest(string ScanCode);

public sealed record EquipmentItemResponse(
    Guid Id,
    string Code,
    string Name,
    string? ScanCode,
    int StockQuantity,
    int AvailableQuantity
);

#pragma warning restore CS1591
