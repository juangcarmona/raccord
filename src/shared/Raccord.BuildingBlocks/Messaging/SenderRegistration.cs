using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Raccord.BuildingBlocks.Messaging;

/// <summary>Explicit dependency-injection registration for a request and its handler.</summary>
public static class SenderRegistration
{
    /// <summary>Registers the sender infrastructure.</summary>
    public static IServiceCollection AddSender(this IServiceCollection services)
    {
        services.AddScoped<ISender, Sender>();
        services.AddSingleton<IExpectedExceptionMapper, SharedKernelExceptionMapper>();
        return services;
    }

    /// <summary>Registers one request handler and its optional validators in order.</summary>
    public static IServiceCollection AddRequest<TRequest, TResponse, THandler>(
        this IServiceCollection services,
        params Action<IServiceCollection>[] validators
    )
        where TRequest : IRequest<TResponse>
        where THandler : class, IRequestHandler<TRequest, TResponse>
    {
        services.AddScoped<IRequestHandler<TRequest, TResponse>, THandler>();
        foreach (var validator in validators)
        {
            validator(services);
        }

        return services;
    }

    /// <summary>Registers a FluentValidation validator for a request.</summary>
    public static Action<IServiceCollection> Validator<TRequest>(
        Func<IServiceProvider, IValidator<TRequest>> factory
    ) => services => services.AddScoped(factory);
}
