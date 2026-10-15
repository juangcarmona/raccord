using Raccord.SharedKernel.Common;

#pragma warning disable CS1591

namespace Raccord.Domain.Projects;

public sealed class PackingLine
{
    private PackingLine() { }

    internal PackingLine(Guid equipmentId, int quantity, PackingLineRole role)
    {
        if (quantity <= 0)
            throw new BusinessRuleViolationException(
                "projects.packing-quantity-invalid",
                "Packing quantity must be positive."
            );
        Id = Guid.CreateVersion7();
        EquipmentId = equipmentId;
        Quantity = quantity;
        Role = role;
    }

    public static PackingLine Create(
        Guid equipmentId,
        int quantity,
        PackingLineRole role = PackingLineRole.Required
    ) => new(equipmentId, quantity, role);

    public Guid Id { get; private set; }
    public Guid EquipmentId { get; private set; }
    public int Quantity { get; private set; }
    public PackingLineRole Role { get; private set; }
    public bool IsPacked { get; private set; }
    public bool IsBookedOut { get; private set; }
    public bool IsReceived { get; private set; }
    public string? PackedBy { get; private set; }
    public DateTimeOffset? PackedAt { get; private set; }
    public string? BookedOutBy { get; private set; }
    public DateTimeOffset? BookedOutAt { get; private set; }
    public string? ReceivedBy { get; private set; }
    public DateTimeOffset? ReceivedAt { get; private set; }

    public void Adjust(int quantity, PackingLineRole role)
    {
        if (IsPacked || IsBookedOut || IsReceived)
            throw new BusinessRuleViolationException(
                "projects.packing-adjustment-locked",
                "Only planned packing lines can be adjusted."
            );
        if (quantity <= 0)
            throw new BusinessRuleViolationException(
                "projects.packing-quantity-invalid",
                "Packing quantity must be positive."
            );
        Quantity = quantity;
        Role = role;
    }

    public void Pack(string actor, DateTimeOffset at)
    {
        EnsureActor(actor, "projects.packing-actor-required");
        if (IsPacked)
            throw new BusinessRuleViolationException(
                "projects.packing-already-complete",
                "This packing line is already packed."
            );
        IsPacked = true;
        PackedBy = actor.Trim();
        PackedAt = at;
    }

    public void BookOut(string actor, DateTimeOffset at)
    {
        EnsureActor(actor, "projects.book-out-actor-required");
        if (!IsPacked)
            throw new BusinessRuleViolationException(
                "projects.book-out-not-packed",
                "Only packed equipment can be booked out."
            );
        if (IsBookedOut)
            throw new BusinessRuleViolationException(
                "projects.book-out-already-complete",
                "This packing line is already booked out."
            );
        IsBookedOut = true;
        BookedOutBy = actor.Trim();
        BookedOutAt = at;
    }

    public void Receive(string actor, DateTimeOffset at)
    {
        EnsureActor(actor, "projects.receive-actor-required");
        if (!IsBookedOut)
            throw new BusinessRuleViolationException(
                "projects.receive-not-booked-out",
                "Only booked-out equipment can be received."
            );
        if (IsReceived)
            throw new BusinessRuleViolationException(
                "projects.receive-already-complete",
                "This packing line is already received."
            );
        IsReceived = true;
        ReceivedBy = actor.Trim();
        ReceivedAt = at;
    }

    private static void EnsureActor(string actor, string code)
    {
        if (string.IsNullOrWhiteSpace(actor))
            throw new BusinessRuleViolationException(code, "An actor is required.");
    }
}

#pragma warning restore CS1591
