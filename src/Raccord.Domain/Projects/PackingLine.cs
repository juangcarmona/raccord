using Raccord.SharedKernel.Common;

namespace Raccord.Domain.Projects;

/// <summary>A planned equipment quantity prepared for warehouse fulfillment.</summary>
public sealed class PackingLine
{
    private PackingLine() { }

    internal PackingLine(Guid equipmentId, int quantity)
    {
        Id = Guid.CreateVersion7();
        EquipmentId = equipmentId;
        Quantity = quantity;
    }

    /// <summary>Packing line identifier.</summary>
    public Guid Id { get; private set; }

    /// <summary>Planned equipment identifier.</summary>
    public Guid EquipmentId { get; private set; }

    /// <summary>Quantity to prepare.</summary>
    public int Quantity { get; private set; }

    /// <summary>Whether the line has been packed.</summary>
    public bool IsPacked { get; private set; }

    /// <summary>Whether the line has left the warehouse.</summary>
    public bool IsBookedOut { get; private set; }

    /// <summary>Whether the line has been received back.</summary>
    public bool IsReceived { get; private set; }

    /// <summary>Actor who packed the line.</summary>
    public string? PackedBy { get; private set; }

    /// <summary>Trusted time at which the line was packed.</summary>
    public DateTimeOffset? PackedAt { get; private set; }

    /// <summary>Actor who booked the line out.</summary>
    public string? BookedOutBy { get; private set; }

    /// <summary>Trusted time at which the line left the warehouse.</summary>
    public DateTimeOffset? BookedOutAt { get; private set; }

    /// <summary>Actor who received the line.</summary>
    public string? ReceivedBy { get; private set; }

    /// <summary>Trusted time at which the line was received.</summary>
    public DateTimeOffset? ReceivedAt { get; private set; }

    /// <summary>Marks the line packed once.</summary>
    public void Pack(string actor, DateTimeOffset packedAt)
    {
        if (string.IsNullOrWhiteSpace(actor))
            throw new BusinessRuleViolationException(
                "projects.packing-actor-required",
                "The packing actor is required."
            );
        if (IsPacked)
            throw new BusinessRuleViolationException(
                "projects.packing-already-complete",
                "This packing line is already packed."
            );
        IsPacked = true;
        PackedBy = actor.Trim();
        PackedAt = packedAt;
    }

    /// <summary>Books the packed line out once.</summary>
    public void BookOut(string actor, DateTimeOffset bookedOutAt)
    {
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
        if (string.IsNullOrWhiteSpace(actor))
            throw new BusinessRuleViolationException(
                "projects.book-out-actor-required",
                "The booking-out actor is required."
            );
        IsBookedOut = true;
        BookedOutBy = actor.Trim();
        BookedOutAt = bookedOutAt;
    }

    /// <summary>Receives the booked-out line once.</summary>
    public void Receive(string actor, DateTimeOffset receivedAt)
    {
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
        if (string.IsNullOrWhiteSpace(actor))
            throw new BusinessRuleViolationException(
                "projects.receive-actor-required",
                "The receiving actor is required."
            );
        IsReceived = true;
        ReceivedBy = actor.Trim();
        ReceivedAt = receivedAt;
    }
}
