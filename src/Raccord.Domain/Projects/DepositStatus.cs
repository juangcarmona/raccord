namespace Raccord.Domain.Projects;

/// <summary>Lifecycle of a refundable security deposit.</summary>
public enum DepositStatus
{
    /// <summary>No payment has been received.</summary>
    NotPaid,

    /// <summary>The customer has paid the deposit.</summary>
    Paid,

    /// <summary>The deposit has been returned.</summary>
    Returned,
}
