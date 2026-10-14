#pragma warning disable CS1591

namespace Raccord.Contracts.Documents;

public sealed record LetterheadSnapshot(
    Guid Id,
    string Name,
    string DocumentTypes,
    decimal PageWidthMillimetres,
    decimal PageHeightMillimetres,
    decimal MarginTopMillimetres,
    decimal MarginRightMillimetres,
    decimal MarginBottomMillimetres,
    decimal MarginLeftMillimetres,
    bool IsDefault,
    string StorageKey
);

public sealed record CreateLetterheadRequest(
    string Name,
    string DocumentTypes,
    decimal PageWidthMillimetres,
    decimal PageHeightMillimetres,
    decimal MarginTopMillimetres,
    decimal MarginRightMillimetres,
    decimal MarginBottomMillimetres,
    decimal MarginLeftMillimetres
);

#pragma warning restore CS1591
