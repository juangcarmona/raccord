namespace Raccord.Domain.Commercial;

/// <summary>Review lifecycle of a generated quote.</summary>
public enum QuoteReviewStatus
{
    /// <summary>Quote is being prepared.</summary>
    Draft,

    /// <summary>Quote can be sent for signing.</summary>
    ReadyForSignature,

    /// <summary>Customer acceptance has been recorded.</summary>
    Accepted,

    /// <summary>Signature metadata has been recorded.</summary>
    SignatureRecorded,
}
