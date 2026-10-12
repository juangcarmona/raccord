using ErrorOr;

namespace Raccord.BuildingBlocks.Messaging;

/// <summary>Handles one request type.</summary>
public interface IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>Handles the request.</summary>
    Task<ErrorOr<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken);
}
