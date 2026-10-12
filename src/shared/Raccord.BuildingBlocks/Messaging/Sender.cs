using ErrorOr;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Raccord.BuildingBlocks.Messaging;

internal sealed class Sender(
    IServiceProvider services,
    IEnumerable<IExpectedExceptionMapper> mappers
) : ISender
{
    public async Task<ErrorOr<TResponse>> SendAsync<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken
    )
        where TRequest : IRequest<TResponse>
    {
        ArgumentNullException.ThrowIfNull(request);
        var validators = services.GetServices<IValidator<TRequest>>();
        var validationErrors = new List<Error>();
        foreach (var validator in validators)
        {
            var result = await validator
                .ValidateAsync(request, cancellationToken)
                .ConfigureAwait(false);
            validationErrors.AddRange(
                result.Errors.Select(error => Error.Validation(error.ErrorCode, error.ErrorMessage))
            );
        }

        if (validationErrors.Count > 0)
        {
            return validationErrors;
        }

        try
        {
            var handler = services.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
            return await handler.HandleAsync(request, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            foreach (var mapper in mappers)
            {
                var mapped = mapper.TryMap(exception);
                if (mapped is null)
                {
                    continue;
                }

                return mapped.Error;
            }

            throw;
        }
    }
}
