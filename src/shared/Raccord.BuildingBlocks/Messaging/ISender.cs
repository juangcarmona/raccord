using ErrorOr;

namespace Raccord.BuildingBlocks.Messaging;

/// <summary>Dispatches explicitly registered requests.</summary>
public interface ISender
{
    /// <summary>Validates and dispatches a request.</summary>
    Task<ErrorOr<TResponse>> SendAsync<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken
    )
        where TRequest : IRequest<TResponse>;
}
