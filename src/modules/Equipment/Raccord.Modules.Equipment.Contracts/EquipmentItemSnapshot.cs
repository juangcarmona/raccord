namespace Raccord.Modules.Equipment.Contracts;

/// <summary>Stable equipment identity published for consumers such as Projects.</summary>
public sealed record EquipmentItemSnapshot(Guid Id, string Code, string Name);
