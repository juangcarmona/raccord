using ErrorOr;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Raccord.BuildingBlocks.Messaging;

namespace Raccord.BuildingBlocks.Tests.Messaging;

public sealed class SenderTests
{
    [Fact]
    public async Task Sender_Should_InvokeHandlerOnceForValidRequest()
    {
        var services = new ServiceCollection();
        services.AddSender();
        services.AddScoped<IRequestHandler<TestRequest, string>, TestHandler>();
        await using var provider = services.BuildServiceProvider();
        await using var scope = provider.CreateAsyncScope();

        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        var result = await sender.SendAsync<TestRequest, string>(
            new TestRequest("ok"),
            TestContext.Current.CancellationToken
        );

        result.IsError.ShouldBeFalse();
        result.Value.ShouldBe("ok");
        TestHandler.Invocations.ShouldBe(1);
    }

    [Fact]
    public async Task Sender_Should_NotInvokeHandlerWhenValidationFails()
    {
        TestHandler.Invocations = 0;
        var services = new ServiceCollection();
        services.AddSender();
        services.AddScoped<IRequestHandler<TestRequest, string>, TestHandler>();
        services.AddScoped<IValidator<TestRequest>, InvalidValidator>();
        await using var provider = services.BuildServiceProvider();
        await using var scope = provider.CreateAsyncScope();

        var result = await scope
            .ServiceProvider.GetRequiredService<ISender>()
            .SendAsync<TestRequest, string>(
                new TestRequest("bad"),
                TestContext.Current.CancellationToken
            );

        result.IsError.ShouldBeTrue();
        result.Errors.Single().Code.ShouldBe("request.invalid");
        TestHandler.Invocations.ShouldBe(0);
    }

    private sealed record TestRequest(string Value) : IRequest<string>;

    private sealed class TestHandler : IRequestHandler<TestRequest, string>
    {
        public static int Invocations { get; set; }

        public Task<ErrorOr<string>> HandleAsync(
            TestRequest request,
            CancellationToken cancellationToken
        )
        {
            Invocations++;
            cancellationToken.IsCancellationRequested.ShouldBeFalse();
            return Task.FromResult<ErrorOr<string>>(request.Value);
        }
    }

    private sealed class InvalidValidator : AbstractValidator<TestRequest>
    {
        public InvalidValidator() =>
            RuleFor(request => request.Value)
                .Must(value => string.Equals(value, "ok", StringComparison.Ordinal))
                .WithErrorCode("request.invalid")
                .WithMessage("The request is invalid.");
    }
}
