#pragma warning disable CS1591

namespace Raccord.Contracts.Equipment;

public sealed record CreateEquipmentItemRequest(string Code, string Name, int StockQuantity);

public sealed record AssignEquipmentScanCodeRequest(string ScanCode);

public sealed record RetireEquipmentRequest(string Reason);

public sealed record SellEquipmentRequest(decimal Amount, string? Buyer);

public sealed record ScheduleStockMovementRequest(
    string Direction,
    int Quantity,
    DateOnly ScheduledDate,
    string? Reference
);

public sealed record StockMovementResponse(
    Guid Id,
    Guid EquipmentId,
    string Direction,
    int Quantity,
    DateOnly ScheduledDate,
    string? Reference,
    string Status,
    DateTimeOffset CreatedAt
);

public sealed record ReconcileStockCountRequest(int CountedQuantity, string Reason);

public sealed record StockCountResponse(
    Guid Id,
    Guid EquipmentId,
    int CountedQuantity,
    int PreviousQuantity,
    int Difference,
    string Reason,
    DateTimeOffset CountedAt
);

public sealed record EquipmentItemResponse(
    Guid Id,
    string Code,
    string Name,
    string? ScanCode,
    int StockQuantity,
    int AvailableQuantity,
    string Status,
    DateTimeOffset? DisposedAt,
    string? DispositionReason,
    decimal? SaleAmount
);

public sealed record EquipmentProjectHistoryResponse(
    Guid ProjectId,
    string ProjectName,
    Guid PackingLineId,
    int Quantity,
    string Role,
    bool IsPacked,
    bool IsBookedOut,
    bool IsReceived
);

public sealed record EquipmentInsightsResponse(
    Guid EquipmentId,
    int MovementCount,
    int CountAdjustmentCount,
    int ProjectCount,
    int TotalProjectQuantity,
    string Status
);

#pragma warning restore CS1591
