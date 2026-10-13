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

    /// <summary>Actor who packed the line.</summary>
    public string? PackedBy { get; private set; }

    /// <summary>Trusted time at which the line was packed.</summary>
    public DateTimeOffset? PackedAt { get; private set; }

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
}
