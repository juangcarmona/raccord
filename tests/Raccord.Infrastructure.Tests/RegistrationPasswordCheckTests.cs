using System.Net;
using Raccord.Infrastructure.Persistence;

namespace Raccord.Infrastructure.Tests;

public sealed class RegistrationPasswordCheckTests
{
    [Theory]
    [InlineData("1E4C9B93F3F0682250B6CF8331B7EE68FD8:5\r\n", "matched")]
    [InlineData("1E4C9B93F3F0682250B6CF8331B7EE68FD8:0\r\n", "clear")]
    [InlineData("00000000000000000000000000000000000:5\r\n", "clear")]
    [InlineData("bad response", "unavailable")]
    public async Task Check_Should_SendOnlyFiveHashCharactersAndCompareLocally(
        string body,
        string expected
    )
    {
        using var handler = new RangeHandler(body);
        using var client = new HttpClient(handler);
        var check = new RegistrationPasswordCheck(client);
        // A known public protocol vector, never an account credential.
        (await check.CheckAsync("password", CancellationToken.None)).ShouldBe(expected);
        handler.Url.ShouldBe("https://api.pwnedpasswords.com/range/5BAA6");
        handler.Padding.ShouldBeTrue();
        handler.HadContent.ShouldBeFalse();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task ProviderFailure_Should_BeUnavailableRatherThanClear(bool timeout)
    {
        using var handler = new RangeHandler("", fail: true, timeout);
        using var client = new HttpClient(handler);
        (
            await new RegistrationPasswordCheck(client).CheckAsync(
                "password",
                CancellationToken.None
            )
        ).ShouldBe("unavailable");
    }

    private sealed class RangeHandler(string body, bool fail = false, bool timeout = false)
        : HttpMessageHandler
    {
        public string? Url { get; private set; }
        public bool Padding { get; private set; }
        public bool HadContent { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken
        )
        {
            Url = request.RequestUri!.AbsoluteUri;
            Padding = string.Equals(
                request.Headers.GetValues("Add-Padding").Single(),
                "true",
                StringComparison.Ordinal
            );
            HadContent = request.Content is not null;
            if (timeout)
                throw new TaskCanceledException();
            if (fail)
                throw new HttpRequestException("Synthetic provider failure");
            return Task.FromResult(
                new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(body) }
            );
        }
    }
}
