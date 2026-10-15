namespace Raccord.Modules.Projects.Contracts;

/// <summary>Coordinates equipment availability with project packing transitions.</summary>
public interface IEquipmentStock
{
    /// <summary>Books equipment out, optionally validating its scan code.</summary>
    Task BookOutAsync(
        Guid equipmentId,
        int quantity,
        string? scanCode,
        CancellationToken cancellationToken
    );

    /// <summary>Returns equipment to available stock.</summary>
    Task ReceiveAsync(Guid equipmentId, int quantity, CancellationToken cancellationToken);
}
