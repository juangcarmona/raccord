#pragma warning disable CS1591

namespace Raccord.Domain.Projects;

public sealed class PlannedEquipmentLine
{
    private PlannedEquipmentLine() { }

    internal PlannedEquipmentLine(
        Guid equipmentId,
        int quantity,
        DateOnly startDate,
        DateOnly endDate
    )
    {
        Id = Guid.CreateVersion7();
        EquipmentId = equipmentId;
        Quantity = quantity;
        StartDate = startDate;
        EndDate = endDate;
    }

    public Guid Id { get; private set; }
    public Guid EquipmentId { get; private set; }
    public int Quantity { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }

    public void ReplaceEquipment(Guid equipmentId) => EquipmentId = equipmentId;

    public void ShiftDays(int days)
    {
        StartDate = StartDate.AddDays(days);
        EndDate = EndDate.AddDays(days);
    }
}

#pragma warning restore CS1591
